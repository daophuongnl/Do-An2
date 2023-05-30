using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    public class Base
    {
        protected virtual dynamic? Dict_Storage { get; set; }
        public dynamic Dict
        {
            get => this.Dict_Storage!;
            set => this.Dict_Storage = value;
        }
    }
}
