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
    /// Interaction logic for FrequencyDisplay.xaml
    /// </summary>
    public partial class FrequencyDisplay : UserControlBase
    {
        public static readonly DependencyProperty FrequencyProperty =
            DependencyProperty.Register("Frequency", typeof(uint), typeof(FrequencyDisplay),
            new PropertyMetadata(default(uint), OnPropertyChanged));

        public static readonly DependencyProperty PrecisionProperty =
            DependencyProperty.Register("Precision", typeof(sbyte), typeof(FrequencyDisplay),
            new PropertyMetadata(default(sbyte), OnPropertyChanged));

        public uint Frequency { get; set; }
        public sbyte Precision { get; set; }

        public FrequencyDisplay()
        {
            InitializeComponent();
        }
    }
}
