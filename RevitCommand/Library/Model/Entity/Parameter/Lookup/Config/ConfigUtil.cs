using Autodesk.Revit.DB;
using SingleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.ParameterLookupNS
{
    public static class ConfigUtil
    {
        private static RevitData revitData => RevitData.Instance;

        public static Func<ShareParameterFactory> GetOnCreatingShareParameterFactory(this Config q)
        {
            return () => new ShareParameterFactory
            {
                Config = q.ShareParameterFactoryConfig,
            };
        }

        //GetOnCreatedShareParameterFactory
        public static Action<ShareParameterFactory> GetOnCreatedShareParameterFactory(this Config q)
        {
            return (factory) => { };
        }
    }
}
