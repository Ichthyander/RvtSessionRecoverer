using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RvtSessionRecoverer.ViewModels
{
    class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private UIDocument _uiDocument;

        public string OutputString { get; set; }

        public DelegateCommand SaveSessionCommand { get; }
        public DelegateCommand LoadSessionCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _uiDocument = _commandData.Application.ActiveUIDocument;

            SaveSessionCommand = new DelegateCommand(OnSaveSessionCommand);
            LoadSessionCommand = new DelegateCommand(OnLoadSessionCommand);
        }

        private void OnLoadSessionCommand()
        {
            List<UIView> SessionSheets = Models.ViewUtils.GetSessionSheets(_commandData, _uiDocument);
            StringBuilder output = new StringBuilder();

            foreach (View sheet in SessionSheets) {
                output.Append(sheet.Name);
                output.Append("/r");
                }

            OutputString = output.ToString();

            RaiseCloseRequest();
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
    }
}
