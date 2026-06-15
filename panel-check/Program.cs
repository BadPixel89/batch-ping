using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace panel_check
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // set up the ping stuff - timeout 120 is to speed up overall time when checking large number of devices
            Ping _ping = new Ping();
            PingOptions _options = new PingOptions();
            PingReply _pingReply;
            byte[] _buffer = Encoding.ASCII.GetBytes("aaaaaaaapanelcheckaaaaaaaaaaaaaa");
            int _timeout = 120;
            // store the results here
            List<List<string>> _offlineRooms = new List<List<string>>();
            //file paths for in/out files (maybe we can set with flags later - defaults to current exe location
            string infile = AppDomain.CurrentDomain.BaseDirectory + "panels.csv";
            string outfile = AppDomain.CurrentDomain.BaseDirectory + "panels-status.csv";
            //create a csv read/writer and pull in the data
            CSV _csv = new CSV();
            List<List<string>> csvData = _csv.ParseTable(infile);

            if (csvData == null || csvData.Count == 0)
            {
                Console.WriteLine("[error] : panels.csv not found or contains no data. filepath checked : " + infile);
                Console.WriteLine("press any key to quit");
                Console.ReadKey();
                return;
            }
            int ipindex = GetIPIndex(csvData[0]);
            //add headers as first row of output data
            _offlineRooms.Add(csvData[0]);
            //skip checking the first row as we assume it is headers
            foreach (List<string> line in csvData.Skip(1))
            {
                try
                {
                    _pingReply = _ping.Send(line[ipindex], _timeout, _buffer, _options);
                    if (_pingReply.Status == IPStatus.Success)
                    {
                        continue;
                    }
                    _offlineRooms.Add(line);
                    Console.WriteLine(String.Join("\t", line));
                }
                catch (Exception) {
                    _offlineRooms.Add(line);
                    Console.WriteLine(String.Join("\t",line));
                }
            }
            _csv.Write(_offlineRooms, outfile);
            Console.WriteLine("[done] output file : "+outfile + "\r\npress enter key to quit");
            Console.ReadLine();
        }

        /// <summary>
        /// first row of table should be the header, find the field with "IP" in it and return the index
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static int GetIPIndex(List<string> headers)
        {

            for (int i = 0; i < headers.Count; i++)
            {
                if (headers[i] == "IP")
                {
                    return i;
                }
            }
            throw new Exception("[error] IP field not found");
        }
    }
}
