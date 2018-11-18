using Newtonsoft.Json;
using StoryDesignLib;
using StoryDesignLib.Diagram;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UISupport;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoryDesign.View.Control
{
    public sealed partial class CreateStoryDialog : ContentDialog
    {
        public CreateStoryDialog()
        {
            this.InitializeComponent();
            DataContext= new CreateStoryDialogViewModel();
        }
        public void Init(StoryDesign.Model.StoryDesign design)
        {
            var dc = DataContext as  CreateStoryDialogViewModel;
            dc.Name=design.TargetStory.Name;
            dc.Memo = design.TargetStory.Memo;
            dc.Author = design.TargetStory.Author;
            dc.BeginTime = design.TargetStory.BeginTime;
            dc.EndTime = design.TargetStory.EndTime;
        }

        public async System.Threading.Tasks.Task<bool> CreateProject(StoryDesign.Model.StoryDesign design)
        {
            var dc = DataContext as CreateStoryDialogViewModel;
            if (dc.BaseFolder == null)
            {
                CommonLib.CommonProc.ShowMessage("Error", "please select project folder");
                return false;

            }
            design.ProjectFolder = await dc.BaseFolder.CreateFolderAsync(dc.Name);
            var file = await design.ProjectFolder.CreateFileAsync(dc.Name + ".story");
            if (file != null)
            {
                design.TargetStory = new Story() { Name = dc.Name, Memo = dc.Memo, Author = dc.Author, BeginTime = dc.BeginTime, EndTime = dc.EndTime };
                design.TargetStory.StructureDiagramList.Add(new StructureDiagram());
                design.CurrentWorkViewList.Clear();
                //design.CurrentWorkView = null;

            }
            return true;
        }
    }

    public class CreateStoryDialogViewModel:ViewModelBase
    {
        public string Name
        {
            get;set;
        }

        public string Memo
        {
            get; set;
        }

        public string Author
        {
            get; set;
        }

        public DateTime BeginTime
        {
            get; set;
        }

        public DateTime EndTime
        {
            get; set;
        }

        public CommonCommand SelectBaseFolderCommand
        {
            get
            {
                return new CommonCommand(async (o) =>
                {
                    FolderPicker pick = new FolderPicker();
                    pick.FileTypeFilter.Add(".story");
                    pick.SuggestedStartLocation = 0;
                    Windows.Storage.StorageFolder folder = await pick.PickSingleFolderAsync();
                    if (folder != null)
                    {
                        try
                        {
                            var f = await folder.GetItemAsync(Name);
                            CommonLib.CommonProc.ShowMessage("Error", "Story folder named "+Name+" already exist, please select another folder or change story name.");
                            return;
                        }
                        catch (FileNotFoundException)
                        {
                            //right
                        }


                        BaseFolderPath = folder.Path;
                        OnPropertyChanged("BaseFolderPath");
                        BaseFolder = folder;
                    }
                });
            }
        }

        public StorageFolder BaseFolder { get; set; }

        public string BaseFolderPath
        {
            get;set;
        }
    }
}
