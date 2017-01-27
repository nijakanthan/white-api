using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
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
       /* static void Main(string[] args)
        {
            ElementHandler elementHandler = new ElementHandler();

            AutomationController test = new AutomationController();
            Window window = elementHandler.getWindow("Type=Win32Window|Name=Eclipse SDK");
            TestStack.White.UIItems.ListView datagrid =   window.Get<TestStack.White.UIItems.ListView>(SearchCriteria.ByXPath("",window));
            var x = datagrid.Rows[1];
          
            window.Focus();
            window.Click();
          //  IUIItem element1 = elementHandler.getIUIElement(window, "//data-grid[@Name='Row Down']");
            TestStack.White.UIItems.ListView element = (TestStack.White.UIItems.ListView)elementHandler.getIUIElement(window, "//data-grid[@Name='The selected validators will run when validation is performed:']");


            //ListView dataGrid = this.Get<ListView>(SearchCriteria.ByAutomationId("gridControl"));
            var row = element.Rows[1];
           // row.Text;
            row.Select();
       //     IUIItem element = null;
        //    bool a = true;
        //    while (a)
         //   {
        //        element = elementHandler.getIUIElement(window, "//List-Item[@Name='Account']");     
       //     if (element.IsOffScreen)
      //      {
               // element.ScrollBars.Vertical.ScrollDown();
        //        IUIItem element1= elementHandler.getIUIElement(window, "//Button[@Name='Row Down']");
        //        element1.DrawHighlight();
        //        element1.Focus();
        //        element1.Click();
               
           //     a = true;
           
            //}
          //  else
          //  {
           //     a = false;
          //  }
       //  }
            element.DrawHighlight();
            element.Focus();
            element.Click();
        }*/
    }
}
