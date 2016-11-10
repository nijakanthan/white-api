using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace white_api
{
    class Program
    {
        [DllImport("kernel32.dll", ExactSpelling = true)]

        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int HIDE = 0;
        private const int MAXIMIZE = 3;
        private const int MINIMIZE = 6;
        private const int RESTORE = 9;


        static void Main(string[] args)
        {
            //args = new string[2];
            //args[0] = "localhost";
            //args[1] = "7121";

            if (args.Length != 0)


            {
                Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
                ShowWindow(ThisConsole, MAXIMIZE);

                //Base URL
                String baseAddress = "http://" + args[0] + ":" + args[1];
                WebApp.Start<Startup>(url: baseAddress);

                Console.WriteLine();
                Console.WriteLine("------------------------------------   WHITE SEVER Version 1.2.1   ------------------------------------");
                Console.WriteLine();
                Console.WriteLine("----------------  Server running at {0} - press Ctrl + C to quit. ", baseAddress);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   WHITE SEVER Version 1.2.1   ------------------------------------");
                //Console.ReadLine();
                new System.Threading.ManualResetEvent(false).WaitOne();
            }
        }
    }
}
