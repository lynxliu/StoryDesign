using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace StoryDesignInterface.Diagram
{
    public interface IDiagramDesignObject
    {
        //BitmapIcon Icon { get; }
        //string DesignButtonText { get; }
        //UserControl DesignControl { get; }
        void RegisterDesignButton();
    }
}
