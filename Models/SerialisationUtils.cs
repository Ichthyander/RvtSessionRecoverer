using Autodesk.Revit.DB;
using Microsoft.Win32;
using Newtonsoft.Json;
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
    class SerialisationUtils
    {
        public static bool? SerializeSession(Session UserSession)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.DefaultExt = ".json";
            saveFileDialog.AddExtension = true;
            saveFileDialog.Filter = "json файлы (*.json)|*.json|Все файлы (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.Title = "Сохранение сессии";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.FileName = "Сессия_" + DateTime.Now.ToString("yyyy_MM_dd_H-mm");

            // Show save file dialog box
            bool? result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                // Save document
                string filename = saveFileDialog.FileName;

                using (StreamWriter sw = new StreamWriter(filename))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(UserSession));
                }
            }

            return result;
        }

        public static Session DeserializeSession(string path = "C:/Users/Фуфелшмерц/Desktop/JSON/Session.json")
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Multiselect = false;
            openFileDialog.DefaultExt = ".json";
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = "json файлы (*.json)|*.json";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Title = "Открытие сессии";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Show open file dialog box
            bool? result = openFileDialog.ShowDialog();

            string jsonString = String.Empty;
            if (result == true)
            {
                //Open document
                string filename = openFileDialog.FileName;

                using (StreamReader sr = new StreamReader(filename))
                {
                    jsonString = sr.ReadToEnd();
                }
            }

            return JsonConvert.DeserializeObject<Session>(jsonString);
        }
    }
}
