using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using DesignTool.Lib.Model;
using DesignTool.Lib.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Input;
using UISupport.Diagram.Controls;
using Windows.ApplicationModel.DataTransfer;
using System.Reflection;
using CommonLib;
using DesignTool.Lib.View.DesignControlSupport;

namespace DesignTool.Lib.View
{
    /// <summary>
    /// Interaction logic for DesignCanvas.xaml
    /// </summary>
    public partial class DesignCanvas : UserControl
    {
        public DesignCanvas()
        {
            InitializeComponent();
            TargetDesignCanvas.PointerPressed += TargetDesignCanvas_PointerPressed;
            //foreach(var v in DesignManager.CreateCommandList)
            //{

            //}

        }
        MenuFlyout contextFlyout;
        public void InitContextMemu(Dictionary<string, Func<FrameworkElement>> createCommandList)
        {
            contextFlyout = new MenuFlyout();

            MenuFlyoutItem pasteItem = new MenuFlyoutItem { Text = "Paste" };
            pasteItem.Click += (s, e) =>
            {
                object data;
                if (Clipboard.GetContent().Properties.TryGetValue("entity", out data))
                {
                    ProcessEntity(data, contextMenuPoint);
                }
                if (Clipboard.GetContent().Properties.TryGetValue("link", out data))
                {
                    ProcessLink(data, contextMenuPoint);
                }
            };
            contextFlyout.Items.Add(pasteItem);
            contextFlyout.Items.Add(new MenuFlyoutSeparator());

            MenuFlyoutItem saveItem = new MenuFlyoutItem { Text = "Save" };
            saveItem.Click += SaveMenu_OnClick;
            contextFlyout.Items.Add(saveItem);
            MenuFlyoutItem loadItem = new MenuFlyoutItem { Text = "Load" };
            loadItem.Click += LoadMenu_OnClick;
            contextFlyout.Items.Add(loadItem);

            contextFlyout.Items.Add(new MenuFlyoutSeparator());

            if (createCommandList!=null)
                foreach(var ccommand in createCommandList)
                {
                    MenuFlyoutItem addItem = new MenuFlyoutItem
                    {
                        Text = "Add "+ccommand.Key,
                    };
                    addItem.Click += (s, e) =>
                    {
                        var control = ccommand.Value();
                        var dcontrol = DesignManager.CreateDesignControl(control);
                        Canvas.SetLeft(dcontrol, contextMenuPoint.X);
                        Canvas.SetTop(dcontrol, contextMenuPoint.Y);
                        Draw(dcontrol);
                    };
                    contextFlyout.Items.Add(addItem);
                }

            contextFlyout.Items.Add(new MenuFlyoutSeparator());
            MenuFlyoutItem fitItem = new MenuFlyoutItem { Text = "Auto fit" };
            loadItem.Click += (s, e) =>
            {
                ActoSize();
            };
            contextFlyout.Items.Add(fitItem);

        }
        void CreateControlInCanvas(FrameworkElement control,double x,double y)
        {
            if (control == null)
            {
                throw new Exception("Can not create control");
            }
            var dc = DataContext as DesignCanvasViewModel;
            var item = new DesignItem()
            {
                TargetControl = control,
                Left = x,
                Top = y,
                Width = double.IsNaN(control.Width) ? 200 : control.Width,
                Height = double.IsNaN(control.Height) ? 150 : control.Height
            };
            dc.ItemList.Add(item);
            var dcontrol = new DesignControl();
            dcontrol.DataContext = item;
            TargetDesignCanvas.Children.Add(dcontrol);
        }
        public void ActoSize()
        {
            double minX=0, maxX=0, minY=0, maxY=0;
            foreach (var c in TargetDesignCanvas.Children)
            {
                var control = c as FrameworkElement;
                if (control != null)
                {
                    minX = Math.Min(minX, Canvas.GetLeft(control));
                    maxX = Math.Max(maxX, Canvas.GetLeft(control) + control.ActualWidth);
                    minY = Math.Min(minY, Canvas.GetTop(control));
                    maxY = Math.Max(maxY, Canvas.GetTop(control)+control.ActualHeight);
                }
            }

            var width = maxX - minX;
            width += 100;
            var height = maxY - minY;
            height += 100;
            if(TargetDesignCanvas.ActualWidth<width)
                TargetDesignCanvas.Width = width;
            if(TargetDesignCanvas.ActualHeight<height)
                TargetDesignCanvas.Height = height;
        }
        public void Draw(FrameworkElement control)
        {
            if (control == null)
                return;
            var vm = DataContext as DesignCanvasViewModel;
            if (vm == null) return;

            TargetDesignCanvas.Children.Add(control);
            ActoSize();
            RefreshData();
        }
        public void RefreshData()
        {
            var vm = DataContext as DesignCanvasViewModel;
            if (vm == null) return;
            vm.ItemList.Clear();
            vm.LinkList.Clear();
            
            foreach(var c in TargetDesignCanvas.Children)
            {
                if(c is DesignControl)
                {
                    var controlVM = (c as DesignControl).DataContext as DesignItem;
                    if (controlVM != null)
                        vm.ItemList.Add(controlVM);
                }
                if(c is ArrowCurveSegment)
                {
                    var linkVM = (c as ArrowCurveSegment).DataContext as DesignLink;
                    if (linkVM != null)
                        vm.LinkList.Add(linkVM);
                }
            }
        }

        //void ConnectNode()
        public void RefreshView(object value)
        {
            var vm = value as DesignCanvasViewModel;
            if (vm == null) return;
            TargetDesignCanvas.Children.Clear();
            vm.ItemList.ToList().ForEach(v =>
            {
                var control = new DesignControl()
                {
                    DataContext = v
                };
                v.OutLinkList.Clear();
                foreach (var outlink in vm.LinkList.Where(u => u.SourceDesignItemID== v.DesignItemID))
                {
                    v.OutLinkList.Add(outlink);
                }
                v.InLinkList.Clear();
                foreach (var inLink in vm.LinkList.Where(u => u.TargetDesignItemID == v.DesignItemID))
                {
                    v.InLinkList.Add(inLink);
                }
                TargetDesignCanvas.Children.Add(control);
            });

            vm.LinkList.ToList().ForEach(v =>
                {
                    if(!TargetDesignCanvas.Children.Contains(v.LinkLine))
                        TargetDesignCanvas.Children.Add(v.LinkLine);
                    Canvas.SetZIndex(v.LinkLine, 10);
                });
        }

        public DesignControl SourceControl { get; set; }
        public Point SourcePoint { get; set; }
        public bool IsDrawing { get; set; }
        public FrameworkElement CurrentControl { get; set; }

        private void LoadMenu_OnClick(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as DesignCanvasViewModel;
            if (dc == null) return;
            //dc.Load(()=> RefreshDataContext(dc));
            RefreshView(dc);
        }

        private void SaveMenu_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshData();
            //var dc = DataContext as DesignCanvasViewModel;
            //if (dc == null) return;
            //dc.Save();
        }

        public void SetCreate(Func<FrameworkElement> createControl)
        {
            
            createControlFunc = createControl;
        }

        Func<FrameworkElement> _createControlFunc;
        private Func<FrameworkElement> createControlFunc
        {
            get
            {
                if (_createControlFunc == null)
                {
                    var dc = DataContext as DesignCanvasViewModel;
                    if (dc != null)
                        _createControlFunc= dc.CurrentCreateItemFunc;
                }
                return _createControlFunc;
            }
            set
            {
                _createControlFunc = value;
            }
        }

        void TargetDesignCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (createControlFunc == null)
            {
                foreach (var v in TargetDesignCanvas.Children)
                {
                    if (v is ArrowCurveSegment)
                        (v as ArrowCurveSegment).ChangeActive(false);
                    if (v is DesignControl)
                        (v as DesignControl).ChangeActive(false);
                }
                if (DesignManager.SetCurrent != null&&Parent!=null)
                {
                    var p = Parent as FrameworkElement;
                    DesignManager.SetCurrent(p.DataContext);
                }
                return;
            }
            if (DataContext == null) return;
            var dc = DataContext as DesignCanvasViewModel;
            if (dc == null) return;
            var control = createControlFunc();
            if (control == null)
            {
                throw new Exception("Can not create control");
            }
            var item = new DesignItem()
            {
                TargetControl = control,
                Left = e.GetCurrentPoint(TargetDesignCanvas).Position.X,
                Top = e.GetCurrentPoint(TargetDesignCanvas).Position.Y,
                Width = double.IsNaN(control.Width) ? 200 : control.Width,
                Height = double.IsNaN(control.Height) ? 150 : control.Height
            };
            dc.ItemList.Add(item);
            var dcontrol = new DesignControl();
            dcontrol.DataContext = item;
            TargetDesignCanvas.Children.Add(dcontrol);
            //RefreshDataContext(DataContext);
            createControlFunc = null;
            //e.Handled = true;
        }
        private async void TargetDesignCanvas_KeyUp(object sender, KeyRoutedEventArgs e)
        {
           if (CurrentControl != null)
            {
                if (e.Key == Windows.System.VirtualKey.Delete )
                {
                    var dialog = new ContentDialog()
                    {
                        Title = "Confirm Delete",
                        Content = "Selected control and its connections will be deleted from diagram, confirm?",
                        PrimaryButtonText = "OK",
                        SecondaryButtonText = "Cancel",
                        FullSizeDesired = false,
                    };

                    dialog.PrimaryButtonClick += (_s, _e) => {
                        if (TargetDesignCanvas.Children.Contains(CurrentControl))
                        {
                            var item = CurrentControl.DataContext;
                            var vm = DataContext as DesignCanvasViewModel;
                            if(item is DesignItem)
                            {
                                vm.DeleteItem(item as DesignItem);
                                
                            }
                            if (item is DesignLink)
                            {
                                vm.DeleteLink(item as DesignLink);
                            }
                            TargetDesignCanvas.Children.Remove(CurrentControl);
                        }
                    };
                    await dialog.ShowAsync();

                }

            }
        }

        private void DesignCanvas_OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            RefreshView(args.NewValue);
        }
        Point contextMenuPoint;
        private void TargetDesignCanvas_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            if (contextFlyout == null)
                InitContextMemu(DesignManager.DefaultCreateCommandList);
            contextFlyout.ShowAt(sender as UIElement, contextMenuPoint= e.GetPosition(sender as UIElement));
            e.Handled = true;
        }
        DesignControl GetItemByID(Guid id)
        {
            foreach (var c in TargetDesignCanvas.Children)
            {
                if (c is DesignControl&& (c as DesignControl).DataContext!=null&& (c as DesignControl).DataContext is DesignItem)
                {
                    var item = (c as DesignControl).DataContext as DesignItem;
                    if(item.TargetObjectID==id)
                        return c as DesignControl;
                }

            }
            return null;
        }
        DesignConnection getConnectionControl(DesignControl c, RelativePosition type)
        {
            var l = c.GetConnectionList();
            if (type == RelativePosition.Left)
                return l.FirstOrDefault(v => v.IsLeft);
            if (type == RelativePosition.Right)
                return l.FirstOrDefault(v => v.IsRight);
            if (type == RelativePosition.Bottom)
                return l.FirstOrDefault(v => v.IsBottom);
            if (type == RelativePosition.Top)
                return l.FirstOrDefault(v => v.IsTop);
            return null;
        }
        void Connect(Tuple<Guid, Guid, object> link)
        {
            var s = GetItemByID(link.Item1);
            if (s == null) return;
            var t = GetItemByID(link.Item2);
            if (t == null) return;
            var si = s.DataContext as DesignItem;
            var ti = t.DataContext as DesignItem;
            var p = DesignManager.GetLinkConnection(s.DataContext as DesignItem, t.DataContext as DesignItem);
            var sp = DesignManager.GetPoint(si, p.Item1);
            var tp = DesignManager.GetPoint(ti, p.Item2);
            var sc = getConnectionControl(s, p.Item1);
            var tc = getConnectionControl(t, p.Item2);
            var CurrentLink = new DesignLink()
            {
                SourceX = sp.X,
                SourceY = sp.Y,
                TargetX = tp.X,
                TargetY = tp.Y,
                TargetObject = link.Item3
        };
            CurrentLink.SourcePoint = sc.TargetControl;
            CurrentLink.TargetPoint = tc.TargetControl;
            si.OutLinkList.Add(CurrentLink);
            ti.InLinkList.Add(CurrentLink);

            Draw(CurrentLink.LinkLine);
        }
        void AutoConnect(List<Tuple<Guid, Guid, object>> linkList)
        {
            linkList.ForEach(v =>
            {
                Connect(v);

            });

        }
        List<Tuple<Guid, Guid, object>> FilterExistLink(List<Tuple<Guid, Guid, object>> ol)
        {
            var rl = new List<object>();
            var vm = DataContext as DesignCanvasViewModel;
            vm.LinkList.ForEach(l =>
            {
                ol.ForEach(v =>
                {
                    var id = CommonProc.GetID(v.Item3);
                    if (id!=Guid.Empty&&l.TargetObjectID == id)
                    {
                        rl.Add(v);
                    }
                });
            });
            var list = new List<Tuple<Guid, Guid, object>>();
            ol.ForEach(v =>
            {
                if (!rl.Contains(v.Item3))
                    list.Add(v);
            });
            return list;
        }
        List<Tuple<Guid, Guid, object>> GetConnectionList(FrameworkElement currentControl)
        {
            if (DesignManager.GetConnection == null) return null;

            List<Tuple<Guid, Guid, object>> ol = new List<Tuple<Guid, Guid, object>>();
            
            List<Guid> idl = new List<Guid>();
            List<Guid> linkidl = new List<Guid>();
            if (currentControl == null||(currentControl as DesignControl)==null||
                ((currentControl as DesignControl).DataContext)==null) return ol;

            var sitem = (currentControl as DesignControl).DataContext as DesignItem;
            if (sitem == null) return ol;
            var id = sitem.TargetObjectID;
            if (id == null)
                return ol;
            foreach (var c in TargetDesignCanvas.Children)
            {
                if (c is ArrowCurveSegment)
                {
                    var linkVM = (c as ArrowCurveSegment).DataContext as DesignLink;
                    linkidl.Add(linkVM.DesignLinkID);
                }
            }
            foreach (var c in TargetDesignCanvas.Children)
            {

                if (c is DesignControl)
                {
                    var controlVM = (c as DesignControl).DataContext as DesignItem;
                    if (controlVM != null&&controlVM.TargetObjectID!=null) {
                        var l = DesignManager.GetConnection(id.Value, controlVM.TargetObjectID.Value);
                        if (l != null)
                        {
                            l.ForEach(v =>
                            {
                                var tid = CommonProc.GetID(v);
                                if (tid != null && !idl.Contains(tid.Value)&&!linkidl.Contains(tid.Value))
                                {
                                    ol.Add(new  Tuple<Guid, Guid, object>( id.Value, controlVM.TargetObjectID.Value,v));
                                    idl.Add(tid.Value);
                                }
                            });
                        }

                        l = DesignManager.GetConnection( controlVM.TargetObjectID.Value, id.Value);
                        if (l != null)
                        {
                            l.ForEach(v =>
                            {
                                var tid = CommonProc.GetID(v);
                                if (tid != null && !idl.Contains(tid.Value) && !linkidl.Contains(tid.Value))
                                {
                                    ol.Add(new Tuple<Guid, Guid, object>( controlVM.TargetObjectID.Value,id.Value, v));
                                    idl.Add(tid.Value);
                                }
                            });
                        }
                    }
                }

            }

            return FilterExistLink( ol);
        }

        void AutoAddConnection(FrameworkElement currentControl)
        {
            var l = GetConnectionList(currentControl);
            AutoConnect(l);
        }
        void ProcessEntity(object data,Point point)
        {
            var control = DesignManager.CreateDesignControl(data);
            Canvas.SetLeft(control, point.X);
            Canvas.SetTop(control, point.Y);
            Draw(control);
            if (DesignManager.IsAutoConnectNode != null && DesignManager.IsAutoConnectNode())
            {
                AutoAddConnection(control);
            }
        }
        void ProcessLink(object data,Point point)
        {
            if (data == null) return;

            var l = DesignManager.GetDesignLink(data);
            if (l == null) return;

            var s = GetItemByID(l.Item1);
            var t = GetItemByID(l.Item2);
            if (s == null&&t==null)
            {

                CommonProc.ShowMessage("Error", "Link source item and target item does not exist in diagram");
                return;
            }
            if (s == null) {
                var control = DesignManager.CreateDesignControl(s);
                Canvas.SetLeft(control, point.X);
                Canvas.SetTop(control, point.Y);
                Draw(control);
                if (DesignManager.IsAutoConnectNode != null && DesignManager.IsAutoConnectNode())
                {
                    AutoAddConnection(control);
                }
             }
            if (t == null) {
                var control = DesignManager.CreateDesignControl(t);
                Canvas.SetLeft(control, point.X);
                Canvas.SetTop(control, point.Y);
                Draw(control);
                if (DesignManager.IsAutoConnectNode != null && DesignManager.IsAutoConnectNode())
                {
                    AutoAddConnection(control);
                }
            }
            Connect(l);

        }
        private void TargetDesignCanvas_Drop(object sender, DragEventArgs e)
        {
            object data;
            if (e.DataView.Properties.TryGetValue("entity", out data))
            {
                ProcessEntity(data,e.GetPosition(sender as UIElement));
            }
            if (e.DataView.Properties.TryGetValue("link", out data))
            {
                ProcessLink(data, e.GetPosition(sender as UIElement));

            }
            
            //var data = await e.DataView.GetDataAsync("entity");

        }

        private void TargetDesignCanvas_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Copy;
        }
    }

    public enum DesignCanvasStatus
    {
        CreateNew,Common
    }
}
