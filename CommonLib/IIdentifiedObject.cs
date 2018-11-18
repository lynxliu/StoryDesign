using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public interface IIdentifiedObject
    {
        Guid ObjectID { get; set; }
    }
}
