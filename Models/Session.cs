using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace RvtSessionRecoverer.Models
{
    //Serializable class for saving and loading Revit sessions
    [Serializable]
    public class Session
    {
        List<int> viewIntIds = new List<int>();
        public List<int> ViewIntIds
        {
            get
            {
                return viewIntIds;
            }
            set
            {
                viewIntIds = value;
            }
        }

        //have to store integer values instead of ElementId 'cos of that FPoS Deserialization method
        public Session(List<int> viewIntIds)
        {
            ViewIntIds = viewIntIds;
        }

        //public List<ElementId> GetViewElementIds()
        //{
        //    List<ElementId> elementIds = new List<ElementId>();
        //    foreach (var viewIntId in viewIntIds)
        //    {
        //        elementIds.Add(new ElementId(viewIntId));
        //    }
        //    return elementIds;
        //}

        public List<View> GetViews(Document document)
        {
            List<View> views = new List<View>();
            foreach (var viewId in viewIntIds)
            {
                ElementId elementId = new ElementId(viewId);
                views.Add(document.GetElement(elementId) as View);
            }
            return views;
        }
    }
}
