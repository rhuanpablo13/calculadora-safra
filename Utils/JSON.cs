using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Converter
{
    public class JSON
    {

        public static DateTime toDateTime(String date) {
            if (date is null) {
                throw new ArgumentNullException(nameof(date));
            }
            return DateTime.Parse(date);
        }


        public static string toStringJson<T>(T obj) { 
            try {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, obj);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return jsonString;
            }
            catch {
                throw new Exception("Erro ao converter em Json o objeto: " + obj.GetType().FullName);
            }
        }


        public static T toObject<T>(string jsonString) {
            try {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T obj = (T)serializer.ReadObject(ms);
                return obj;
            }
            catch {
                throw new Exception("Erro ao converter em objeto o json: " + jsonString);
            }
        }

        
    }
}