using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace panel_check
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Ping _ping = new Ping();
            PingOptions _options = new PingOptions();
            PingReply _pingReply;
            byte[] _buffer = Encoding.ASCII.GetBytes("aaaaaaaapalmlabchecksaaaaaaaaaaa");
            int _timeout = 120;
            List<string> _offlineRooms = new List<string>();

            CSV _csv = new CSV();
            string infile = AppDomain.CurrentDomain.BaseDirectory + "panels.csv";
            string outfile = AppDomain.CurrentDomain.BaseDirectory + "panels-status.csv";

            
            List<string> csvData = _csv.ParseLines(infile);
            if (csvData == null || csvData.Count == 0)
            {
                Console.WriteLine("[error] : panels.csv not found or contains no data. filepath checked : " + infile);
                Console.WriteLine("press enter key to quit");
                Console.Read();
                return;
            }
            int ipindex = GetIPIndex(csvData[0]);
            //add headers to output data
            _offlineRooms.Add(csvData[0]);
            
            foreach (string line in csvData.Skip(1))
            {
                if (line == "")
                {
                    continue;
                }
                try
                {
                    _pingReply = _ping.Send(line.Split(',')[ipindex], _timeout, _buffer, _options);
                    if (_pingReply.Status == IPStatus.Success)
                    {
                        continue;
                    }
                    _offlineRooms.Add(line);
                    Console.WriteLine(line);
                }
                catch (Exception) {
                    _offlineRooms.Add(line);
                    Console.WriteLine(line);
                }
                
            }
            _csv.Write(_offlineRooms, outfile);
            Console.WriteLine("[done] output file : "+outfile + "\r\npress enter key to quit");
            Console.ReadLine();
        }

        private static string IncrementStatus(string statusbar)
        {
            if (statusbar.Length == 21)
            {
                return "[checking]           ";
            }
            else
            {
                return statusbar + ".";
            }
        }

        private static int GetIPIndex(string headers)
        {
            string[] columns = headers.Split(',');

            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i] == "IP")
                {
                    return i;
                }
            }
            throw new Exception("[error] IP field not found");
        }
    }
}
