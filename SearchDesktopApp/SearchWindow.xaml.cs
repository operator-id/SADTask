using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SearchDesktopApp.Models;

namespace SearchDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public List<RealEstateBase> Properties = new List<RealEstateBase>();

        private readonly ApiClient _apiClient;

        public SearchWindow(string url)
        {
            InitializeComponent();
            _apiClient = new ApiClient(url);
            PropertiesComboBox.ItemsSource = Properties;
        }

        private async void PropertiesComboBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchPhrase = PropertiesComboBox.Text;
            if (!PropertiesComboBox.IsDropDownOpen)
            {
                PropertiesComboBox.IsDropDownOpen = true;
            }

            if (string.IsNullOrWhiteSpace(searchPhrase))
            {
                return;
            }
            
            var result = await _apiClient.Search(searchPhrase, MarketTextBox.Text);
            
            if (result == null || result.Count < 1)
            {
                return;
            }
            
            Properties.Clear();
            Properties.AddRange(result);
            PropertiesComboBox.Items.Refresh();
        }
    }
}