using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Anti_Miner
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] PIDS = new string[100];
            int pids_int = 0;
            using (Process p = new Process())
            {

                ProcessStartInfo ps = new ProcessStartInfo();
                ps.Arguments = "-a -n -o -p TCP";
                ps.FileName = "netstat.exe";
                ps.UseShellExecute = false;
                ps.WindowStyle = ProcessWindowStyle.Hidden;
                ps.RedirectStandardInput = true;
                ps.RedirectStandardOutput = true;
                ps.RedirectStandardError = false;

                p.StartInfo = ps;
                p.Start();

                StreamReader output = p.StandardOutput;

                string netstat_out = output.ReadToEnd();

                string[] line = Regex.Split(netstat_out, "\r\n");

                for (int i = 4; i != line.Length - 2; i++)
                {

                    string[] port = Regex.Split(line[i], "\\s+");

                    if (port[3].Contains("3333"))
                    {
                        PIDS[pids_int] = port[5];
                        pids_int++;
                    }
                }
            }

            Console.WriteLine(PIDS[0]);
            Console.ReadLine();
        }
    }
}
