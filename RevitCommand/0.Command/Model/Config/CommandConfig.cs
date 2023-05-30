using Model.Entity;
using Model.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class CommandConfig : Config
    {
        private string? name;
        public string? Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.Save();
            }
        }
    }
}
