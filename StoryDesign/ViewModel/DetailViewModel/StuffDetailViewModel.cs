using StoryDesignInterface;
using StoryDesignLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesign.ViewModel.DetailViewModel
{
    [AttributeIcon(DefaultIconUri = "ms-appx:///Assets/Icon/Stuff.png")]
    public class StuffDetailViewModel : DetailViewModelBase<IStuff>
    {
        public double Number
        {
            get { return TargetObject.Number; }
            set { TargetObject.Number = value; OnPropertyChanged("Number"); }
        }

        public double Value
        {
            get { return TargetObject.Value; }
            set { TargetObject.Value = value; OnPropertyChanged("Value"); }
        }

        public IStuffFunction CurrentFunction { get; set; }
        ObservableCollection<IStuffFunction> _FunctionList = new ObservableCollection<IStuffFunction>();
        public ObservableCollection<IStuffFunction> FunctionList { get { return _FunctionList; } }

        public void AddFunction()
        {
            CurrentFunction = new StuffFunction() { Memo = "function", Efficiency = 1 };
            FunctionList.Add(CurrentFunction);
            OnPropertyChanged("CurrentFunction");
        }
        public void RemoveCurrentFunction()
        {
            if (CurrentFunction != null)
            {
                FunctionList.Remove(CurrentFunction);
                CurrentFunction = null;
                OnPropertyChanged("CurrentFunction");
            }
        }
        public override void LoadInfo()
        {
            base.LoadInfo();

            FunctionList.Clear();
            TargetObject.FunctionList.ForEach(v => FunctionList.Add(v));
        }

        public override void SaveInfo()
        {
            base.SaveInfo();
            TargetObject.FunctionList.Clear();
            FunctionList.ToList().ForEach(v => TargetObject.FunctionList.Add(v));
        }
    }
}
