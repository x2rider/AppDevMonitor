using System;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;
using System.Text.RegularExpressions;

namespace CheckAppleStatus
{
    class AppleStatus
    {
        public bool memberCenter = false;
        public bool iosDevCenter = false;
        public bool Certificates = false;
        public bool iTunesConnect = false;

        public string Print()
        {
            var response = new StringBuilder();

            if (memberCenter)
                response.Append("Member Center is online\n");
            else
                response.Append("Member Center is offline\n");

            if (iosDevCenter)
                response.Append("iOS Dev Center is online\n");
            else
                response.Append("iOS Dev Center is offline\n");

            if (Certificates)
                response.Append("Certificates are online\n");
            else
                response.Append("Certificates is offline\n");

            if (iTunesConnect)
                response.Append("iTunes Connect is online\n");
            else
                response.Append("iTunes Connect is offline\n");

            return response.ToString();
        }

        public bool IsSiteAllWorking()
        {
            return (memberCenter && iosDevCenter && Certificates && iTunesConnect);
        }

        public void PrintToConsole()
        {
            Console.Write("Member Center is ");
            if (memberCenter)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("online\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("offline\n");
            }
            Console.ResetColor();

            Console.Write("iOS Dev Center is ");
            if (iosDevCenter)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("online\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("offline\n");
            }
            Console.ResetColor();

            Console.Write("Certificates are ");
            if (Certificates)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("online\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("offline\n");
            }
            Console.ResetColor();

            Console.Write("iTunes Connect is ");
            if (iTunesConnect)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("online\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("offline\n");
            }
            Console.ResetColor();
        }
    }

    class Program
    {
        static Timer timer;
        private const string STATUS_FILE = @"c:\temp\AppleStatusClass.txt";

        static void Main(string[] args)
        {
#if !DEBUG
            timer = new Timer();
            timer.Interval = 1000 * 60 * 20; //set interval of checking here
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            CheckOnAppleDevCenter();

            start_timer();
            Console.Read();
#else
            CheckOnAppleDevCenter();
            Console.ReadLine();
#endif
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckOnAppleDevCenter();
        }

        static void CheckOnAppleDevCenter()
        {
            Console.Clear();
            Console.WriteLine("{0} Apple Dev Monitor", DateTime.Now.ToLongTimeString());
            string checkApple = FetchAppleData();
            File.WriteAllText(STATUS_FILE, checkApple);

            var newStatus = ParseApple(checkApple);
            Console.WriteLine("Status:");
            newStatus.PrintToConsole();

            var lastStatus = LoadStatus();

            var emailBody = new StringBuilder("");
            if (lastStatus.memberCenter == false)
            {
                if (newStatus.memberCenter)
                {
                    emailBody.Append("Member Center is online\n");
                }
            }

            if (lastStatus.iosDevCenter == false)
            {
                if (newStatus.iosDevCenter)
                {
                    emailBody.Append("iOS Dev Center is online\n");
                }
            }

            if (lastStatus.Certificates == false)
            {
                if (newStatus.Certificates)
                {
                    emailBody.Append("Certificates are online\n");
                }
            }

            if (emailBody.Length > 0)
            {
                // send email
            }

            if (newStatus.IsSiteAllWorking())
            {
                timer.Stop();
                //System.Environment.Exit(0);
            }
        }

        private static AppleStatus LoadStatus()
        {
            var status = new AppleStatus();

            var statusFile = STATUS_FILE;
            if (File.Exists(statusFile))
            {
                var currentStatusData = File.ReadLines(statusFile);

                foreach (string line in currentStatusData)
                {
                    status = ParseApple(line);
                    break;
                }
            }
            return status;
        }

        private static AppleStatus ParseApple(string data)
        {
            var status = new AppleStatus();

            Regex MyRegex = new Regex(
                  "<td\\b[^>]*>(.*?)</td>",
                RegexOptions.IgnoreCase
                | RegexOptions.CultureInvariant
                | RegexOptions.Compiled
                );

            // Capture all Matches in the InputText
            MatchCollection ms = MyRegex.Matches(data);

            //foreach (string line in currentStatusData)
            foreach (Match line in ms)
            {
                if (line.Value.Contains("online"))
                {
                    if (line.Value.Contains("Certificates"))
                        status.Certificates = true;
                    else if (line.Value.Contains("iOS Dev Center"))
                        status.iosDevCenter = true;
                    else if (line.Value.Contains("Member Center"))
                        status.memberCenter = true;
                    else if (line.Value.Contains("iTunes Connect"))
                        status.iTunesConnect = true;
                }
            }

            return status;
        }

        private static string FetchAppleData()
        {
            string sURL;
            sURL = "https://www.apple.com/support/systemstatus/";

            var response = new StringBuilder();

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            WebProxy myProxy = new WebProxy("myproxy", 80);
            myProxy.BypassProxyOnLocal = true;

            // If you're behind corporate walls and need to specify a proxy
            //wrGETURL.Proxy = new WebProxy("YOUR_PROXY", 80);

            try
            {
                Stream objStream;
                objStream = wrGETURL.GetResponse().GetResponseStream();

                StreamReader objReader = new StreamReader(objStream);

                string sLine = "";
                int i = 0;

                while (sLine != null)
                {
                    i++;
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                    {
                        response.Append(sLine);
                        //Console.WriteLine("{0}:{1}", i, sLine);
                    }
                }
            }
            catch (Exception wtf)
            {
                Console.WriteLine("An error occurred, will try again.\nDetails: {0}", wtf.Message);
            }

            return response.ToString();
        }

        private static void start_timer()
        {
            timer.Start();
            //Console.WriteLine("Start!");
        }
    }
}
