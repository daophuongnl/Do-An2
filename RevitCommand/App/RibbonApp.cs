using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SingleData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Model.Application
{
    public class RibbonApp : IExternalApplication
    {
        private RibbonData ribbonData => RibbonData.Instance;

        public Result OnStartup(UIControlledApplication application)
        {
            ribbonData.Application = application;

            var tab = EntTabUtil.Get("BIMDev tools");
            var panel = tab.GetPanel("Panel");
            panel.GetPushButton("Command", "Model.RevitCommand.Command", "Resourse/Icon/command.icon");

            tab.CreateTab();

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
