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
    public sealed partial class FateDiagramView : UserControl
    {
        public FateDiagramView()
        {
            this.InitializeComponent();
            DataContextChanged += ExpressView_DataContextChanged;

        }
        private void ExpressView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue != null)
            {
                var dc = args.NewValue as FateDiagramViewModel;
                if (dc == null) return;
                dc.TargetCanvas = TimeCanvas;

            }
        }
        private void TimeCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        private void TimeCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

        }

        private void TimeCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {

        }
    }
}
