using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace RvtSessionRecoverer.Models
{
    class ViewUtils
    {
        public static List<int> GetSessionViews(ExternalCommandData commandData, UIDocument uiDocument)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            //Creating list of opened UIViews
            List<UIView> sessionUIViews = new List<UIView>();
            sessionUIViews = uiDocument.GetOpenUIViews() as List<UIView>;

            //Creating list of View IDs
            List<int> sessionViewIds = new List<int>();
            foreach (UIView sessionUIView in sessionUIViews)
            {
                sessionViewIds.Add(sessionUIView.ViewId.IntegerValue);
            }

            return sessionViewIds;
        }
    }
}
