using StoryDesign.ViewModel.DetailViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UISupport;
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

namespace StoryDesign.View.DetailView
{
    public sealed partial class GroupDetailView : UserControl
    {
        public GroupDetailView()
        {
            this.InitializeComponent();
        }

        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var dc = args.NewValue as GroupDetailViewModel;
            dc.ResourcePanel = resourcePanel;
        }

        private void ListBox_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            UIManager.ShowMenu(() =>
            {
                var contextFlyout = new MenuFlyout();

                MenuFlyoutItem addNoteItem = new MenuFlyoutItem { Text = "Add" };
                addNoteItem.Click += (s, ev) =>
                {
                    var dc = DataContext as GroupDetailViewModel;
                    if (dc != null)
                        dc.AddNoteCommand.Execute(null);
                };
                contextFlyout.Items.Add(addNoteItem);
                MenuFlyoutItem removeNoteItem = new MenuFlyoutItem { Text = "Remove" };
                removeNoteItem.Click += (s, ev) =>
                {
                    var dc = DataContext as GroupDetailViewModel;
                    if (dc != null)
                        dc.RemoveNoteCommand.Execute(null);
                };
                contextFlyout.Items.Add(removeNoteItem);
                return contextFlyout;
            }, sender as UIElement, e);
        }

        private void PositionListBox_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            UIManager.ShowMenu(() =>
            {
                var contextFlyout = new MenuFlyout();

                MenuFlyoutItem addNoteItem = new MenuFlyoutItem { Text = "Add" };
                addNoteItem.Click += (s, ev) =>
                {
                    var dc = DataContext as GroupDetailViewModel;
                    if (dc != null)
                        dc.AddPosition();
                };
                contextFlyout.Items.Add(addNoteItem);
                MenuFlyoutItem removeNoteItem = new MenuFlyoutItem { Text = "Remove" };
                removeNoteItem.Click += (s, ev) =>
                {
                    var dc = DataContext as GroupDetailViewModel;
                    if (dc != null)
                        dc.RemoveCurrentPosition();
                };
                contextFlyout.Items.Add(removeNoteItem);
                return contextFlyout;
            }, sender as UIElement, e);
        }
    }
}
