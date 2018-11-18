using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using DesignTool.Lib.Model;
using DesignTool.Lib.View.DesignControlSupport;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DesignTool.Lib.View
{
    /// <summary>
    /// Interaction logic for DesignControl.xaml
    /// </summary>
    public partial class DesignControl : UserControl
    {
        public DesignControl()
        {
            InitializeComponent();
            dm=new DesignMove(this);
            LTds = new DesignScale(this, LTScale){IsLeft=true,IsTop=true};
            LBds = new DesignScale(this, LBScale) { IsLeft = true, IsBottom = true };
            RTds = new DesignScale(this, RTScale) { IsRight = true, IsTop = true };
            RBds = new DesignScale(this, RBScale) { IsRight = true, IsBottom = true };

            lc = new DesignConnection(this, LConnectionPoint) { IsLeft=true};
            tc = new DesignConnection(this, TConnectionPoint) { IsTop=true};
            rc = new DesignConnection(this, RConnectionPoint) { IsRight=true};
            bc = new DesignConnection(this, BConnectionPoint) { IsBottom=true};


            //Lds = new DesignScale(this, LConnectionPoint) { IsLeft = true };
            //Bds = new DesignScale(this, BConnectionPoint) { IsBottom = true };
            //Tds = new DesignScale(this, TConnectionPoint) { IsTop = true };
            //Rds = new DesignScale(this, RConnectionPoint) { IsRight = true};
            //dc = new DesignConnection(
            //    this, 
            //    new List<FrameworkElement>()
            //    {
            //        TConnectionPoint, RConnectionPoint, BConnectionPoint, LConnectionPoint
            //    }, 
            //    new List<FrameworkElement>()
            //        {
            //            TConnectionPoint, RConnectionPoint, BConnectionPoint, LConnectionPoint
            //        }
            //        );



            da =new DesignActive(this);

            da.ActiveControlList.Add(LTScale);
            da.ActiveControlList.Add(LBScale);
            da.ActiveControlList.Add(RTScale);
            da.ActiveControlList.Add(RBScale);
            da.ActiveControlList.Add(TConnectionPoint);
            da.ActiveControlList.Add(RConnectionPoint);
            da.ActiveControlList.Add(BConnectionPoint);
            da.ActiveControlList.Add(LConnectionPoint);
            


            dm.IsEnable = true;
            LTds.IsEnable = true;
            RTds.IsEnable = true;
            LBds.IsEnable = true;
            RBds.IsEnable = true;
            lc.IsOutEnable = true;
            tc.IsOutEnable = true;
            rc.IsOutEnable = true;
            bc.IsOutEnable = true;
            lc.IsInEnable = true;
            tc.IsInEnable = true;
            rc.IsInEnable = true;
            bc.IsInEnable = true;
        }
        public void ChangeActive(bool isActive)
        {
            da.IsActive = isActive;
        }

        public List<DesignConnection> GetConnectionList()
        {
            return new List<DesignConnection>() { lc,tc,bc,rc};

        }

        private DesignActive da;
        private DesignConnection lc;
        private DesignConnection tc;
        private DesignConnection rc;
        private DesignConnection bc;
        private DesignScale LTds;
        private DesignScale LBds;
        private DesignScale RTds;
        private DesignScale RBds;
        //private DesignScale Lds;
        //private DesignScale Bds;
        //private DesignScale Tds;
        //private DesignScale Rds;
        private DesignMove dm;

        public DesignItem GetObject()
        {
            if (DataContext != null && DataContext is DesignItem)
                return DataContext as DesignItem;
            return null;
        }


        public bool IsEnableMove
        {
            get { return dm.IsEnable; }
            set { 
                dm.IsEnable = value;
            }
        }

    }

}
