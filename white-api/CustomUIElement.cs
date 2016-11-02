using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems;

namespace white_api
{
    class CustomUIElement
    {
        private UIItem UI_Element;
        private string element_name;
        private string element_id;
        private WindowsFramework framework;
        private string element_classname;


        public CustomUIElement()
        {
            this.UI_Element = null;
            this.element_name = null;
            this.element_id = null;
            this.framework = WindowsFramework.None;
            this.element_classname = null;
        }

        public CustomUIElement(UIItem uiElement, string element_name, string element_id, WindowsFramework framework, string className)
        {
            this.UI_Element = uiElement;
            this.element_name = element_name;
            this.element_id = element_id;
            this.framework = framework;
            this.element_classname = className;
        }

        public void setUIElement(UIItem uiElement)
        {
            this.UI_Element = uiElement;
        }

        public void setElementName(string elementName)
        {
            this.element_name = elementName;
        }

        public void setElementID(string elementID)
        {
            this.element_id = elementID;
        }

        public void setFramework(string framework)
        {
            this.framework = WindowsFramework.None;
        }

        public UIItem getUIElement()
        {
            return this.UI_Element;
        }

        public string getElementName()
        {
            return this.element_name;
        }

        public string getElementID()
        {
            return this.element_id;
        }

        public WindowsFramework getFramework()
        {
            return this.framework;
        }

        public void setElementClassName(string className)
        {
            this.element_classname = className;
        }

        public string getElementClassName()
        {
            return this.element_classname;
        }
    }
}
