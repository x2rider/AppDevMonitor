using System;
using System.Text;

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
}
