using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
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
        private UIDocument _uiDocument;

        //Text block content
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
            _uiDocument = _commandData.Application.ActiveUIDocument;

            //Delegating commands
            SaveSessionCommand = new DelegateCommand(OnSaveSessionCommand);
            LoadSessionCommand = new DelegateCommand(OnLoadSessionCommand);
        }

        //Loading list of previously opened views
        private void OnLoadSessionCommand()
        {
            //Getting list of UIViews by using method from model
            List<UIView> SessionSheets = Models.ViewUtils.GetSessionSheets(_commandData, _uiDocument);
            StringBuilder output = new StringBuilder();

            //IMPLEMENT try-catch for any non-view instances!!!
            List<View> Views = new List<View>();
            foreach (var sessionSheet in SessionSheets)
            {
                Views.Add(_uiDocument.Document.GetElement(sessionSheet.ViewId) as View);
            }

            foreach (View view in Views)
            {
                output.Append(view.Name);
                output.Append("\r");
            }

            OutputString = output.ToString();

            //Just some dubugging features
            //TaskDialog.Show("Список открытых видов", $"Открытые виды - {output}");
            
            //Will be used later
            //RaiseCloseRequest();
        }

        private void OnSaveSessionCommand()
        {
            throw new NotImplementedException();
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
