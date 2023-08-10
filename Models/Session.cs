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
    public class Session
    {
        List<ElementId> viewIds = new List<ElementId>();
        public List<ElementId> ViewIds
        {
            get
            {
                return viewIds;
            }
            set
            {
                viewIds = value;
            }
        }

        public Session(List<ElementId> viewIds)
        {
            ViewIds = viewIds;
        }

        public void SerializeSession()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };

            //ВРЕМЕННО!!! сохраняем файл на рабочем столе
            string dirPath = "C:/Users/Фуфелшмерц/Desktop/JSON";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string filePath = dirPath + "/Session.json";
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(JsonSerializer.Serialize(ViewIds, options));
            }
        }

        public void DeserializeSession()
        {
            string jsonString = String.Empty;

            string path = "C:/Users/Фуфелшмерц/Desktop/JSON/Session.json";
            using (StreamReader sr = new StreamReader(path))
            {
                jsonString = sr.ReadToEnd();
            }

            ViewIds = JsonSerializer.Deserialize<List<ElementId>>(jsonString);
        }
    }
}
