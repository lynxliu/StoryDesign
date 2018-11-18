using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using CommonLib;
using DesignTool.Lib.Model;
using UISupport;
using Newtonsoft.Json;
using Windows.UI.Xaml;
using System.Windows.Input;
using System.ComponentModel;

namespace DesignTool.Lib.ViewModel
{
    public class DesignCanvasViewModel: INotifyPropertyChanged
    {
        //Dictionary<string, Func<FrameworkElement>> _CreateCommandList = new Dictionary<string, Func<FrameworkElement>>();
        //public Dictionary<string, Func<FrameworkElement>> CreateCommandList { get { return _CreateCommandList; } }
        //public DesignCanvasViewModel() { InitMenuList(); }

        //public void InitMenuList()
        //{
        //    foreach(var v in CreateCommandList)
        //    {
        //        MenuList.Add(new Tuple<string, ICommand>(v.Key, new CommonCommand((o) => { CurrentCreateItemFunc = v.Value; })));
        //    }
        //}
        public Func<FrameworkElement> CurrentCreateItemFunc { get; set; }
        //List<Tuple<string, ICommand>> _MenuList = new List<Tuple<string, ICommand>>();
        //public List<Tuple<string, ICommand>> MenuList { get { return _MenuList; } }

        //public Visibility UseCreateMenuList
        //{
        //    get { if (MenuList.Count > 0) return Visibility.Visible; return Visibility.Collapsed; }
        //}
        public void ActiveData(Guid id)
        {
            foreach(var item in ItemList.Where(v => v.TargetObjectID == id))
            {
                item.IsActive = true;
            }
            foreach (var item in LinkList.Where(v => v.TargetObjectID == id))
            {
                item.IsSelected = true;
            }
        }
        double _Height = 2000;
        public double Height
        {
            get { return _Height; }
            set { _Height = value;if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Height")); }
        }
        double _Width = 3000;
        public double Width
        {
            get { return _Width; }
            set { _Width = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Width")); }
        }

        List<DesignItem> _ItemList = new List<DesignItem>();
        public List<DesignItem> ItemList
        {
            get { return _ItemList; }
        }

        List<DesignLink> _LinkList = new List<DesignLink>();

        public event PropertyChangedEventHandler PropertyChanged;

        public List<DesignLink> LinkList
        {
            get { return _LinkList; }
        }

        [JsonIgnore]
        public Action<object> RefreshDesign { get; set; }

        public List<DesignItem> GetTargetItemList(DesignItem source)
        {
            var l = new List<DesignItem>();
            var linkList = LinkList.Where(v => v.SourceID == source.TargetObjectID);
            foreach(var v in linkList)
            {
                var item = ItemList.FirstOrDefault(i => i.TargetObjectID == v.TargetID);
                if(item!=null)
                    l.Add(item);
            }
            return l;
        }

        public List<DesignItem> GetSourceItemList(DesignItem target)
        {
            var l = new List<DesignItem>();
            var linkList = LinkList.Where(v => v.TargetID == target.TargetObjectID);
            foreach (var v in linkList)
            {
                var item = ItemList.FirstOrDefault(i => i.TargetObjectID == v.SourceID);
                if (item != null)
                    l.Add(item);
            }

            return l;
        }

        public void ValidRelation()
        {
            var tl = new List<DesignLink>();
            LinkList.ToList().ForEach(v =>
                {
                    if(!ItemList.Any(item=>item.TargetObjectID==v.SourceID)||
                    !ItemList.Any(item => item.TargetObjectID == v.TargetID))
                        tl.Add(v);
                });
            tl.ForEach(v=>LinkList.Remove(v));
        }

        public void Save()
        {
            var s = CommonProc.ConvertObjectToString(this);
            CommonProc.SaveStringToFile(s);

        }
        public void Load(Action callBack)
        {
            CommonProc.LoadStringFromFile((f,s)=>
            {
                Load(s);
                if (callBack != null)
                    callBack();
            });

        }
        public void Load(string status)
        {
            if (status == null) return;

            var pt = CommonProc.ConvertStringToObject<DesignCanvasViewModel>(status);
            ItemList.Clear();
            LinkList.Clear();

            pt.ItemList.ToList().ForEach(v => ItemList.Add(v));
            pt.LinkList.ToList().ForEach(v => LinkList.Add(v));
        }
        public void Refresh()
        {
            var rl=LinkList.Where(v => v.SourceID == null && v.TargetID == null).ToList();
            rl.ForEach(v => LinkList.Remove(v));

            foreach(var v in ItemList)
            {
                v.InLinkList.Clear();
                v.OutLinkList.Clear();
                var il = LinkList.Where(l => l.SourceID == v.TargetObjectID).ToList();
                il.ForEach(l => v.OutLinkList.Add(l));
                var ol = LinkList.Where(l => l.TargetID == v.TargetObjectID).ToList();
                ol.ForEach(l => v.InLinkList.Add(l));

            }
            if (RefreshDesign != null)
                RefreshDesign(this);
        }
        public void RefreshRadius()
        {
            List<DesignLink> linkList = new List<DesignLink>();
            LinkList.ForEach(l =>
            {
                try
                {
                    RefreshRadius(l, linkList);
                }
                catch(Exception e)
                {
                    LogSupport.Error(e.Message);
                }
            });
        }
        void RefreshRadius(DesignLink link,List<DesignLink> linkList)
        {
            var s = ItemList.FirstOrDefault(v => v.DesignItemID == link.SourceDesignItemID);
            var t= ItemList.FirstOrDefault(v => v.DesignItemID == link.TargetDesignItemID);
            if (s == null || t == null)
                throw new Exception("Can not find valid design item, source id is " + link.SourceDesignItemID.ToString() + ", target id is " + link.TargetDesignItemID.ToString());
            var l = LinkList.Where(v => v.SourceDesignItemID == link.SourceDesignItemID &&
                v.TargetDesignItemID == link.TargetDesignItemID &&
                v.TargetPosition == link.TargetPosition && v.SourcePosition == link.SourcePosition &&
                 link != v&&!linkList.Contains(v)).ToList();
            if (l.Count > 0)
            {
                l.ForEach(tl =>
                {
                    tl.RadiusRadio = 0;
                    tl.LineSweepDirection = Windows.UI.Xaml.Media.SweepDirection.Clockwise;
                });
                l.ForEach(tl => tl.SetCurveRadius(s, t));
                linkList.AddRange(l);
            }
        }
        public void DeleteItem(DesignItem item)
        {
            if (ItemList.Contains(item))
                ItemList.Remove(item);

            item.InLinkList.ForEach(v =>
            {
                if (LinkList.Contains(v))
                    LinkList.Remove(v);
            });
            item.OutLinkList.ForEach(v =>
            {
                if (LinkList.Contains(v))
                    LinkList.Remove(v);
            });
            //var dl=LinkList.Where(v => v.SourceID == item.TargetObjectID || v.TargetID == item.TargetObjectID);
            //ItemList.ForEach(v =>
            //{
            //    foreach(var l in dl)
            //    {
            //        if (v.InLinkList.Contains(l))
            //            v.InLinkList.Remove(l);
            //        if (v.OutLinkList.Contains(l))
            //            v.OutLinkList.Remove(l);
            //    }
            //});
            //foreach (var l in dl)
            //    LinkList.Remove(l);
        }
        public void DeleteLink(DesignLink link)
        {
            if (LinkList.Contains(link))
                LinkList.Remove(link);
            ItemList.ForEach(v =>
            {
                if (v.InLinkList.Contains(link))
                    v.InLinkList.Remove(link);
                if (v.OutLinkList.Contains(link))
                    v.OutLinkList.Remove(link);
            });
            var s = ItemList.FirstOrDefault(v => v.DesignItemID == link.SourceDesignItemID);
            var t = ItemList.FirstOrDefault(v => v.DesignItemID == link.TargetDesignItemID);
            if (s == null || t == null)
                return;
            var l = LinkList.Where(v => v.SourceDesignItemID == link.SourceDesignItemID &&
                v.TargetDesignItemID == link.TargetDesignItemID &&
                v.TargetPosition == link.TargetPosition && v.SourcePosition == link.SourcePosition &&
                 link != v ).ToList();
            if (l.Count > 0)
            {
                l.ForEach(tl =>
                {
                    tl.RadiusRadio = 0;
                    tl.LineSweepDirection = Windows.UI.Xaml.Media.SweepDirection.Clockwise;
                });
                l.ForEach(tl => tl.SetCurveRadius(s, t));

            }

        }

    }
}
