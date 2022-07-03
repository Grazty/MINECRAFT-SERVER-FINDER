using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetTools;
using System.IO;
using System.Drawing;
using System.Net;
using Console = Colorful.Console;
using Colorful;
using System.Text.RegularExpressions;
using MCQuery;
using System.Threading;
using MCServerStatus;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Minecraft_Server_Finder_Solution
{
    class Program
    {

        public class Rootobject
        {
            public string ip { get; set; }
            public int port { get; set; }
            public string version { get; set; }
            public bool online { get; set; }
            public int protocol { get; set; }
            public string hostname { get; set; }
        }

        public class Rootobject2
        {

            public string country { get; set; }
            public string region { get; set; }
            public string regionName { get; set; }
            public string city { get; set; }
            public string isp { get; set; }
        }

        public int totalOnline = 0;
        public int totalOffline = 0;
        public int totalErrors = 0;
        public int port = 25565;





        Formatter[] status = new Formatter[]
            {
                new Formatter("Online", Color.Green),
                new Formatter("Offline", Color.Red),
                new Formatter("404 Error", Color.Orange),
                new Formatter("Players", Color.Teal),
                new Formatter("Version", Color.Tan)
            };

        static void Main()
        {
            Program pro = new Program();
            string key = "";
            while (true)
            {
                Console.Clear();
                Console.Title = "Minecraft Greif Multi Tool By Grazty SB";
                Console.WriteLine("*****************************************************************************************", Color.Red);
                Console.WriteAscii("Minecraft Greif", Color.LawnGreen);
                Console.WriteLine("Developed By Grazty", Color.BlanchedAlmond);
                Console.WriteLine("*****************************************************************************************", Color.Red);
                Console.WriteLine("[1] Server Finder", Color.DodgerBlue);
                Console.WriteLine("[2] Real IP Finder", Color.DeepPink);
                Console.WriteLine("*****************************************************************************************", Color.Red);
                Console.Write("Please Enter 1 or 2: ");
                key = Console.ReadLine().Replace("Please Enter 1 or 2: ","");
                if(key == "1")
                {
                    Console.Clear();
                    pro.ServerEnterFeild();
                    break;
                }
                else if(key == "2")
                {
                    pro.ipFinder();
                    break;
                }
                else
                {
                    Console.WriteLine("PLEASE ENTER ONLY 1 OR 2", Color.OrangeRed);
                }
            }
        }

        public void ServerEnterFeild()
        {
            Program program = new Program();
            string startIP = "";
            string endIp = "";
            List<string> ips = new List<string>();



            Console.Title = "Minecraft Stalker| Online: " + program.totalOnline + " | Offline: " + program.totalOffline + " | Errors: " + program.totalErrors;

            Console.WriteLine("*****************************************************************************************", Color.Red);
            Console.WriteAscii("Minecraft Stalker", Color.LawnGreen);
            Console.WriteLine("Developed By Grazty", Color.BlanchedAlmond);
            Console.WriteLine("*****************************************************************************************", Color.Red);
            Console.Write("Please enter a starting IP Address *Example: 192.0.0.0*: ", Color.White);
            startIP = Console.ReadLine().Replace("Please enter a starting IP Address *Example: 192.0.0.0*: ", "");
            Console.Write("Please enter a ending IP Address *Example: 192.0.0.10*: ", Color.White);
            endIp = Console.ReadLine().Replace("Please enter a starting IP Address *Example: 192.0.0.10*: ", "");


            try
            {
                var ipRange = IPAddressRange.Parse(startIP + "-" + endIp);
                foreach (var item in ipRange)
                {
                    ips.Add(item.ToString());
                }

                program.GetServersV2(ips);
                //program.GetServerStatus(ips);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press Enter to Goto Main Menu");
                Console.ReadLine();
                Main();
            }
            
        }

        public void ipfinderd()
        {
            string realIP = "";
            Console.Write("Please Enter IP/Domain: ");
            string hostName = Console.ReadLine().Replace("Please Enter IP/Domain: ", "");
            WebClient wc = new WebClient();
            string jsonRes = wc.DownloadString("https://api.mcsrvstat.us/2/" + hostName);
            Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(jsonRes);


            Console.Write("Real IP: ", Color.White);
            realIP = obj.ip;
            Console.Write(obj.ip.ToString(), Color.Green);
            if (realIP == "127.0.0.1")
            {
                Console.WriteLine();
                Console.WriteLine("ERROR NOT A MINECRAFT SERVER OR ERROR IN HOSTNAME", Color.Red);
                Console.ForegroundColor = Color.White;
            }
            Console.WriteLine();

            string geoIPjson = wc.DownloadString("http://ip-api.com/json/" + realIP);
            Rootobject2 obj2 = JsonConvert.DeserializeObject<Rootobject2>(geoIPjson);
            Console.Write("ISP/HostName: ");
            Console.Write(obj2.isp, Color.LightYellow);
            Console.WriteLine();
            Console.Write("Location: ");
            Console.Write(obj2.regionName + ", " + obj2.country + ", " + obj2.city, Color.Cyan);
            Console.WriteLine("\nPlease Pess Enter to Continue");
            Console.WriteLine();
            Console.ReadLine();
            ipFinder();
        }

        public void ipFinder()
        {
            while (true)
            {
                Program pro = new Program();
                Console.Clear();
                Console.Title = "MineCraft Real IP Finder";
                Console.ForegroundColor = Color.White;
                Console.WriteLine("*****************************************************************************************", Color.Red);
                Console.WriteAscii("IP FINDER", Color.MediumVioletRed);
                Console.WriteLine("Developed By Grazty", Color.BlanchedAlmond);
                Console.WriteLine("*****************************************************************************************", Color.Red);
                Console.WriteLine("[1] Search IP", Color.GreenYellow);
                Console.WriteLine("[2] Exit back to main menu", Color.BlueViolet);
                Console.WriteLine("*****************************************************************************************", Color.Red);
                Console.Write("Please Enter 1 or 2: ");
                string key = Console.ReadLine().Replace("Please Enter 1 or 2: ", "");
                if (key == "1")
                {
                    pro.ipfinderd();
                }
                else if (key == "2")
                {
                    Main();
                }
                else
                {
                    Console.WriteLine("PLEASE ENTER ONLY 1 OR 2", Color.OrangeRed);
                }
            }
        }


        public static string GetUniqueFilePath(string filePath)
        {
            if (File.Exists(filePath))
            {
                string folderPath = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string fileExtension = Path.GetExtension(filePath);
                int number = 1;

                Match regex = Regex.Match(fileName, @"^(.+) \((\d+)\)$");

                if (regex.Success)
                {
                    fileName = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }

                do
                {
                    number++;
                    string newFileName = $"{fileName} ({number}){fileExtension}";
                    filePath = Path.Combine(folderPath, newFileName);
                }
                while (File.Exists(filePath));
            }

            return filePath;
        }

        

        public async void GetServersV2(List<string> ls)
        {
            string key;


            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Output\\onlineServers.txt";
            StreamWriter sw = File.CreateText(GetUniqueFilePath(path));
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < ls.Count; i++)
            {
                IMinecraftPinger pinger = new MinecraftPinger(ls[i], (ushort)port);
                
                try
                {
                    var statuss = await pinger.PingAsync();
                    Console.WriteLineFormatted($"Server: " + ls[i] + " is {0} | {3} online is " + statuss.Players.Online + "/" + statuss.Players.Max + " | " + "{4} is: " + statuss.Version.Name, Color.White, status);
                    totalOnline++;
                    sw.WriteLine(ls[i] + " | Players online is " + statuss.Players.Online + "/" + statuss.Players.Max + " | " + "Version is: " + statuss.Version.Name );
                    sw.Flush();
                }
                catch(InvalidOperationException)
                {
                    Console.WriteLineFormatted("Server: " + ls[i] + " is {1}", Color.White, status);
                    totalOffline++;
                }
                catch
                {
                    Console.WriteLineFormatted("While Checking Server " + ls[i] + " A {2} Occured Sorry!", Color.White, status);
                    totalErrors++;
                }
                Console.Title = "Minecraft Stalker| Online: " + totalOnline + " | Offline: " + totalOffline + " | Errors: " + totalErrors;

            }
            watch.Stop();
            Console.WriteLine("Done Checking Servers! It took " + watch.ElapsedMilliseconds + " miliseconds", Color.Magenta);
            Console.WriteLine("Total Online Servers: [" + totalOnline + "]" + " Total Offline Servers: [" + totalOffline + "]" + " Total Error Servers: [" + totalErrors + "]", Color.White);
            Console.WriteLine("The Online Servers have been Exported to a txt file in the same directory named servers (#).txt", Color.Yellow);
            while (true)
            {
                Console.WriteLine("Would you like to Search for Servers again? Y/N: ");
                key = Console.ReadLine().Replace("Woudl you like to Search for Servers again? Y/N: ", "");
                if (key == "y" || key == "Y")
                {
                    Console.Clear();
                    totalOnline = 0;
                    totalOffline = 0;
                    totalErrors = 0;
                    Console.Title = "Minecraft Stalker| Online: " + totalOnline + " | Offline: " + totalOffline + " | Errors: " + totalErrors;
                    sw.Close();
                    Main();
                }
                else if (key == "n" || key == "N")
                {
                    sw.Close();
                    Main();
                }
                else
                {
                    Console.WriteLine("Please neter only y or n");
                }
            }

        }


        public void GetServerStatus(List<string> ts)
        {
            string key;
            

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\servers.txt";
           
            string webdata = "";
            WebClient wc = new WebClient();
            wc.Proxy = null;
            StreamWriter sw = File.CreateText(GetUniqueFilePath(path));
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < ts.Count; i++)
            {
                
                try
                {
                    webdata = wc.DownloadString("https://api.mcsrvstat.us/simple/" + ts[i]);
                    if (webdata.Contains("Online"))
                    {
                        MCServer server = new MCServer(ts[i], port);
                        ServerStatus status1 = server.Status();
                        Console.WriteLineFormatted($"Server: " + ts[i] + " is {0} | {3} online is " + status1.Players.Online + "/" + status1.Players.Max + " | " + "{4} is: " + status1.Version.Name, Color.White, status);
                        totalOnline++;
                        sw.WriteLine(ts[i]);
                        sw.Flush();
                    }                  
                }
                catch (WebException)
                {
                    Console.WriteLineFormatted("Server: " + ts[i] + " is {1}", Color.White, status);
                    totalOffline++;
                }
                catch
                {
                    Console.WriteLineFormatted("While Checking Server " + ts[i] + "A {2} Occured Sorry!" , Color.White, status);
                    totalErrors++;
                }
                Console.Title = "Minecraft Stalker| Online: " + totalOnline + " | Offline: " + totalOffline + " | Errors: " + totalErrors;
            }
            watch.Stop();
            Console.WriteLine("Done Checking Servers! It took " + watch.ElapsedMilliseconds + " miliseconds", Color.Magenta);
            Console.WriteLine("Total Online Servers: [" + totalOnline + "]" + " Total Offline Servers: [" + totalOffline + "]" + " Total Error Servers: [" + totalErrors + "]", Color.White);
            Console.WriteLine("The Online Servers have been Exported to a txt file in the same directory named servers (#).txt", Color.Yellow);

            while(true)
            {
                Console.WriteLine("Woudl you like to Search for Servers again? Y/N: ");
                key = Console.ReadLine().Replace("Woudl you like to Search for Servers again? Y/N: ", "");
                if (key == "y" || key == "Y")
                {
                    Console.Clear();
                    totalOnline = 0;
                    totalOffline = 0;
                    totalErrors = 0;
                    Console.Title = "Minecraft Stalker| Online: " + totalOnline + " | Offline: " + totalOffline + " | Errors: " + totalErrors;
                    sw.Close();
                    Main();
                }
                else if (key == "n" || key == "N")
                {
                    sw.Close();
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Please neter only y or n");
                }
            }

        }
    }
}
