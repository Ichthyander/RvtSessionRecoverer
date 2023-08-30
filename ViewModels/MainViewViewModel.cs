using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using RvtSessionRecoverer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RvtSessionRecoverer.ViewModels
{
    class MainViewViewModel : INotifyPropertyChanged
    {
        //Everything needed from Revit
        private ExternalCommandData _commandData;
        private UIDocument uiDocument;
        private Document document;

        Session LoadedSession;
        Session CurrentSession;

        #region ListView Properties
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

        List<ListViewElement> _LoadListViewData;
        public List<ListViewElement> LoadListViewData
        {
            get
            {
                return _LoadListViewData;
            }
            set
            {
                _LoadListViewData = value;
                OnPropertyChanged();
            }
        }
        #endregion

        //Buttons actions
        public DelegateCommand SaveSessionCommand { get; }
        public DelegateCommand LoadSessionCommand { get; }
        public DelegateCommand RestoreSessionCommand { get; }

        //Constructor
        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            uiDocument = _commandData.Application.ActiveUIDocument;
            document = uiDocument.Document;

            //Delegating commands
            SaveSessionCommand = new DelegateCommand(OnSaveSessionCommand);
            LoadSessionCommand = new DelegateCommand(OnLoadSessionCommand);
            RestoreSessionCommand = new DelegateCommand(OnRestoreSessionCommand);

            LoadListViewData = new List<ListViewElement>();
            SaveListViewData = new List<ListViewElement>();

            //Initializing UserSession to display it in TextBlock from "Save" Tab 
            CurrentSession = new Session(ViewUtils.GetSessionViews(_commandData, uiDocument));
            List<View> Views = CurrentSession.GetViews(document);

            //Display opened views as ListView
            foreach (View view in Views)
            {
                _SaveListViewData.Add(new ListViewElement(view.Name, view.ViewType.ToString(), view.Id.IntegerValue));
            }
        }

        //Actions on "Restore" button
        private void OnRestoreSessionCommand()
        {
            if (_LoadListViewData != null && _LoadListViewData.Count != 0)
            {
                int counter = 0;

                //Creating List of selected Views
                List<int> LoadViewIds = new List<int>();
                foreach (ListViewElement listViewElement in _LoadListViewData)
                {
                    if (listViewElement.Selected)
                    {
                        uiDocument.ActiveView = document.GetElement(new ElementId(listViewElement.ViewId)) as View;
                        counter++;
                    }
                }

                RaiseCloseRequest();

                TaskDialog.Show("Готово!", "Восстановлено видов: " + counter); 
            }
        }

        //Actions on Load button
        private void OnLoadSessionCommand()
        {
            //Getting list of UIViews by using method from model
            LoadedSession = SerialisationUtils.DeserializeSession();

            if (LoadedSession != null)
            {
                _LoadListViewData.Clear();  //clear list of loaded views
                List<View> Views = LoadedSession.GetViews(document);

                List<ListViewElement> LoadList = new List<ListViewElement>();

                foreach (View view in Views)
                {
                    if (view != null)
                    {
                        LoadList.Add(new ListViewElement(view.Name, view.ViewType.ToString(), view.Id.IntegerValue));
                    }
                }

                LoadListViewData = LoadList;
            }
        }

        //Actions on Save button
        private void OnSaveSessionCommand()
        {
            //Creating List of selected Views
            List<int> SaveViewIds = new List<int>();
            foreach (ListViewElement listViewElement in _SaveListViewData)
            {
                if (listViewElement.Selected)
                {
                    SaveViewIds.Add(listViewElement.ViewId);
                }
            }

            bool? result = SerialisationUtils.SerializeSession(new Session(SaveViewIds));
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
