using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreSheet
{
    public class ThisAndThat
    {
        public static string Jsonize(Type t, object o)
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(t);
            ser.WriteObject(stream1, o);     //Verweis auf System.Xml

            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            string jsonString = sr.ReadToEnd();

            return jsonString;
        }

        public static string Jsonize<T>(T o)
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(stream1, o);     //Verweis auf System.Xml

            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            string jsonString = sr.ReadToEnd();

            return jsonString;
        }

        public static T DeJsonize<T>(string json)
        {
            MemoryStream stream1 = new MemoryStream(Encoding.UTF8.GetBytes(json));            
            stream1.Position = 0;
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            object o = ser.ReadObject(stream1);
            return (T)o;
        }

        public static string[] playersInitials(string[] players)
        {
            if (players == null)
                return null;
            if (players.Length == 0)
                return new string[] { };

            string[] initials = new string[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                string name;
                if (players[i] != null && (players[i]) != string.Empty)
                {
                    //initials
                    var s = players[i].Trim().Split(' ');
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in s)
                    {
                        sb.Append(item.Trim().Substring(0, 1)).Append(".");
                    }
                    name = sb.ToString();
                }
                else
                {
                    name = Convert.ToString(i);
                }

                //Must be unique.
                int n = 0;
                for (int j = 0; j < i; j++)
                {
                    if (initials[j].StartsWith(name))
                    {
                        n++;
                    }
                }
                if (n > 0)
                {
                    //add (n)
                    name = (new StringBuilder(name)).Append('(').Append(n).Append(')').ToString();
                }
                initials[i] = name;
            }
            return initials;
        }
    }
}
