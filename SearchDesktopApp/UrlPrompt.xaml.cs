using System.Windows;

namespace SearchDesktopApp
{
    public partial class UrlPrompt : Window
    {
        public UrlPrompt()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var searchWindow = new SearchWindow(UrlTextBox.Text);
            Close();
            searchWindow.Show();
        }
    }
}