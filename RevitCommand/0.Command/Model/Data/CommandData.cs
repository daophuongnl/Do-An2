using Model.Entity;
using Model.Form;
using SingleData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class CommandData
    {
        private static CommandData? instance;
        public static CommandData Instance
        {
            get => instance ??= new CommandData();
            set => instance = value;
        }

        //
        private static IOData ioData => IOData.Instance;

        // form
        //private Form.Form? form;
        //public Form.Form Form => this.form ??= new Form.Form { DataContext = this };

        // config
        public string ConfigFolder { get; set; } =
#if DEBUG
            @"[DIRECTORY_NAME]\output\Resource";
#elif RELEASE
            Path.Combine(ioData.AssemblyDirectoryPath, "Resource");
#endif

        public string ConfigPath => Path.Combine(this.ConfigFolder, "config.appsetting");

        private CommandConfig? config;
        public CommandConfig Config => this.config ??= ConfigUtil.Get<CommandConfig>(this.ConfigPath);
    }
}
