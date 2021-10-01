using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace locationserver
{
    public class serverinfo
    {
        private static Dictionary<string, string> newinfo = new Dictionary<string, string>();

        public serverinfo()
        {
            newinfo = new Dictionary<string, string>();
        }

        public static void setlocation(string clientdetails, string location)
        {
            try
            {
                newinfo[clientdetails] = location;
            }
            catch
            {
                newinfo.Add(clientdetails, location);
            }
        }
        public static void update(string clientdetails, string location)
        {
            newinfo[clientdetails] = location;

        }

        public static string getlocation(string clientdetails)
        {
            if(newinfo.ContainsKey(clientdetails))
            {
                return newinfo[clientdetails];
            }
            else
            {
                return "ERROR: no entries found";
            }
        }

        private void savedictionary(Dictionary<string,string> keywords)
        {
            StreamWriter sw = new StreamWriter("../../12.tx");
            foreach(KeyValuePair<string,string> i in keywords)
            {
                sw.WriteLine(i.Key + ">" + i.Value);
                sw.Flush();
                sw.Close();
            }
        } 
        public static Dictionary<string, string> Accessdictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            FileStream fileStream = new FileStream("../../12.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fileStream);

            while(!sr.EndOfStream)
            {
                string hello = sr.ReadLine();
                string[] any = hello.Split('>');
                dictionary.Add(any[0], any[1]);
            }
            sr.Close();
            return dictionary;
        }
    }
}
