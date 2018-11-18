using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface INoteObject
    {
        bool HaveNote { get; }
        List<INote> NoteList { get; }
    }
    public interface INote: ICopySupportObject
    {
        DateTime CreateTime { get; }
        DateTime LastModified { get; set; }
        string Description { get; set; }

    }
}
