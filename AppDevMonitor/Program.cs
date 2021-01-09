using System;
using System.Timers;
using System.Text.RegularExpressions;
using System.Net.Http;
using AppDevMonitor;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;
using System.Linq;

namespace CheckAppleStatus
{
    class Program
    {

        static Timer timer;
        private const string STATUS_FILE = @"c:\temp\AppleStatusClass.txt";
        static HttpClient client = new HttpClient();
        private static Regex rxAppleJson = new Regex(
              "jsonCallback\\((?<jsondata>[^\\)]*)\\)\\;",
            RegexOptions.IgnoreCase
            | RegexOptions.Multiline
            | RegexOptions.CultureInvariant
            | RegexOptions.Compiled
            );

        static void Main(string[] args)
        {
#if !DEBUG
            timer = new Timer
            {
                Interval = 1000 * 60 * 5 //set interval of checking here
            };
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);

            RunAsync().GetAwaiter().GetResult();

            start_timer();
            Console.Read();
#else
            RunAsync().GetAwaiter().GetResult();
            Console.ReadLine();
#endif
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            Console.Clear();
            Console.WriteLine("{0} Apple Dev Monitor", DateTime.Now.ToLongTimeString());
            var appleServiceData = await FetchAppleServiceDataAsync();
            if(appleServiceData.services.Any())
            {
                foreach(var service in appleServiceData.services)
                {
                    Console.Write(service.serviceName);
                    Console.ForegroundColor = service.events.Any() ? ConsoleColor.Red : ConsoleColor.Green;
                    if(service.events.Any())
                    {
                        foreach(var serviceEvent in service.events)
                        {
                            Console.ForegroundColor = serviceEvent.eventStatus.Equals("resolved") ? ConsoleColor.Green : ConsoleColor.Red;
                            Console.Write($" {service.serviceName}: ({serviceEvent.eventStatus}) - {serviceEvent.message}");
                        }
                    }
                    else
                    {
                        Console.Write (" Available");
                    }

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("");
                }
            }
            else
            {
                Console.WriteLine("No data from website");
            }
        }

        private static async Task<AppleServiceRoot> FetchAppleServiceDataAsync()
        {
            var appleServices = new AppleServiceRoot();
            HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["AppleServicesUrl"]);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string result = await response.Content.ReadAsStringAsync();
                    var match = rxAppleJson.Match(result);
                    if (match.Success)
                    {
                        var jsonData = match.Groups["jsondata"].Value;
                        appleServices = JsonConvert.DeserializeObject<AppleServiceRoot>(jsonData);
                    }
                }
                catch(Exception wtf)
                {
                    Console.WriteLine(wtf.Message);
                }
            }

            return appleServices;
        }
        private static void start_timer()
        {
            timer.Start();
            //Console.WriteLine("Start!");
        }
    }
}
