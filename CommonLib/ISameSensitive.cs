using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public interface ISameSensitive
    {
        bool IsSame(object targetObject);
    }
}
