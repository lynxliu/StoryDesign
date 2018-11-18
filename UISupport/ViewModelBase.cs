using CommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Reflection;

namespace UISupport
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        bool _isSynchronize = true;
        public bool IsSynchronize
        {
            get { return _isSynchronize; }
            set { _isSynchronize = value; }
        }

        bool _IsChanged = false;
        public virtual bool IsChanged { get { return _IsChanged; } set { _IsChanged = value; } }

        public ViewModelBase()
        {

        }

        public Action<string> SynchronizeChange { get; set; }//引发其他的viewmodel的变化

        public virtual void OnPropertyChanged(string name,bool isSynchronize=true)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
                IsChanged = true;
                PropertyChanged(this, new PropertyChangedEventArgs("Changed"));
                if(isSynchronize&&IsSynchronize)
                    SynchronizeChange?.Invoke(name);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RefreshAllProperty()
        {
            var pl = GetType().GetProperties();
            foreach(var p in pl)
            {
                OnPropertyChanged(p.Name);
            }
        }

    }

    public abstract class SupportConsistentViewModel : ViewModelBase,IStatusSupportObject
    {
        public virtual string GetStatus()
        {
            return CommonProc.ConvertObjectToString(this);
        }

        public abstract void SetStatus(string status); //should overwrite this function

        private DateTime _CreateTime = DateTime.Now;
        public DateTime CreateTime
        {
            get { return _CreateTime; }
            set { _CreateTime = value; OnPropertyChanged("CreateTime"); }
        }

        protected DateTime _LastModifyTime = DateTime.Now;
        public DateTime LastModifyTime
        {
            get { return _LastModifyTime; }
            set { _LastModifyTime = value; OnPropertyChanged("LastModifyTime"); }
        }


        public void UpdateStatus()
        {
            Status = GetStatus();
        }

        public void LoadStatus()
        {
            SetStatus(Status);
        }

        public string Status { get; set; }
    }
    public class ShowObject : ViewModelBase
    {
        object _TargetObject;
        public object TargetObject
        {
            get { return _TargetObject; }
            set { _TargetObject = value; }
        }
        public Guid ObjectID
        {
            get
            {
                if (TargetObject == null) return Guid.Empty;
                var r = CommonProc.GetID(TargetObject);
                if (r != null && r is Guid)
                    return ( Guid)r;
                return Guid.Empty;
            }
        }
        public Uri Icon
        {
            get {
                if (TargetObject == null) return null;
                var r = CommonProc.GetPropertyValue(TargetObject, new List<string>() { "Icon", "icon" });
                if (r != null && r is Uri)
                    return r as Uri;
                return null;
            }
        }

        public string Name
        {
            get
            {
                if (TargetObject == null) return null;
                var r = CommonProc.GetPropertyValue(TargetObject, new List<string>() { "Name", "name","Title","title","Text","text" });
                if (r != null && r is string)
                    return r.ToString();
                return null;
            }
        }
        public string Memo
        {
            get
            {
                if (TargetObject == null) return null;
                var r = CommonProc.GetPropertyValue(TargetObject, new List<string>() { "Memo", "memo", "Description", "description", "Text", "text" });
                if (r != null && r is string)
                    return r.ToString();
                return null;
            }
        }
    }
    public class EntityBase : SupportConsistentViewModel, IDataObject
    {
        private BitmapImage _Icon;
        public BitmapImage Icon
        {
            get { return _Icon; }
            set
            {
                _Icon = value; OnPropertyChanged("Icon");
                LastModifyTime = DateTime.Now;
            }
        }

        private string _Name = "";
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                LastModifyTime = DateTime.Now;
                OnPropertyChanged("Name");
            }
        }
        private string _Memo = "";
        public string Memo
        {
            get { return _Memo; }
            set
            {
                _Memo = value;
                LastModifyTime = DateTime.Now;
                OnPropertyChanged("Memo");
            }
        }
        public virtual string AbstractInfo
        {
            get
            {
                string s = "";
                if (!string.IsNullOrEmpty(Name))
                    s += Name ;
                
                    if (!string.IsNullOrEmpty(Memo))
                    if (string.IsNullOrEmpty(s))
                        s = Memo;
                    else
                        s += ":" + Memo;

                return s;
            }
        }
        public override void OnPropertyChanged(string name,bool isSynchronize=true)
        {
            base.OnPropertyChanged(name, isSynchronize);
            _LastModifyTime = DateTime.Now;
            base.OnPropertyChanged("LastModifyTime");
        }
        public override void SetStatus(string status) { }

        private Guid _ObjectID = Guid.NewGuid();
        public Guid ObjectID
        {
            get { return _ObjectID; }
            set { _ObjectID = value; OnPropertyChanged("ObjectID"); }
        }

        List<string> _KeyWordList = new List<string>();
        public List<string> KeyWordList { get { return _KeyWordList; } }
    }

    public class ControlViewModel : ViewModelBase, IIdentifiedObject,IDataObject
    {
        //private Guid _ObjectID = Guid.NewGuid();//control view model is different to dtatobject, so use different id
        public Guid ObjectID
        {
            get { return DataObject.ObjectID; }
            set { DataObject.ObjectID = value; OnPropertyChanged("ObjectID"); }
        }

        private EntityBase _DataObject = null;
        public EntityBase DataObject
        {
            get { return _DataObject; }
            set
            {
                _DataObject = value;
                OnPropertyChanged("CreateTime");
                OnPropertyChanged("LastModifyTime");
                OnPropertyChanged("Icon");
                OnPropertyChanged("Name");
                OnPropertyChanged("Memo");
            }
        }

        public DateTime CreateTime
        {
            get { return DataObject.CreateTime; }
            set { DataObject.CreateTime = value; OnPropertyChanged("CreateTime"); }
        }

        public DateTime LastModifyTime
        {
            get { return DataObject.LastModifyTime; }
            set { DataObject.LastModifyTime = value; OnPropertyChanged("LastModifyTime"); }
        }
        public BitmapImage Icon
        {
            get { return DataObject.Icon; }
            set
            {
                DataObject.Icon = value;
                OnPropertyChanged("Icon");
                OnPropertyChanged("LastModifyTime");
            }
        }

        public string Name
        {
            get { return DataObject.Name; }
            set
            {
                DataObject.Name = value;
                OnPropertyChanged("LastModifyTime");
                OnPropertyChanged("Name");
            }
        }

        public string Memo
        {
            get { return DataObject.Memo; }
            set
            {
                DataObject.Memo = value;
                OnPropertyChanged("LastModifyTime");
                OnPropertyChanged("Memo");
            }
        }
        public virtual string AbstractInfo
        {
            get
            {
                string s = "";
                if (!string.IsNullOrEmpty(Name))
                    s += Name;

                if (!string.IsNullOrEmpty(Memo))
                    if (string.IsNullOrEmpty(s))
                        s = Memo;
                    else
                        s += ":" + Memo;

                return s;
            }
        }
        List<string> _KeyWordList = new List<string>();
        public List<string> KeyWordList { get { return _KeyWordList; } }
    }

    public class EntityViewModelBase<T>:ViewModelBase where T:EntityBase
    {
        private T _TargetObject = null;
        public T TargetObject
        {
            get { return _TargetObject; }
            set
            {
                _TargetObject = value;
                OnPropertyChanged("Name");
                OnPropertyChanged("Memo");
            }
        }
        public Guid ObjectID
        {
            get { return TargetObject.ObjectID; }
            set { TargetObject.ObjectID = value; OnPropertyChanged("ObjectID"); }
        }
        public string Name
        {
            get { return TargetObject.Name; }
            set
            {
                TargetObject.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Memo
        {
            get { return TargetObject.Memo; }
            set
            {
                TargetObject.Memo = value;
                OnPropertyChanged("Memo");
            }
        }
    }
}
