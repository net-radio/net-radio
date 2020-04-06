using System;
using System.Windows;
using System.Windows.Controls;
using NetRadio.G313.Model;

namespace NetRadio.G313
{
    /// <summary>
    /// Interaction logic for DebugWindow.xaml
    /// </summary>
    public partial class DebugWindow
    {
        private RadioModel _model;

        private DebugModel.DebugCommand _selectedCommand;

        public DebugWindow()
        {
            InitializeComponent();
        }

        internal DebugWindow(RadioModel model)
            : this()
        {
            _model = model;

            var commands=_model.Debug.GetCommands();
            foreach (var command in commands)
                CommandList.Items.Add(command);

        }

        private void ExecuteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedCommand == null)
                return;
            try
            {
                if (_selectedCommand.SendMode)
                {
                    var type = _selectedCommand.Parameter.GetType();
                    if (type.IsValueType)
                        _selectedCommand.Parameter = type.IsEnum
                            ? Enum.Parse(type, Value.Text)
                            : Convert.ChangeType(Value.Text, type);
                    else
                        _selectedCommand.Parameter = PropertyGrid.SelectedObject;
                    _selectedCommand.Execute();
                }
                else
                {
                    _selectedCommand.Execute();
                    PropertyGrid.SelectedObject = _selectedCommand.Parameter;
                    Value.Text = _selectedCommand.Parameter.ToString();
                }

                System.Windows.Forms.MessageBox.Show("Succeed.");
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("Failed.");
            }

        }

        private void CommandList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedCommand = CommandList.SelectedItem as DebugModel.DebugCommand;
            if (_selectedCommand == null)
                return;

            _selectedCommand.Prepare();
            if (_selectedCommand.SendMode)
            {
                PropertyGrid.SelectedObject = _selectedCommand.Parameter;
                Value.Text = _selectedCommand.Parameter.ToString();
            }
        }

    }
}
