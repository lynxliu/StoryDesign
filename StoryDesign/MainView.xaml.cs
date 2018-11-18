using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
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

namespace StoryDesign
{
    public sealed partial class MainView : UserControl
    {
        public MainView()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            var dc = new MainViewModel() { MainContent = mainGrid, ListGrid = listGrid, PropertyView = propertyView };
            dc.Initialize();
            DataContext = dc;
        }

        private void TextBox_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

            MainViewModel.SetCurrent(DataContext);
        }

    }
}
