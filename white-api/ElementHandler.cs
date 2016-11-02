using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using TestStack.White.Factory;
using TestStack.White.InputDevices;
using TestStack.White.Sessions;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace white_api
{
    class ElementHandler
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locatorInstance"></param>
        /// <returns></returns>
        public CustomUIElement getUIElement(string locator) 
        {
            CustomUIElement customElement= new CustomUIElement();

            string[] locatorInfo = locator.Split('|');

            Dictionary<string, string> extractedLocatorInfo = getLocatorInfo(locatorInfo);

            string elementType = extractedLocatorInfo["Type"];

            Type type = getElementType(elementType);//Type.GetType("TestStack.White.UIItems." + elementType + ",TestStack.White");

            try 
            {
                UIItem element1 = (UIItem)Activator.CreateInstance(type, true);
                customElement.setUIElement(element1);
                customElement.setElementID(null);
                customElement.setElementName(null);
                customElement.setFramework(null);
                customElement.setElementClassName(null);

                foreach (KeyValuePair<string, string> kvp in extractedLocatorInfo)
                {
                    if (kvp.Key.Equals("AutomationId"))
                    {
                        customElement.setElementID(kvp.Value);
                    }
                    else if (kvp.Key.Equals("Name"))
                    {
                        customElement.setElementName(kvp.Value);
                    }
                    else if (kvp.Key.Equals("Technology"))
                    {
                        customElement.setFramework(kvp.Value);
                    }
                    else if (kvp.Key.Equals("ClassName"))
                    {
                        customElement.setElementClassName(kvp.Value);
                    }
                }
            }
            catch (Exception e) 
            {
                throw new Exception(e.Message);
            }
            
            return customElement;
        }

        /// <summary>
        /// Create and return an instance of the given UI element type
        /// </summary>
        /// <param name="elementType"></param>
        /// <returns></returns>
        private Type getElementType(string elementType)
        {
            List<Type> allTypesWithSpecificName = new List<Type>();

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = a.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++)
                {
                    //Console.WriteLine(assemblyTypes[j].Name);
                    if (assemblyTypes[j].Name == elementType)
                    {
                        allTypesWithSpecificName.Add(assemblyTypes[j]);
                    }
                }
            }

            for (int j = 0; j < allTypesWithSpecificName.Count; j++)
            {
                string[] nameSpaceClasses = allTypesWithSpecificName.ElementAt(j).Namespace.Split('.');
                if (nameSpaceClasses[0] == "TestStack")
                {
                    return allTypesWithSpecificName.ElementAt(j);
                }
            }
            throw new Exception("Error while getting element type..");
        }

        /// <summary>
        /// Extract Locator Information from the given UI object locator string
        /// </summary>
        /// <param name="locatorInfo"></param>
        /// <returns></returns>
        private Dictionary<string, string> getLocatorInfo(string[] locatorInfo)
        {
            Dictionary<string, string> locatorInformation = new Dictionary<string, string>();
            int i = 0;
            while (i < locatorInfo.Length)
            {
                string[] extractedInfo = locatorInfo[i].Split('=');
                locatorInformation.Add(extractedInfo[0], extractedInfo[1]);
                i++;
            }
            return locatorInformation;
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
        /// Catch UI element from the focused window
        /// </summary>
        /// <param name="mainWindow"></param>
        /// <param name="elementIdentifier"></param>
        /// <returns></returns>
        internal IUIItem getIUIElement(TestStack.White.UIItems.WindowItems.Window mainWindow, string elementIdentifier)
        {
            try
            {
                resetMousePointer();
                //string locator = HttpUtility.UrlDecode(elementIdentifier);

                if (elementIdentifier.Contains("|"))
                {
                    CustomUIElement uiElement = getUIElement(elementIdentifier);
                    string AutomationID = uiElement.getElementID();
                    string ElementText = uiElement.getElementName();
                    WindowsFramework framework = uiElement.getFramework();
                    framework = WindowsFramework.Win32;
                    Type type = uiElement.getUIElement().GetType();


                    if (AutomationID == null)
                    {
                        SearchCriteria searchCriteria = SearchCriteria.ByText(ElementText).AndControlType(type, framework);
                        IUIItem element = mainWindow.Get(searchCriteria);
                        return element;
                    }
                    else if (ElementText == null)
                    {
                        SearchCriteria searchCriteria = SearchCriteria.ByAutomationId(AutomationID).AndControlType(type, framework);
                        IUIItem element = mainWindow.Get(searchCriteria);
                        return element;
                    }
                    else
                    {
                        SearchCriteria searchCriteria = SearchCriteria.ByAutomationId(AutomationID).AndByText(ElementText).AndControlType(type, framework);
                        IUIItem element = mainWindow.Get(searchCriteria);
                        return element;
                    }
                }
                else
                {
                    SearchCriteria xpathCriteria = SearchCriteria.ByXPath(elementIdentifier, mainWindow);
                    return mainWindow.Get(xpathCriteria);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Cannot identify the UIObject.. " + e.Message);
            }
        }

        private CustomUIElement getXPathUIElement(Window mainWindow, string elementIdentifier)
        {
            CustomUIElement customElement = new CustomUIElement();

            string locator = HttpUtility.UrlDecode(elementIdentifier);
            string[] locatorInfo = locator.Split('|');

            Dictionary<string, string> extractedLocatorInfo = getLocatorInfo(locatorInfo);

            string elementType = extractedLocatorInfo["type"];

            Type type = getElementType(elementType);//Type.GetType("TestStack.White.UIItems." + elementType + ",TestStack.White");

            try
            {
                UIItem element1 = (UIItem)Activator.CreateInstance(type, true);
                customElement.setUIElement(element1);
                customElement.setElementID(null);
                customElement.setElementName(null);
                customElement.setFramework(null);

                foreach (KeyValuePair<string, string> kvp in extractedLocatorInfo)
                {
                    if (kvp.Key.Equals("id"))
                    {
                        customElement.setElementID(kvp.Value);
                    }
                    else if (kvp.Key.Equals("name"))
                    {
                        customElement.setElementName(kvp.Value);
                    }
                    else if (kvp.Key.Equals("technology"))
                    {
                        customElement.setFramework(kvp.Value);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return customElement;
        }

        /// <summary>
        /// Get Window from the opened Window List
        /// </summary>
        /// <param name="locator"></param>
        /// <returns></returns>
        internal Window getWindow(string locator)
        {
            CustomUIElement uiElement = getUIElement(locator);
            Window window = null;

            if (uiElement != null)
            {
                AutomationElement rootElement = AutomationElement.RootElement;
                var winCollection = rootElement.FindAll(TreeScope.Children, Condition.TrueCondition);

                foreach (AutomationElement w in winCollection)
                {
                    if (w.Current.Name.Equals(uiElement.getElementName()) || w.Current.AutomationId.Equals(uiElement.getElementID()) || w.Current.ClassName.Equals(uiElement.getElementClassName()))
                    {
                        window = new Win32Window(w, WindowFactory.Desktop, InitializeOption.NoCache, new NullWindowSession());
                    }
                }

                //List<Window> windows = WindowFactory.Desktop.DesktopWindows();
                //foreach (Window w in windows)
                //{
                //    //Console.WriteLine(w.Name);
                //    if (w.Name.Equals(uiElement.getElementName()) || w.Id.Equals(uiElement.getElementID()))
                //    {
                //        window = w;
                //    }
                //}

                if (window != null)
                    return window;
            }

            throw new Exception("Error while getting window..");
        }
    }
}
