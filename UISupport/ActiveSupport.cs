using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UISupport
{
    public interface IUIActiveSupport
    {
        bool IsActiveEnable { get; set; }
        List<Action> SetActive { get; }
        List<Action> SetDeActive { get; }
    }
}
