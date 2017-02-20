using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace CombatForms
{
    static class DataManager<T> where T : new()
    {
        public static void Serialize(string fileName, T data)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            Directory.CreateDirectory(Environment.CurrentDirectory + "../Saves/");
            TextWriter writer = new StreamWriter(Environment.CurrentDirectory + "../Saves/" + fileName + ".xml");

            serializer.Serialize(writer, data);

            writer.Close();
        }

        public static T Deserialize(string fileName)
        {
            T data;

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            TextReader reader = new StreamReader(Environment.CurrentDirectory + "../Saves/" + fileName + ".xml");

            data = (T)serializer.Deserialize(reader);

            reader.Close();

            return data;
        }
    }
}
