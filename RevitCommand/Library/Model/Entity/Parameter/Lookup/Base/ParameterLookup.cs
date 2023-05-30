using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Entity.ParameterLookupNS;

namespace Model.Entity
{
    public class ParameterLookup
    {
        private Config? config;
        public Config Config
        {
            get => this.config ??= new Config();
            set => this.config = value;
        }

        public Element? Element { get; set; }


    }
}
