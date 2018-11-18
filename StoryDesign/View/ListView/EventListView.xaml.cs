using StoryDesign.ViewModel.DetailViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
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

namespace StoryDesign.View.ListView
{
    public sealed partial class EventListView : UserControl
    {
        public EventListView()
        {
            this.InitializeComponent();
        }
        MenuFlyout contextFlyout;
        public void InitContextMemu()
        {
            contextFlyout = new MenuFlyout();
            MenuFlyoutItem copyItem = new MenuFlyoutItem
            {
                Text = "Copy",
            };
            copyItem.Click += (s, e) =>
            {
                var data = new Windows.ApplicationModel.DataTransfer.DataPackage();
                data.Properties.Add("entity", (s as FrameworkElement).DataContext);
                Clipboard.SetContent(data);
            };
            contextFlyout.Items.Add(copyItem);
            contextFlyout.Items.Add(new MenuFlyoutSeparator());

            MenuFlyoutItem addItem = new MenuFlyoutItem
            {
                Text = "Add",
            };
            addItem.Click += (s, e) =>
            {
                var command = DataContext.GetType().GetMethod("Add");
                if (command != null)
                    command.Invoke(DataContext, new object[0]);
            };
            contextFlyout.Items.Add(addItem);

            MenuFlyoutItem openItem = new MenuFlyoutItem
            {
                Text = "Open",
            };
            openItem.Click += (s, e) =>
            {
                var command = DataContext.GetType().GetMethod("Open");
                if (command != null)
                    command.Invoke(DataContext, new object[1] { (s as FrameworkElement).DataContext });
            };
            contextFlyout.Items.Add(openItem);

            MenuFlyoutItem removeItem = new MenuFlyoutItem
            {
                Text = "Remove",
            };
            removeItem.Click += (s, e) =>
            {
                var command = DataContext.GetType().GetMethod("Remove");
                if (command != null)
                    command.Invoke(DataContext, new object[1] { (s as FrameworkElement).DataContext });
            };
            contextFlyout.Items.Add(removeItem);

            //contextFlyout.ShowAt(targetControl, new Point(x,y));

        }

        private void TextBlock_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (contextFlyout == null)
                InitContextMemu();
            contextFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }

        private void TextBlock_DragStarting(UIElement sender, DragStartingEventArgs args)
        {
            //args.Data.SetData("entity", (sender as FrameworkElement).DataContext);
            args.Data.Properties.Add("entity", (sender as FrameworkElement).DataContext);
        }

        private void StackPanel_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            MainViewModel.SetCurrent((sender as FrameworkElement).DataContext);
        }
    }
}
