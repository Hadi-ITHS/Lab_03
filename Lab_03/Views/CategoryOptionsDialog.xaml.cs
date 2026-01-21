using Lab_03.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab_03.Views
{
    public partial class CategoryOptionsDialog : Window
    {
        private MainWindowViewModel MainWindowViewModel { get; set; }
        public CategoryOptionsDialog(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            //Categories = mainWindowViewModel.Categories;
            InitializeComponent();
            DataContext = MainWindowViewModel;
        }

        private void AddToCategories_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.MongoDbManager.InsertCategory(CategoryTextBox.Text);
            MainWindowViewModel.Categories = MainWindowViewModel.MongoDbManager.LoadCategories();
            CategoryTextBox.Clear();
        }

        private void RemoveFromCategories_Click(object sender, RoutedEventArgs e)
        {
            MainWindowViewModel.MongoDbManager.RemoveCategory(CategoryComboBox.SelectedItem.ToString());
            MainWindowViewModel.Categories = MainWindowViewModel.MongoDbManager.LoadCategories();
            if (CategoryComboBox.Items.Count > 0)
                CategoryComboBox.SelectedIndex = 0;
        }
    }
}
