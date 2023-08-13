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
        string outputString;
        public string OutputString
        {
            get
            {
                return outputString;
            }
            set
            {
                outputString = value;
                OnPropertyChanged();
            }
        }

        //Buttons
        public DelegateCommand SaveSessionCommand { get; }
        public DelegateCommand LoadSessionCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            uiDocument = _commandData.Application.ActiveUIDocument;
            document = uiDocument.Document;

            //Delegating commands
            SaveSessionCommand = new DelegateCommand(OnSaveSessionCommand);
            LoadSessionCommand = new DelegateCommand(OnLoadSessionCommand);

            //Initializing UserSession for output in TextBlock from "Save" Tab 
            UserSession = new Session(ViewUtils.GetSessionViews(_commandData, uiDocument));
            
            //debug
            StringBuilder output = new StringBuilder();     //debug purposes

            List<View> Views = UserSession.GetViews(document);

            foreach (View view in Views)
            {
                output.Append(view.Name);
                output.Append("\r");
                uiDocument.ActiveView = view;
            }

            CurrentlyOpenedViews = output.ToString();
            //debug
        }

        //Loading list of previously opened views
        private void OnLoadSessionCommand()
        {
            //Getting list of UIViews by using method from model
            UserSession = SerialisationUtils.DeserializeSession();
            StringBuilder output = new StringBuilder();     //debug purposes

            List<View> Views = UserSession.GetViews(document);

            foreach (View view in Views)
            {
                output.Append(view.Name);
                output.Append("\r");
                uiDocument.ActiveView = view;
            }

            OutputString = output.ToString();

            //Will be used later
            //RaiseCloseRequest();
        }

        private void OnSaveSessionCommand()
        {
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


            SerialisationUtils.SerializeSession(UserSession);
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
