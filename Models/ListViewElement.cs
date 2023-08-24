using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RvtSessionRecoverer.Models
{
    public class ListViewElement : INotifyPropertyChanged
    {
        bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                OnPropertyChanged();
            }
        }

        public string ViewName { get; set; }
        public string ViewType { get; set; }
        public int ViewId { get; set; }

        public ListViewElement(string viewName, string viewType, int viewId)
        {
            Selected = true; //always true upon initialization

            ViewName = viewName;
            ViewType = viewType;
            ViewId = viewId;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
