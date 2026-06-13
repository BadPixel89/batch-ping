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
        /// Read the specified CSV file
        /// </summary>
        /// <param name="filePath"></param><br></br>
        /// <returns>A list<string> of the lines in the csv doc</returns>
        public List<string> ParseLines(string filePath)
        {
            string line = "";
            List<string> lines = new List<string>();

            try
            {

                using (StreamReader reader = new StreamReader(filePath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        lines.Add(line);
                    }
                }

                return lines;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Write a csv file from a List<string> of CSV lines. If the lines are not comma separated they will still be written.
        /// </summary>
        /// <param name="csvLines"></param>
        /// <param name="filePath"></param>
        public void Write(List<string> csvLines, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (string line in csvLines)
                {
                    writer.WriteLine(line);
                }
            }
            return;
        }
        public void Write(string csvLines, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                writer.Write(csvLines);
            }
            return;
        }
    }
}
