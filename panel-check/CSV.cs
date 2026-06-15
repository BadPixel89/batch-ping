using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace panel_check
{
    internal class CSV
    {
        /// <summary>
        /// read the file into a table comprised of lists
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>A 2D list of string</returns>
        public List<List<string>> ParseTable(string filePath)
        {
            List<List<string>> csvtable = new List<List<string>>();
            List<string> currentLine = new List<string>();
            string line = "";

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        currentLine = line.Split(',').ToList();
                        csvtable.Add(currentLine);
                    }
                }
                return csvtable;
            }
            catch (Exception)
            {
                return null;
            }


        }
        /// <summary>
        /// write the list table to the file path specified
        /// </summary>
        /// <param name="csvTable"></param>
        /// <param name="filePath"></param>
        public void Write(List<List<string>> csvTable, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (List<string> line in csvTable)
                {
                    writer.WriteLine(String.Join(".",line));
                }
            }
            return;
        }
    }
}
