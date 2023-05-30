using Autodesk.Revit.DB;
using SingleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    public static class ParameterLookupUtil
    {
        private static RevitData revitData => RevitData.Instance;

        public static Parameter? Find(this ParameterLookup q, string name)
        {
            var element = q.Element!;
            var parameter = element.LookupParameter(name);

            if (parameter == null)
            {
                var config = q.Config;
                if (config.IsCreateNewShareParameterIfNotExists)
                {
                    var shareParameterFactory = config.OnCreatingShareParameterFactory();
                    shareParameterFactory.Name = name;
                    config.OnCreatedShareParameterFactory(shareParameterFactory);

                    shareParameterFactory.Do();

                    parameter = element.LookupParameter(name);
                }
            }

            return parameter;
        }
    }
}
