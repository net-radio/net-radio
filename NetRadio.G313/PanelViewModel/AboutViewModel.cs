using System;
using System.Windows.Input;
using NetRadio.G313.Model;
using NetRadio.G313.ViewModel;

namespace NetRadio.G313.PanelViewModel
{
    class AboutViewModel:ViewModelBase
    {
        private readonly RadioModel _model;

        public string Name
        {
            get { return _model.Radio.CachedInfo.Name; }
        }

        public string Serial
        {
            get { return _model.Radio.CachedInfo.Serial; }
        }

        public ulong MinFrequency
        {
            get { return _model.Radio.CachedInfo.MinFrequency; }
        }

        public ulong MaxFrequency
        {
            get { return _model.Radio.CachedInfo.MaxFrequency; }
        }

        public Action WindowCloseAction { get; set; }

        public ICommand CommandClose { get; private set; }

        public AboutViewModel(RadioModel model)
        {
            _model = model;

            CommandClose=new DelegateCommand(()=>WindowCloseAction());
        }
    }
}
