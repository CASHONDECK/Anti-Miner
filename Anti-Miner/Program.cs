using System;
using System.IO;
using System.Management;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Anti_Miner
{
    class Program {

        static string NetStat()
        {
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

            return netstat_out;
        }

        static string TaskList()
        {
            Process p = new Process();

            ProcessStartInfo ps = new ProcessStartInfo();
            ps.Arguments = "";
            ps.FileName = "tasklist.exe";
            ps.UseShellExecute = false;
            ps.WindowStyle = ProcessWindowStyle.Hidden;
            ps.RedirectStandardInput = true;
            ps.RedirectStandardOutput = true;
            ps.RedirectStandardError = false;

            p.StartInfo = ps;
            p.Start();

            StreamReader output = p.StandardOutput;
            string tasklist_out = output.ReadToEnd();

            return tasklist_out;
        }

        static string Miner_AGR(string PID) {
            string run_agr = null;
            using (var searcher = new ManagementObjectSearcher("SELECT CommandLine FROM Win32_Process WHERE ProcessId = " + PID)) {
                var matchEnum = searcher.Get().GetEnumerator();
                if (matchEnum.MoveNext()) {
                    run_agr = matchEnum.Current["CommandLine"]?.ToString();
                }
            }
            return run_agr;
        }

        static void KillMiner(int pids_int, string[] PIDS)
        {
            for (int c = 0; c != pids_int; c++)
            {
                Process p2 = new Process();

                ProcessStartInfo ps2 = new ProcessStartInfo();
                ps2.Arguments = " /PID " + PIDS[c];
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
        }

        static void FindUnSafeAgr()
        {
            string[] PIDS = new string[200];
            int pids_int = 0;

            string[] line = Regex.Split(TaskList(), "\r\n");
            string[] agrs = { "pool", "xmr", "monero", "eth", "minergate", "nicehash", "mine", "mining", "money"};

            for (int i = 4; i != line.Length - 1; i++)
            {
                string[] agr = Regex.Split(line[i], "\\s+");

                string pid_agr = Miner_AGR(agr[1]);

                for (int a = 0; a != agrs.Length; a++)
                {
                    try { 
                        if (pid_agr.Contains(agrs[a])) {
                            PIDS[pids_int] = agr[1];
                            pids_int++;
                            break;
                        }
                    } catch { }
                }
            }

            KillMiner(pids_int, PIDS);
        }

        static void FindUnSafePort()
        {
            string[] PIDS = new string[200];
            int pids_int = 0;

            string[] line = Regex.Split(NetStat(), "\r\n");
            string[] ports = { "3333", "4444", "5555", "6666", "7777", "8888", "9999" };

            for (int i = 4; i != line.Length - 2; i++)
            {
                string[] port = Regex.Split(line[i], "\\s+");

                for (int a = 0; a != ports.Length; a++)
                {
                    if (port[3].Contains(ports[a]))
                    {
                        PIDS[pids_int] = port[5];
                        pids_int++;
                    }
                }
            }

            KillMiner(pids_int, PIDS);
        }

        static void Main(string[] args)
        {
            FindUnSafePort();
            Console.WriteLine("Search in unsafe port - completed!");
            FindUnSafeAgr();
            Console.WriteLine("Search in unsafe agr - completed!");
            Console.ReadLine();
        }
    }
}
