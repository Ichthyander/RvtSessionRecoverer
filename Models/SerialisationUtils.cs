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
        public static void SerializeSession(Session UserSession)
        {
            //ВРЕМЕННО!!! сохраняем файл на рабочем столе
            string dirPath = "C:/Users/Фуфелшмерц/Desktop/JSON";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string filePath = dirPath + "/Session.json";
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine(JsonConvert.SerializeObject(UserSession));
            }
        }

        public static Session DeserializeSession(string path = "C:/Users/Фуфелшмерц/Desktop/JSON/Session.json")
        {
            //ВРЕМЕННО!!! сохраняем файл на рабочем столе 
            string jsonString = String.Empty;

            using (StreamReader sr = new StreamReader(path))
            {
                jsonString = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<Session>(jsonString);
        }
    }
}
