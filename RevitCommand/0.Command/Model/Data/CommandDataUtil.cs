using Autodesk.Revit.DB;
using Model.Entity;
using SingleData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility;

namespace Model.Data
{
    public static class CommandDataUtil
    {
        private static RevitData revitData => RevitData.Instance;

        // init
        public static void Dispose(this CommandData q)
        {
            RevitDataUtil.Dispose();
            CommandData.Instance = null;
        }
    }
}
