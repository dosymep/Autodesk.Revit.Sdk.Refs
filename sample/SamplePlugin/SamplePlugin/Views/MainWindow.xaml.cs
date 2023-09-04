using System.Windows;

namespace SamplePlugin.Views {
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }

        public string Greeting {
            get => _textBox.Text;
            set => _textBox.Text = value;
        }

        private void ButtonOk_OnClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
        }

        private void ButtonCancel_OnClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
        }
    }
}