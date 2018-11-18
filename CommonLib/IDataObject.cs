using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public interface IDataObject
    {
        string Name { get; set; }
        string Memo { get; set; }
        //string AbstractInfo { get; }
    }

    //public interface IDescriptionObject
    //{
    //    //string Memo { get; set; }
    //    List<string> KeyWordList { get; }//描述性信息
    //}
}
