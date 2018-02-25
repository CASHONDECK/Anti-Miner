using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Anti_Miner
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] PIDS = new string[100];
            int pids_int = 0;

            Process p = new Process();

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

                if (port[3].Contains("3333") && port[3].Contains("7777") && port[3].Contains("4444") && port[3].Contains("5555") && port[3].Contains("6666") && port[3].Contains("45550") && port[3].Contains("45560") && port[3].Contains("45570") && port[3].Contains("45590") && port[3].Contains("45790")) {
                    PIDS[pids_int] = port[5];
                    pids_int++;
                }
            }

            for (int c = 0; c != pids_int; c++) {
                Process p2 = new Process();

                ProcessStartInfo ps2 = new ProcessStartInfo();
                ps2.Arguments = " /PID "+ PIDS[c];
                ps2.FileName = "taskkill.exe";
                ps2.UseShellExecute = false;
                ps2.WindowStyle = ProcessWindowStyle.Hidden;
                ps2.RedirectStandardInput = true;
                ps2.RedirectStandardOutput = true;
                ps2.RedirectStandardError = false;

                p2.StartInfo = ps2;
                p2.Start();

                Console.WriteLine("Founded and killed.");
            }
            Console.ReadLine();
        }
    }
}
