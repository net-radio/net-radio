using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControlLibrary
{
    /// <summary>
    /// Interaction logic for HoppingControl.xaml
    /// </summary>
    public partial class HoppingControl : UserControlBase
    {
        #region CommandStart
        public static readonly DependencyProperty CommandStartProperty =
            DependencyProperty.Register("CommandStart", typeof(ICommand), typeof(HoppingControl),
            new UIPropertyMetadata(default(ICommand), OnPropertyChanged));        
        public ICommand CommandStop { get; set; }
        #endregion

        #region CommandStop
        public static readonly DependencyProperty CommandStopProperty =
            DependencyProperty.Register("CommandStop", typeof(ICommand), typeof(HoppingControl),
            new UIPropertyMetadata(default(ICommand), OnPropertyChanged));        
        public ICommand CommandStart { get; set; }
        #endregion

        #region FrequencyStart
        public static readonly DependencyProperty FrequencyStartProperty =
            DependencyProperty.Register("FrequencyStart", typeof(uint), typeof(HoppingControl),
                new UIPropertyMetadata(default(uint), OnPropertyChanged));
        public uint FrequencyStart { get; set; }
        #endregion

        #region FrequencyStop
        public static readonly DependencyProperty FrequencyStopProperty =
            DependencyProperty.Register("FrequencyStop", typeof(uint), typeof(HoppingControl),
                new UIPropertyMetadata(default(uint), OnPropertyChanged));
        public uint FrequencyStop { get; set; }
        #endregion

        #region ThresholdMinimum
        public static readonly DependencyProperty ThresholdMinimumProperty =
            DependencyProperty.Register("ThresholdMinimum", typeof(float), typeof(HoppingControl),
                new UIPropertyMetadata(default(float), OnPropertyChanged));
        public float ThresholdMinimum { get; set; }
        #endregion

        #region ThresholdMaximum
        public static readonly DependencyProperty ThresholdMaximumProperty =
            DependencyProperty.Register("ThresholdMaximum", typeof(float), typeof(HoppingControl),
                new UIPropertyMetadata(default(float), OnPropertyChanged));
        public float ThresholdMaximum { get; set; }
        #endregion

        #region Gain
        public static readonly DependencyProperty GainProperty =
            DependencyProperty.Register("Gain", typeof(float), typeof(HoppingControl),
                new UIPropertyMetadata(default(float), OnPropertyChanged));
        public float Gain { get; set; }
        #endregion


        #region Ctor
        public HoppingControl()
        {
            InitializeComponent();
        }
        #endregion
    }
}
