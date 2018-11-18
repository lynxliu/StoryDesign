using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Media;

namespace UISupport
{
    public class CommonCommand : ICommand
    {
        public string Name { get; set; }
        public string Memo { get; set; }
        public SolidColorBrush BackgroundBrush { get; set; }
        public SolidColorBrush ForegroundBrush { get; set; }
        public CommonCommand(Action<object> action)
        {
            TargetAction = action;
        }
        public CommonCommand(Action<object> action, Predicate<object> predicate)
        {
            TargetAction = action;
            TargetPredicate = predicate;
        }
        public Action<object> TargetAction { get; set; }
        public Predicate<object> TargetPredicate { get; set; }
        bool _CanExecute = true;
        
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if (TargetPredicate != null)
            {
                var r= TargetPredicate(parameter);
                if (r != _CanExecute && CanExecuteChanged != null)
                {
                    _CanExecute = r;
                    CanExecuteChanged(this, new EventArgs());
                }
                return _CanExecute;
            }
            return true;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
                TargetAction(parameter);
        }
    }

}
