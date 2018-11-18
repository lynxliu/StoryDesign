using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;

namespace StoryDesign.ViewModel
{
    public abstract class EntityViewModelBase<T> : ViewModelBase , IUIActiveSupport
    {
        T _TargetObject;
        public T TargetObject
        {
            get
            {
                if (_TargetObject == null)
                {
                    _TargetObject = (T)Activator.CreateInstance(typeof(T));
                }
                return _TargetObject;
            }
            set
            {
                if (value != null)
                {
                    _TargetObject = value;
                    LoadInfo();
                    //RefreshAllProperty();
                }
            }
        }

        public virtual void LoadInfo()
        {
            IsChanged = false;
        }
        public virtual void SaveInfo()
        {
            IsChanged = false;
            var vm = MainViewModel.mainViewModel;
            if (vm != null)
                vm.RefreshAllView(this);
        }

        bool _IsActiveEnable = true;
        public bool IsActiveEnable { get { return _IsActiveEnable; } set { _IsActiveEnable = value; OnPropertyChanged("IsActiveEnable"); } }

        List<Action> _SetActive = new List<Action>();
        public List<Action> SetActive { get { return _SetActive; } }

        List<Action> _SetDeActive = new List<Action>();
        public List<Action> SetDeActive { get { return _SetDeActive; } }
    }

    public abstract class DataEntityViewModelBase<T> : EntityViewModelBase<T> where T : IDataObject
    {

        public string Name { get { if (TargetObject != null) return TargetObject.Name; return null; } set { if (TargetObject != null) TargetObject.Name = value; OnPropertyChanged("Name");OnPropertyChanged("AbstractInfo"); MainViewModel.RefreshTitle(value, this); } }

        public string Memo { get { if (TargetObject != null) return TargetObject.Memo; return null; } set { if (TargetObject != null) TargetObject.Memo = value; OnPropertyChanged("Memo");OnPropertyChanged("AbstractInfo"); } }

    }

}
