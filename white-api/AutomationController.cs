using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Forms;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;
using white_api.Model;

namespace white_api
{
    [RoutePrefix("windowsAutomation")]
    public class AutomationController : System.Web.Http.ApiController
    {
        private static TestStack.White.Application application = null;
        private static Window mainWindow = null;
        private static ElementHandler elementHandler = new ElementHandler();
        //private static IUIItem globalUIElement = null;
        //private static Object globalUIElementProperty = null;
        private static IUIItem temporaryUIElement = null;
        private static Object temporaryUIElementProperty = null;

        /// <summary>
        /// Method to open an application
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("launch")]
        [HttpPost]
        public HttpResponseMessage OpenApplication(HttpRequestMessage data)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing OPEN Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                LaunchActionData actionData = jsonSerializer.Deserialize<LaunchActionData>(data.Content.ReadAsStringAsync().Result);

                if (actionData != null)
                {
                    Console.WriteLine("OPEN: {0}", actionData.url);
                    var psi = new ProcessStartInfo(actionData.url);
                    application = TestStack.White.Application.AttachOrLaunch(psi);
                    Thread.Sleep(5000);
                    mainWindow = application.GetWindows()[0];
                    System.Diagnostics.Debug.WriteLine(application.Name + " has started");

                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StringContent(application.Name + " is running");
                    Console.WriteLine();
                    Console.WriteLine("------------------------------------   OPEN Statement Executed   ------------------------------------");
                    Console.WriteLine();
                    return response;
                }
                else
                {
                    response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    response.Content = new StringContent("Another application is already opened. Wait until the current automation gets finished");
                    Console.WriteLine();
                    Console.WriteLine("------------------------------------   OPEN Statement Executed   ------------------------------------");
                    Console.WriteLine();
                    return response;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("No such executable file in the system. Actual Error: " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   OPEN Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }

        }
        
        /// <summary>
        /// Method to perform type action on an element
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("type")]
        public HttpResponseMessage TypeAction(HttpRequestMessage data)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing TYPE Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                TypeActionData actionData = jsonSerializer.Deserialize<TypeActionData>(data.Content.ReadAsStringAsync().Result);

                Console.WriteLine("TYPE: {0}", actionData.locator);
                mainWindow.WaitWhileBusy();
                IUIItem element = elementHandler.getIUIElement(mainWindow, actionData.locator);
                element.Focus();
                element.Enter(actionData.value);
                //element.SetValue(actionData.value);
                mainWindow.WaitWhileBusy();

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Success");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   TYPE Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Element not found. Actual Error: " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   TYPE Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
        }

        /// <summary>
        /// Method to perform click action on an element
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("click")]
        public HttpResponseMessage ClickAction(HttpRequestMessage data)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing CLICK Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                ClickActionData actionData = jsonSerializer.Deserialize<ClickActionData>(data.Content.ReadAsStringAsync().Result);

                Console.WriteLine("CLICK: {0}", actionData.locator);
                IUIItem element = null;
                if (actionData.locator.Contains("^"))
                {
                    String elementIdentifierNext = actionData.locator.Split('^')[0];
                    String elementIdentifier = actionData.locator.Split('^')[1];
                    mainWindow.Focus();
                    mainWindow.Click();
                    bool notVisible = true;
                    while (notVisible)
                    {
                        element = elementHandler.getIUIElement(mainWindow, elementIdentifierNext);
                        if (element.IsOffScreen)
                        {
                            IUIItem elementScroll = elementHandler.getIUIElement(mainWindow, elementIdentifier);
                            elementScroll.Focus();
                            elementScroll.Click();
                            mainWindow.WaitWhileBusy();
                        }
                        else
                        {
                            notVisible = false;
                        }

                    }
                    element.Focus();
                    element.Click();
                    mainWindow.WaitWhileBusy();
                }
                else
                {
                    element = elementHandler.getIUIElement(mainWindow, actionData.locator);
                    element.Focus();
                    element.Click();
                    mainWindow.WaitWhileBusy();
                }
                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Success");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   CLICK Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Element not found. Actual Error: " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   CLICK Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }

        }


        /// <summary>
        /// Method to check the availability of an element
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("checkElementPresent")]
        [HttpPost]
        public HttpResponseMessage checkElementPresentAction(HttpRequestMessage data)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing CHECKELEMENTPRESENT Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                ClickActionData actionData = jsonSerializer.Deserialize<ClickActionData>(data.Content.ReadAsStringAsync().Result);
                IUIItem element = null;
                Window window = null;

                Console.WriteLine("CHECKELEMENTPRESENT: {0}", actionData.locator);
                if (actionData.locator.Contains("Window"))
                {
                    window = elementHandler.getWindow(actionData.locator);
                    mainWindow = window;
                }
                else
                {
                    element = elementHandler.getIUIElement(mainWindow, actionData.locator);
                }

                
                response = new HttpResponseMessage(HttpStatusCode.OK);

                if (element != null || window != null)
                {
                    response.Content = new StringContent("True");
                }
                else
                {
                    response.Content = new StringContent("False");
                }
                Console.WriteLine(response.Content);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   CHECKELEMENTPRESENT Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Error while executing. Actual Error: " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   CHECKELEMENTPRESENT Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }

        }

        /// <summary>
        /// Method to close the already opened application
        /// </summary>
        /// <returns></returns>
        [Route("close")]
        [HttpGet]
        public HttpResponseMessage CloseApplication()
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing CLOSE Statement   ------------------------------------");
            Console.WriteLine();
            if (application != null)
            {
                var processes = Process.GetProcessesByName(application.Name);
                foreach (var process in processes)
                {
                    process.Kill();
                }
                application = null;
                mainWindow = null;

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Application closed");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   CLOSE Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            else
            {
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("No application is opened in the web service..");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   CLOSE Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
        }

        /// <summary>
        /// RightClickAction()
        /// </summary>
        /// <param name="elementIdentifier"></param>
        /// <returns></returns>
        [Route("rightclick")]
        [HttpPost]
        public HttpResponseMessage RightClickAction(HttpRequestMessage data)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing RIGHTCLICK Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                ClickActionData actionData = jsonSerializer.Deserialize<ClickActionData>(data.Content.ReadAsStringAsync().Result);
                Thread.Sleep(1000);

                Console.WriteLine("RIGHTCLICK: {0}", actionData.locator);
                IUIItem element = elementHandler.getIUIElement(mainWindow, actionData.locator);
                element.Focus();
                element.Click();
                Thread.Sleep(500);
                element.RightClick();
                Thread.Sleep(500);
                mainWindow.WaitWhileBusy();

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Success");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   RIGHTCLICK Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Error while executing. Actual Error: " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   RIGHTCLICK Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }

        }

        /// <summary>
        /// DoubleClickAction()
        /// </summary>
        /// <param name="elementIdentifier"></param>
        /// <returns></returns>
        [Route("doubleclick")]
        [HttpPost]
        public HttpResponseMessage DoubleClickAction(HttpRequestMessage data)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing DOUBLECLICK Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                ClickActionData actionData = jsonSerializer.Deserialize<ClickActionData>(data.Content.ReadAsStringAsync().Result);

                Console.WriteLine("DOUBLECLICK: {0}", actionData.locator);
                IUIItem element = elementHandler.getIUIElement(mainWindow, actionData.locator);
                element.DoubleClick();
                mainWindow.WaitWhileBusy();

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Success");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   DOUBLECLICK Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Error while executing. Actual Error: " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   DOUBLECLICK Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }

        }

        /// <summary>
        /// SelectWindowAction()
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("selectwindow")]
        [HttpPost]
        public HttpResponseMessage SelectWindowAction(HttpRequestMessage data)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing SELECTWINDOW Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                ClickActionData actionData = jsonSerializer.Deserialize<ClickActionData>(data.Content.ReadAsStringAsync().Result);

                Console.WriteLine("SELECTWINDOW: {0}", actionData.locator);
                Window window = elementHandler.getWindow(actionData.locator);
                mainWindow = window;
                mainWindow.Focus();
                mainWindow.WaitWhileBusy();

                resetMousePointer();


                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Success");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   SELECTWINDOW Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                //response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                //response.Content = new StringContent("Error while executing. Actual Error: " + e.Message);
                //return response;
                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Success");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   SELECTWINDOW Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }

        }

        private void resetMousePointer() 
        {
            try
            {

                int heigth = SystemInformation.VirtualScreen.Height / 2;
                int width = SystemInformation.VirtualScreen.Width / 2;

                Mouse.Instance.Location = new Point(width, heigth);
                //Cursor.Hide();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// SelectDropDownElementAction()
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Route("select")]
        [HttpPost]
        public HttpResponseMessage SelectDropDownElementAction(HttpRequestMessage data)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing SELECT Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                SelectActionData actionData = jsonSerializer.Deserialize<SelectActionData>(data.Content.ReadAsStringAsync().Result);

                //string locator = HttpUtility.UrlDecode(locatorIdentifier);
                Console.WriteLine("SELECT: {0}", actionData.locator);
               String locator = actionData.locator;
               if(locator.Contains("list-box"))
               {
                   TestStack.White.UIItems.ListBoxItems.ListBox element = (TestStack.White.UIItems.ListBoxItems.ListBox)elementHandler.getIUIElement(mainWindow, actionData.locator);
                   element.Select(actionData.selector);
               }
               else if (locator.Contains("combo-box"))
               {
               
                   TestStack.White.UIItems.ListBoxItems.ComboBox element = (TestStack.White.UIItems.ListBoxItems.ComboBox)elementHandler.getIUIElement(mainWindow, actionData.locator);
                   element.Select(actionData.selector);
               }
           //    else if (locator.Contains("list-item"))
             //  {
               //    TestStack.White.UIItems.ListBoxItems.ListItem element = (TestStack.White.UIItems.ListBoxItems.ListItem)elementHandler.getIUIElement(mainWindow, actionData.locator);
                //   element.Select();
              // }
               else if (locator.Contains("list-items"))
               {
                   TestStack.White.UIItems.ListBoxItems.ListItems element = (TestStack.White.UIItems.ListBoxItems.ListItems)elementHandler.getIUIElement(mainWindow, actionData.locator);
                   element.Select(actionData.selector);
               }
              
               
            //    TestStack.White.UIItems.ListBoxItems.ListBox element = (TestStack.White.UIItems.ListBoxItems.ListBox)elementHandler.getIUIElement(mainWindow, actionData.locator);


                
                mainWindow.WaitWhileBusy();

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent("Success");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   SELECT Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Error while executing. Actual Error: " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   SELECT Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }

        }

        /// <summary>
        /// CheckObjectPropertyAction()
        /// </summary>
        /// <param name="elementIdentifier"></param>
        /// <param name="expectedValue"></param>
        /// <param name="objectName"></param>
        /// <param name="propertyName"></param>
        /// <param name="customMessage"></param>
        /// <returns></returns>
        [Route("checkobjectproperty/{elementIdentifier}/{expectedValue}/{objectName}/{propertyName}/{customMessage}")]
        [HttpGet]
        public HttpResponseMessage CheckObjectPropertyAction(string elementIdentifier, string expectedValue, string objectName, string propertyName, string customMessage)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing CHECKOBJECTPROPERTY Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                string locator = HttpUtility.UrlDecode(elementIdentifier);
                string expected_value = HttpUtility.UrlDecode(expectedValue);
                string object_name = HttpUtility.UrlDecode(objectName);
                string property_name = HttpUtility.UrlDecode(propertyName);
                string custom_message = HttpUtility.UrlDecode(customMessage);
                Console.WriteLine("Locator : " + locator);
                Console.WriteLine("Expected Value : " + expected_value);
                Console.WriteLine("Object Name : " + object_name);
                Console.WriteLine("Property Name : " + property_name);
                Console.WriteLine("Custom Message : " + custom_message);
                TestStack.White.UIItems.TextBox element = (TestStack.White.UIItems.TextBox)elementHandler.getIUIElement(mainWindow, elementIdentifier);
                string element_text = element.Text;

                Console.WriteLine("CHECKOBJECTPROPERTY: {0}", locator);
                if (element_text.Equals(expected_value))
                {
                    response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StringContent("Success");
                    Console.WriteLine();
                    Console.WriteLine("------------------------------------   CHECKOBJECTPROPERTY Statement Executed   ------------------------------------");
                    Console.WriteLine();
                    return response;
                }

                mainWindow.WaitWhileBusy();
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Fail");
                Console.WriteLine();
                Console.WriteLine("------------------------------------   CHECKOBJECTPROPERTY Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Error while executing. Actual Error: " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   CHECKOBJECTPROPERTY Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }

        }

        /// <summary>
        /// setVariablePropertyAction()
        /// </summary>
        /// <param name="elementIdentifier"></param>
        /// <param name="elementProperty"></param>
        /// <returns></returns>
        [Route("setVariableProperty")]
        [HttpPost]
        public HttpResponseMessage setVariablePropertyAction(HttpRequestMessage data)
        {
            HttpResponseMessage response = null;
            Console.WriteLine();
            Console.WriteLine("------------------------------------   Executing SETVARPROPERTY Statement   ------------------------------------");
            Console.WriteLine();
            try
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                SetVarPropertyActionData actionData = jsonSerializer.Deserialize<SetVarPropertyActionData>(data.Content.ReadAsStringAsync().Result);

                Console.WriteLine("SETVARPROPERTY: {0}", actionData.locator);
                IUIItem element = elementHandler.getIUIElement(mainWindow, actionData.locator);
                object propertyValue = element.GetType().GetProperty(actionData.property).GetValue(element, null); ;
                mainWindow.WaitWhileBusy();

                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(propertyValue.ToString());
                Console.WriteLine();
                Console.WriteLine("------------------------------------   SETVARPROPERTY Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;

            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Given property is invalid.. " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   SETVARPROPERTY Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent("Error while executing. Actual Error: " + e.Message);
                Console.WriteLine();
                Console.WriteLine("------------------------------------   SETVARPROPERTY Statement Executed   ------------------------------------");
                Console.WriteLine();
                return response;
            }
        }

        ///// <summary>
        ///// getVariablePropertyAction()
        ///// </summary>
        ///// <returns></returns>
        //[Route("getVariableProperty")]
        //[HttpGet]
        //public HttpResponseMessage getVariablePropertyAction()
        //{
        //    HttpResponseMessage response = null;

        //    try
        //    {
        //        if (globalUIElement != null)
        //        {
        //            response = new HttpResponseMessage(HttpStatusCode.OK);
        //            response.Content = new StringContent(globalUIElement.Id);
        //            return response;
        //        }
        //        else
        //        {
        //            response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //            response.Content = new StringContent("No global variable was found");
        //            return response;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        //        response.Content = new StringContent("Error while executing. Actual Error: " + e.Message);
        //        return response;
        //    }
        //}
    }
}
