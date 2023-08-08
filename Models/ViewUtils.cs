using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvtSessionRecoverer.Models
{
    class ViewUtils
    {
        public static List<UIView> GetSessionSheets(ExternalCommandData commandData, UIDocument uiDocument)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            //Выбор открытых видов
            //FilteredElementCollector viewCollector = new FilteredElementCollector(doc);
            //viewCollector.OfClass(typeof(View));

            //List<View> SessionSheets = new List<View>();
            //foreach (Element viewElement in viewCollector)
            //{
            //    View view = (View)viewElement;
            //    SessionSheets.Add(view);
            //}

            List<UIView> SessionSheets = new List<UIView>();
            SessionSheets = uiDocument.GetOpenUIViews() as List<UIView>;

            return SessionSheets;
        }
    }
}
