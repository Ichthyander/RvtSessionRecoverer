using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;

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
