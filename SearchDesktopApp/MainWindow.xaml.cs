using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using SearchDesktopApp.Models;

namespace SearchDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<PropertyModel> Properties = new List<PropertyModel>();

        private readonly ApiClient _apiClient;

        public MainWindow()
        {
            InitializeComponent();
            _apiClient = new ApiClient("https://localhost:7299");
            PropertiesComboBox.ItemsSource = Properties;
        }

        private async void PropertiesComboBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchPhrase = PropertiesComboBox.Text;
            if (string.IsNullOrWhiteSpace(searchPhrase))
            {
                //PropertiesComboBox.IsDropDownOpen = false;
                return;
            }
            
            var result = await _apiClient.SearchProperties(searchPhrase, MarketTextBox.Text);
            if (result == null || result.Count < 1)
            {
                Properties.Clear();
                PropertiesComboBox.Items.Refresh();
                //PropertiesComboBox.IsDropDownOpen = false;
                return;
            }
            
            Properties.Clear();
            Properties.AddRange(result);
            PropertiesComboBox.Items.Refresh();
            //PropertiesComboBox.IsDropDownOpen = true;
        }
    }
}