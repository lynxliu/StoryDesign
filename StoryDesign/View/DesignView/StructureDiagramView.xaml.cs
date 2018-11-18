using DesignTool.Lib.View;
using StoryDesign.ViewModel.DesignViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace StoryDesign.View.DesignView
{
    public sealed partial class StructureDiagramView : UserControl
    {
        public StructureDiagramView()
        {
            this.InitializeComponent();

        }
        public DesignCanvas GetDesignCanvas()
        {
            return designCanvas;
        }

        private void DesignCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            MainViewModel.SetCurrent(DataContext);
        }
    }
}
