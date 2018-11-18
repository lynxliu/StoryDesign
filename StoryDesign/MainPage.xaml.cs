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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StoryDesign
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            
            //DataContext = new MainPageViewModel();
            //SetView(MainPageViewModel.mainViewModel.CurrentMainView);
            //DataContext = new MainPageViewModel();
        }

        //private void TextBox_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{

        //    MainPageViewModel.SetCurrent(DataContext);
        //}

        public void SetView(FrameworkElement view)
        {
            mainView.Child = view;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainPageViewModel.ActiveMainView();
        }
    }
}
