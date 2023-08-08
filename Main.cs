using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvtSessionRecoverer
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var mainWindow = new MainWindow(commandData);
            mainWindow.ShowDialog();
            return Result.Succeeded;
        }
    }
}
