using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreasureHunt
{
    public class View
    {
        public View() 
        { 
        }

        public List<string> Read(string fileName)
        {
            List<string> list = new List<string>();
            string[] allLines = File.ReadAllLines(fileName);
            foreach (string line in allLines)
            {
                if (!line.StartsWith("#"))
                    list.Add(line);
            }
            return list;
        }

        public void Write(List<string> content, string filePath)
        {
            File.WriteAllLines(filePath, content);
        }
    }
}
