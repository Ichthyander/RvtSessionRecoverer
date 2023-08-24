using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using RvtSessionRecoverer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RvtSessionRecoverer.ViewModels
{
    class MainViewViewModel : INotifyPropertyChanged
    {
        //Everything needed from Revit
        private ExternalCommandData _commandData;
        private UIDocument uiDocument;
        private Document document;

        Session UserSession;

        List<ListViewElement> _SaveListViewData;
        public List<ListViewElement> SaveListViewData
        {
            get
            {
                return _SaveListViewData;
            }
            set
            {
                _SaveListViewData = value;
                OnPropertyChanged();
            }
        }

        //Text that represents opened views in current Revit session
        string currentlyOpenedViews;
        public string CurrentlyOpenedViews
        {
            get
            {
                return currentlyOpenedViews;
            }
            set
            {
                currentlyOpenedViews = value;
                OnPropertyChanged();
            }
        }

        //TextBlock content
        string loadedSession;
        public string LoadedSession
        {
            get
            {
                return loadedSession;
            }
            set
            {
                loadedSession = value;
                OnPropertyChanged();
            }
        }

        //Buttons
        public DelegateCommand SaveSessionCommand { get; }
        public DelegateCommand LoadSessionCommand { get; }
        public DelegateCommand RestoreSessionCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            uiDocument = _commandData.Application.ActiveUIDocument;
            document = uiDocument.Document;

            //Delegating commands
            SaveSessionCommand = new DelegateCommand(OnSaveSessionCommand);
            LoadSessionCommand = new DelegateCommand(OnLoadSessionCommand);
            RestoreSessionCommand = new DelegateCommand(OnRestoreSessionCommand);

            //Initializing UserSession to display it in TextBlock from "Save" Tab 
            UserSession = new Session(ViewUtils.GetSessionViews(_commandData, uiDocument));
            
            StringBuilder output = new StringBuilder();     //debug purposes

            List<View> Views = UserSession.GetViews(document);

            //Display opened views as string
            foreach (View view in Views)
            {
                output.Append(view.Name);
                output.Append("\r");
                //uiDocument.ActiveView = view;
            }

            CurrentlyOpenedViews = output.ToString();

            //Display opened views as ListView
            List<ListViewElement> ListViewData = new List<ListViewElement>();

            foreach (View view in Views)
            {
                ListViewData.Add(new ListViewElement(view.Name, view.ViewType.ToString(), view.Id.IntegerValue));
            }

            SaveListViewData = ListViewData;
        }

        //Actions on Restore button
        private void OnRestoreSessionCommand()
        {
            if (LoadedSession != null)
            {
                List<View> Views = UserSession.GetViews(document);
                int counter = 0;

                foreach (View view in Views)
                {
                    if (view != null)
                    {
                        uiDocument.ActiveView = view;
                        counter++;
                    }
                }

                RaiseCloseRequest();

                TaskDialog.Show("Готово!", "Восстановлено " + counter + " видов");
            }
        }

        //Actions on Load button
        private void OnLoadSessionCommand()
        {
            //Getting list of UIViews by using method from model
            UserSession = SerialisationUtils.DeserializeSession();

            if (UserSession != null)
            {
                StringBuilder output = new StringBuilder();     
                List<View> Views = UserSession.GetViews(document);

                foreach (View view in Views)
                {
                    if (view != null)
                    {
                        output.Append(view.Name);
                        output.Append("\r");
                    }
                }

                LoadedSession = output.ToString();
            }
        }

        //Actions on Save button
        private void OnSaveSessionCommand()
        {
            List<int> SaveViewIds = new List<int>();

            foreach (ListViewElement listViewElement in _SaveListViewData)
            {
                if (listViewElement.Selected)
                {
                    SaveViewIds.Add(listViewElement.ViewId);
                }
            }

            Session SavedSession = new Session(SaveViewIds);

            bool? result = SerialisationUtils.SerializeSession(SavedSession);
            if (result == true)
            {
                RaiseCloseRequest();
            }
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
