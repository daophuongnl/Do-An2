using Autodesk.Revit.DB;
using SingleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.ShareParameterFactoryNS
{
    public static class ConfigUtil
    {
        private static RevitData revitData => RevitData.Instance;

        public static DefinitionFile GetDefinitionFile(this Config q)
        {
            var app = revitData.Application;

            return app.OpenSharedParameterFile();
        }

        public static string GetDefinitionGroupName(this Config q)
        {
            return "Group1";
        }

#if REVIT2022_OR_LESS
        public static ParameterType GetParameterType(this Config q)
        {
            return ParameterType.Number;
        }
#else
        public static ForgeTypeId GetForgeTypeId(this Config q)
        {
            return revitData.MeasurableSpecs.First(x => x.TypeId == "autodesk.spec.aec:number-2.0.0");
        }
#endif

        public static BuiltInParameterGroup GetParameterGroup(this Config q)
        {
            return BuiltInParameterGroup.PG_DATA;
        }
    }
}
