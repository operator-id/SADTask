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
            if (string.IsNullOrWhiteSpace(searchPhrase))
            {
                PropertiesComboBox.IsDropDownOpen = false;
                return;
            }
            
            var result = await _apiClient.Search(searchPhrase, MarketTextBox.Text);
            if (result == null || result.Count < 1)
            {
                Properties.Clear();
                PropertiesComboBox.Items.Refresh();
                PropertiesComboBox.IsDropDownOpen = false;
                return;
            }
            
            Properties.Clear();
            Properties.AddRange(result);
            PropertiesComboBox.Items.Refresh();
            PropertiesComboBox.IsDropDownOpen = true;
            
        }
    }
}