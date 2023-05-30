using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity.ParameterLookupNS
{
    public class Config
    {
        public bool IsCreateNewShareParameterIfNotExists { get; set; } = false;

        private ShareParameterFactoryNS.Config? shareParameterFactoryConfig;
        public ShareParameterFactoryNS.Config ShareParameterFactoryConfig
        {
            get => this.shareParameterFactoryConfig ??= new ShareParameterFactoryNS.Config();
            set => this.shareParameterFactoryConfig = value;
        }

        private Func<ShareParameterFactory>? onCreatingShareParameterFactory;
        public Func<ShareParameterFactory> OnCreatingShareParameterFactory
            => this.onCreatingShareParameterFactory ??= this.GetOnCreatingShareParameterFactory();

        private Action<ShareParameterFactory>? onCreatedShareParameterFactory;
        public Action<ShareParameterFactory> OnCreatedShareParameterFactory
            => this.onCreatedShareParameterFactory ??= this.GetOnCreatedShareParameterFactory();
    }
}
