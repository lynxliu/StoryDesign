using StoryDesign.ViewModel.ExpressViewModel;
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

namespace StoryDesign.View.ExpressView
{
    public sealed partial class ExpressView : UserControl
    {
        public ExpressView()
        {
            this.InitializeComponent();
            DataContextChanged += ExpressView_DataContextChanged;
        }

        private void ExpressView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (args.NewValue != null)
            {
                var dc = args.NewValue as ExpressViewModel;
                if (dc != null)
                {
                    dc.TargetCanvas = targetCanvas;
                    dc.ShowExpress();
                }
            }
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            MainViewModel.SetCurrent(DataContext);
        }
    }
}
