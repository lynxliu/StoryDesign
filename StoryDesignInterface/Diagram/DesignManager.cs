using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Reflection;
using Windows.UI.Xaml;

namespace StoryDesignInterface.Diagram
{
    public class DesignManager
    {
        static List<DesignButton> _ToolBarButtonList = new List<DesignButton>();
        static List<DesignButton> ToolBarButtonList { get { return _ToolBarButtonList; } }

        public static void Register(BitmapIcon icon, string name, UserControl control)
        {
            var t = ToolBarButtonList.FirstOrDefault(v => v.Name == name);
            if (t == null)
                ToolBarButtonList.Add(new DesignButton() { Icon = icon, Name = name, DesignControl = control });
        }
        public static void Register()//auto register all button
        {
            var a=Application.Current.GetType().GetTypeInfo().Assembly;
            foreach (var temp in a.DefinedTypes)
            {
                if (typeof(IDiagramDesignObject).IsAssignableFrom(temp.AsType()))
                {
                    var o= Activator.CreateInstance(temp.AsType()) as IDiagramDesignObject;
                    o.RegisterDesignButton();
                }
            }
        }
        public static UserControl GetControl(string name)
        {
            var c = ToolBarButtonList.FirstOrDefault(v => v.Name == name);
            if (c != null)
                return c.DesignControl;
            return null;
        }
    }

    public class DesignButton
    {
        public BitmapIcon Icon { get; set; }
        public string Name { get; set; }
        public UserControl DesignControl { get; set; }
    }
}
