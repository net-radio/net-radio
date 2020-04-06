using NetRadio.G31Ddc.PanelViewModel;
using NetRadio.G31Ddc.View;
using NetRadio.G31Ddc.ViewModel.Exciter;
using NetRadio.G31Ddc.ViewModel.UserManagement;
using System;
using System.Windows.Controls;

namespace NetRadio.G31Ddc.ViewModel
{
    public class MasterViewModel:ViewModelBase, IDisposable
    {
        private ExciterViewModel exciterView_;
        // private PaxViewModel paxView_;
        private UserViewModel _userView;

        [Obsolete("Remove model parameter", false)]
        public MasterViewModel(RadioModel model, ViewObject vo)
        {
            exciterView_ = new ExciterViewModel();
            // paxView_ = new PaxViewModel();
            // _userView = new UserViewModel(radioView_, exciterView_);
        }

        public UserViewModel UserView
        {
            get { return _userView; }
        }

        public ExciterViewModel ExciterView
        {
            get { return exciterView_; } 
        }

        public void Dispose()
        {
            ExciterView.Dispose();
        }
    }
}
