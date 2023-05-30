using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class ParameterUtil
    {
        // Method
        public static double? ParameterAsDouble(this Autodesk.Revit.DB.Element elem, string paramName)
        {
            return elem.LookupParameter(paramName)?.AsDouble();
        }

        public static int? ParameterAsInteger(this Autodesk.Revit.DB.Element elem, string paramName)
        {
            return elem.LookupParameter(paramName)?.AsInteger();
        }

        public static string? ParameterAsString(this Autodesk.Revit.DB.Element elem, string paramName)
        {
            return elem.LookupParameter(paramName)?.AsString();
        }

        public static Autodesk.Revit.DB.Element? ParameterAsElement(this Autodesk.Revit.DB.Element elem, string paramName)
        {
            var param = elem.LookupParameter(paramName);
            if (param == null)
            {
                return null;
            }

            var elemId = param.AsElementId();
            if (elemId == null)
            {
                return null;
            }
            return elemId.GetElement();
        }

        public static string? ParameterAsValueString(this Autodesk.Revit.DB.Element elem, string paramName)
        {
            var param = elem.LookupParameter(paramName);
            if (param == null) return null;

            switch (param.StorageType)
            {
                case StorageType.String:
                    return param.AsString();
                default:
                    return param.AsValueString();
            }
        }

        public static bool ParameterSet(this Autodesk.Revit.DB.Element elem, string paramName, object value)
        {
            var param = elem.LookupParameter(paramName);
            if (param == null)
            {
                return false;
            }

            if (value is int) elem.LookupParameter(paramName).Set((int)value);
            if (value is double) elem.LookupParameter(paramName).Set((double)value);
            if (value is string) elem.LookupParameter(paramName).Set((string)value);
            if (value is Autodesk.Revit.DB.Element) elem.LookupParameter(paramName).Set(((Autodesk.Revit.DB.Element)value).Id);
            if (value is Autodesk.Revit.DB.ElementId) elem.LookupParameter(paramName).Set((Autodesk.Revit.DB.ElementId)value);
            return true;
        }

        public static void CopyAllValues(Element sourceElement, Element targetElement)
        {
            var bic = targetElement.Category.GetBuiltInCategory();

            var isSource_UnconnectedHeight_ReadOnly = false;

            var sourceParameters = sourceElement.ParametersMap.Cast<Parameter>().Where(x =>
            {
                if (bic == BuiltInCategory.OST_Walls)
                {
                    if ((BuiltInParameter)x.Id.IntegerValue == BuiltInParameter.WALL_USER_HEIGHT_PARAM)
                    {
                        isSource_UnconnectedHeight_ReadOnly = x.IsReadOnly;
                    }
                }
                return !x.IsReadOnly;
            }).ToList();

            //var alltargetParameters = targetElement.ParametersMap.Cast<Parameter>();

            Parameter? needAddParameter = null;
            var targetParameters = targetElement.ParametersMap.Cast<Parameter>().Where(x =>
            {
                if (bic == BuiltInCategory.OST_Walls)
                {
                    if (isSource_UnconnectedHeight_ReadOnly)
                    {
                        if ((BuiltInParameter)x.Id.IntegerValue == BuiltInParameter.WALL_USER_HEIGHT_PARAM)
                        {
                            return false;
                        }
                        if ((BuiltInParameter)x.Id.IntegerValue == BuiltInParameter.WALL_TOP_OFFSET)
                        {
                            needAddParameter = x;
                        }
                    }
                    else
                    {
                        if ((BuiltInParameter)x.Id.IntegerValue == BuiltInParameter.WALL_USER_HEIGHT_PARAM)
                        {
                            needAddParameter = x;
                        }
                        if ((BuiltInParameter)x.Id.IntegerValue == BuiltInParameter.WALL_TOP_OFFSET)
                        {
                            return false;
                        }
                    }
                }
                return !x.IsReadOnly;
            }).ToList();

            if (needAddParameter != null)
            {
                targetParameters.Add(needAddParameter);
            }

            foreach (var parameter in targetParameters)
            {
                var sourceParamater = sourceParameters.First(x => x.Id == parameter.Id);
                switch (sourceParamater.StorageType)
                {
                    case StorageType.Integer:
                        parameter.Set(sourceParamater.AsInteger());
                        break;
                    case StorageType.Double:
                        parameter.Set(sourceParamater.AsDouble());
                        break;
                    case StorageType.String:
                        parameter.Set(sourceParamater.AsString());
                        break;
                    case StorageType.ElementId:
                        parameter.Set(sourceParamater.AsElementId());
                        break;
                }
            }
        }
    }
}
