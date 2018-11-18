using CommonLib;
using StoryDesign.View.DesignView;
using StoryDesign.ViewModel.DesignViewModel;
using StoryDesignInterface;
using StoryDesignInterface.Diagram;
using StoryDesignLib.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UISupport;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
using StoryDesignLib;

namespace StoryDesign.ViewModel.DetailViewModel
{
    [AttributeIcon(DefaultIconUri = "ms-appx:///Assets/Icon/default.png") ]
    public class DetailViewModelBase<T>: DataEntityViewModelBase<T>,IconSupport where T : IStoryEntityObject
    {
        public double Grade
        {
            get { return TargetObject.Grade; }
            set { TargetObject.Grade = value; OnPropertyChanged("Grade"); OnPropertyChanged("ActorGrade"); }
        }
        public void UpdateIcon()
        {
            CheckIcon();
            OnPropertyChanged("IconImage");
        }
        public async void CheckIcon()
        {
            
            var folder =await MainViewModel.mainViewModel.GetIconFolder();
            if (folder != null)
            {
                var fl =await folder.GetFilesAsync();
                foreach(var f in fl)
                {
                    if (f.Name.Contains(TargetObject.ObjectID.ToString()))
                    {
                        _IconImage = await UIManager.LoadImageFromFile(f);
                        OnPropertyChanged("IconImage");
                        return ;
                    }
                }
            }
            //_IconImage = new BitmapImage(Icon);

            //OnPropertyChanged("IconImage");
        }
        BitmapImage _IconImage;
        public BitmapImage IconImage
        {
             get 
            {
                if (_IconImage == null)
                {
                    _IconImage = new BitmapImage(Icon);
                    CheckIcon();
                }
                //
                return _IconImage;
            }
        }

        protected Uri _Icon;
        public virtual Uri Icon
        {
            get
            {
                if (string.IsNullOrEmpty(TargetObject.Icon)) {
                    var attrl = GetType().GetTypeInfo().GetCustomAttributes(typeof(AttributeIcon),true);
                    if (attrl != null && attrl.Count() > 0)
                    {
                        var a = attrl.FirstOrDefault() as AttributeIcon;
                        if (a != null)
                            _Icon = new Uri(a.DefaultIconUri);
                    }
                }
                else
                    _Icon = new Uri(TargetObject.Icon);
                return _Icon;
            }
            set
            {
                _Icon = value;
                TargetObject.Icon = value.AbsoluteUri;
                OnPropertyChanged("Icon");
            }
        }
        public GridView ResourcePanel { get; set; }

        public Func<FrameworkElement> GetDetailView { get; set; }
        //bool _IsEditMode = false;
        //public bool IsEditMode
        //{
        //    get { return _IsEditMode; }
        //    set { _IsEditMode = true;OnPropertyChanged("IsEditMode"); OnPropertyChanged("IsReadOnly"); }
        //}
        //public bool IsReadOnly
        //{
        //    get { return !_IsEditMode; }
            
        //}
        //public CommonCommand SwitchEditModeCommand
        //{
        //    get
        //    {
        //        return new CommonCommand((o) =>
        //        {
        //            IsEditMode = !IsEditMode;
        //        });
        //    }
        //}
        public CommonCommand AddResourceCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    Add();
                });
            }
        }
        public CommonCommand PasteResourceCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    Paste();
                });
            }
        }
        public CommonCommand DetailCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    //MainPageViewModel.OpenStoryObject(TargetObject);
                    if (GetDetailView != null)
                    {
                        var view = GetDetailView();
                        CommonProc.ShowMessage(() => { }, "Edit", view);
                    }
                });
            }
        }
        public CommonCommand OpenFateDiagramCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    

                    TargetObject.RefreshFateDiagram();
                    var dc = new FateDiagramViewModel() { TargetObject = TargetObject.TargetFate };
                    var view = new FateDiagramView() { DataContext = dc};
                    dc.DrawDiagram();
                    MainViewModel.OpenView(view, TargetObject.Name+" Fate Diagram", Model.ViewDataType.Fate,null,TargetObject.ObjectID);
                });
            }
        }
        //public DateTime BeginDate
        //{
        //    get { if (TargetObject != null) return TargetObject.BeginTime.Date; return DateTime.Now.Date; }
        //    set { if (TargetObject != null) TargetObject.BeginTime =new DateTime(value.Year,value.Month,value.Day); OnPropertyChanged("BeginDate"); }
        //}

        //public DateTime EndDate { get { if (TargetObject != null) return TargetObject.EndTime.Date; return DateTime.Now.Date; }
        //    set { if (TargetObject != null) TargetObject.EndTime = new DateTime(value.Year, value.Month, value.Day); OnPropertyChanged("EndDate"); } }

        public DateTime BeginTime {
            get { if (TargetObject != null) return TargetObject.BeginTime; return DateTime.Now; }
            set { if (TargetObject != null) TargetObject.BeginTime = value;
                OnPropertyChanged("BeginTime");OnPropertyChanged("AbstractInfo"); } }

        public DateTime EndTime { get { if (TargetObject != null) return  TargetObject.EndTime; return DateTime.Now; }
            set { if (TargetObject != null) TargetObject.EndTime = value; OnPropertyChanged("EndTime"); OnPropertyChanged("AbstractInfo"); } }

        public DateTime CurrentTime { get { if (TargetObject != null) return TargetObject.CurrentTime; return DateTime.Now; } }

        public Guid ObjectID { get { if (TargetObject != null) return TargetObject.ObjectID;return Guid.Empty; } set { if (TargetObject != null) TargetObject.ObjectID = value;OnPropertyChanged("ObjectID"); } }

        public TimeSpan ContinueTime { get { if (TargetObject != null) return TargetObject.ContinueTime;return TimeSpan.FromSeconds(0); } }

        ObservableCollection<IMeasure> _MeasureList = new ObservableCollection<IMeasure>();
        public ObservableCollection<IMeasure> MeasureList { get { return _MeasureList; } }

        #region NoteCommand
        public bool HaveNote { get { if (NoteList.Count > 0) return true; return false; } }

        ObservableCollection<NoteViewModel> _NoteList = new ObservableCollection<NoteViewModel>();
        public ObservableCollection<NoteViewModel> NoteList { get { return _NoteList; } }
        public NoteViewModel CurrentNote { get; set; }

        public CommonCommand AddNoteCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    CurrentNote = new NoteViewModel() { TargetObject = new Note() };
                    NoteList.Add(CurrentNote);
                    SaveNoteList();
                });
            }
        }
        public CommonCommand RemoveNoteCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    if (CurrentNote != null)
                    {
                        NoteList.Remove(CurrentNote);
                    }
                    SaveNoteList();
                });
            }
        }
        void SaveNoteList()
        {
            TargetObject.NoteList.Clear();
            foreach (var note in NoteList)
            {
                TargetObject.NoteList.Add(note.TargetObject);
            }
            CurrentNote = NoteList.FirstOrDefault();
        }
        void LoadNoteList()
        {
            if (TargetObject == null) return;
            NoteList.Clear();
            foreach (var note in TargetObject.NoteList)
            {
                NoteList.Add(new NoteViewModel() { TargetObject = note });
            }
            CurrentNote = NoteList.FirstOrDefault();
        }
        #endregion

        public virtual string AbstractInfo
        {
            get
            {
                string s = "";
                if (!string.IsNullOrEmpty(Name))
                    s += Name + "( from " + BeginTime.ToString() + " to " + EndTime.ToString() + ")";

                if (!string.IsNullOrEmpty(Memo))
                    if (string.IsNullOrEmpty(s))
                        s = Memo;
                    else
                        s += ":" + Memo;

                return s;
            }
        }

        //ObservableCollection<IStructureDiagram> _StructureDiagramList = new ObservableCollection<IStructureDiagram>();
        //public ObservableCollection<IStructureDiagram> StructureDiagramList { get { return _StructureDiagramList; } }

        //public IFateDiagram FateDiagram { get { return TargetObject.TargetFate;  }set { TargetObject.TargetFate = value; OnPropertyChanged("FateDiagram"); } }

        //ObservableCollection <Resource> _ResourceList = new ObservableCollection<Resource>();
        //public ObservableCollection<Resource> ResourceList { get { return _ResourceList; } }

        public override void LoadInfo()
        {
            MeasureList.Clear();
            TargetObject.MeasureList.ForEach(v => MeasureList.Add(v));

            LoadNoteList();


            //StructureDiagramList.Clear();
            //TargetObject.StructureDiagramList.ForEach(v => StructureDiagramList.Add(v));

            RefreshAllProperty();
        }

        public override void SaveInfo()
        {
            TargetObject.MeasureList.Clear();
            MeasureList.ToList().ForEach(v => TargetObject.MeasureList.Add(v));

            SaveNoteList();

            //TargetObject.StructureDiagramList.Clear();
            //StructureDiagramList.ToList().ForEach(v => TargetObject.StructureDiagramList.Add(v));

            IsChanged = false;
            var vm = MainViewModel.mainViewModel;
            if (vm != null)
                vm.RefreshAllView(this);
        }
        async void FileDefaultLaunch(StorageFile file)
        {
            // Path to the file in the app package to launch
            //string imageFile = @"images\test.png";

            //var file = await folder.GetFileAsync(filepath);

            if (file != null)
            {
                // Launch the retrieved file
                var success = await Windows.System.Launcher.LaunchFileAsync(file);

                if (success)
                {
                    // File launched
                }
                else
                {
                    var options = new Windows.System.LauncherOptions();
                    options.DisplayApplicationPicker = true;
                    bool optionsuccess = await Windows.System.Launcher.LaunchFileAsync(file, options);
                    if (optionsuccess)
                    {
                        // File launched
                    }
                    else
                    {
                        // File launch failed
                    }

                }
            }
            else
            {
                // Could not find file
            }
        }
        async Task<StorageFolder> GetResourceFolder()
        {
            var name = TargetObject.Name + "(" + TargetObject.ObjectID.ToString() + ")";
            var baseFolder =await MainViewModel.mainViewModel.GetAssertFolder();
            if(baseFolder==null)
            {
                CommonProc.ShowMessage("Error", "No resource folder" );
                return null;
            }
            StorageFolder f;
            try
            {
                f=await baseFolder.GetFolderAsync(name);
            }
            catch (FileNotFoundException)
            {
                f=await baseFolder.CreateFolderAsync(name);
            }
            return f;
        }

        async void AddItem(StorageFile f)
        {
            var b = new Button() { Width = 120, Height = 85 ,Margin=new Thickness(5)};
            b.Tag = f;
            b.Click += (s, o) =>
            {
                FileDefaultLaunch(f);
            };
            b.RightTapped += (s, e) =>
            {
                if (contextFlyout == null)
                    InitContextMemu(f);
                contextFlyout.ShowAt(s as UIElement, e.GetPosition(s as UIElement));
            };
            var thumbnail = await f.GetScaledImageAsThumbnailAsync(ThumbnailMode.DocumentsView);
            BitmapImage bitmapImage = new BitmapImage();
            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
            await RandomAccessStream.CopyAsync(thumbnail, randomAccessStream);
            randomAccessStream.Seek(0);
            bitmapImage.SetSource(randomAccessStream);
            var im = new Image() { Source = bitmapImage };
            Grid g = new Grid();
            g.RowDefinitions.Add(new RowDefinition());
            g.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
            g.Children.Add(im);
            TextBlock t = new TextBlock() { Text = f.Name };
            Grid.SetRow(t, 1);
            g.Children.Add(t);
            b.Content = g;
            ResourcePanel.Items.Add(b);
        }
        public async void LoadResourceAsync()
        {
            ResourcePanel.Items.Clear();
            var folder = await GetResourceFolder();
            if (folder != null)
            {
                var fl = await folder.GetFilesAsync();
                foreach (var f in fl)
                {
                    AddItem(f);
                }

            }
        }

        async void CopyFiltToResourceFolder(StorageFile f)
        {
            var folder =await GetResourceFolder();
            if (folder != null)
            {
                try
                {
                    var item = await folder.GetItemAsync(f.Name);
                    CommonProc.ShowMessage("Error", "File named " + f.Name + " have already exist in folder");
                    return;
                }
                catch (FileNotFoundException)
                {

                }
                await f.CopyAsync(folder);
            }
            else
                try
                {
                    CommonProc.ShowMessage("Error", "no valid folder for copy");
                }
                catch(Exception e)
                {
                    LogSupport.Error(e);
                }
        }
        void Copy(StorageFile f)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetStorageItems(new List<StorageFile>() { f });
            Clipboard.SetContent(dataPackage);
        }
        async void Paste()
        {
            DataPackageView dataPackageView = Clipboard.GetContent();
            if (dataPackageView.Contains(StandardDataFormats.StorageItems))
            {
                var fl = await dataPackageView.GetStorageItemsAsync();
                var f = fl.FirstOrDefault();
                CopyFiltToResourceFolder(f as StorageFile);
                AddItem(f as StorageFile);
            }
        }
        void Open(StorageFile f)
        {
            if (f != null && f is StorageFile)
                FileDefaultLaunch(f as StorageFile);
        }
        async void Add()
        {
            var pick = new FileOpenPicker();
            pick.FileTypeFilter.Add("*");
            var f = await pick.PickSingleFileAsync();
            if (f != null)
            {
                 CopyFiltToResourceFolder(f);
                AddItem(f);
            }
        }
        async void Remove(StorageFile f)
        {
            if (f != null && f is StorageFile)
                await(f as StorageFile).DeleteAsync();
        }
        MenuFlyout contextFlyout;
        public void InitContextMemu(StorageFile f)
        {
            contextFlyout = new MenuFlyout();
            MenuFlyoutItem copyItem = new MenuFlyoutItem
            {
                Text = "Copy",Tag=f
            };
            copyItem.Click +=  (s, e) =>
            {
                Copy(f);
            };
            contextFlyout.Items.Add(copyItem);

            MenuFlyoutItem pasteItem = new MenuFlyoutItem
            {
                Text = "Paste",
            };
            pasteItem.Click += (s, e) =>
            {
                Paste();


            };
            contextFlyout.Items.Add(pasteItem);
            contextFlyout.Items.Add(new MenuFlyoutSeparator());

            MenuFlyoutItem addItem = new MenuFlyoutItem
            {
                Text = "Add",
            };
            addItem.Click += (s, e) =>
            {
                Add();
            };
            contextFlyout.Items.Add(addItem);

            MenuFlyoutItem openItem = new MenuFlyoutItem
            {
                Text = "Open",
                Tag = f
            };
            openItem.Click += (s, e) =>
            {
                Open(f);

            };
            contextFlyout.Items.Add(openItem);

            MenuFlyoutItem removeItem = new MenuFlyoutItem
            {
                Text = "Remove",
                Tag = f
            };
            removeItem.Click += (s, e) =>
            {
                Remove(f);
            };
            contextFlyout.Items.Add(removeItem);

            //contextFlyout.ShowAt(targetControl, new Point(x,y));

        }

    }

    public class AttributeIcon : Attribute
    {
        public string DefaultIconUri { get; set; }
    }
    public interface IconSupport
    {
         void UpdateIcon();
    }
}
