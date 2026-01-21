using Lab_03.Database;
using Lab_03.Models;
using Lab_03.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Lab_03.Views
{
    /// <summary>
    /// Interaction logic for PackOptionsDialog.xaml
    /// </summary>
    public partial class PackOptionsDialog : Window
    {
        public MainWindowViewModel? _mainWindowViewModel { get; set; }
        public PackOptionsDialog(MainWindowViewModel? mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            InitializeComponent();
            DataContext = _mainWindowViewModel;
            Closing += (s, e) => OnClosing(e);
        }
        private async void OnClosing(CancelEventArgs e)
        {
            await _mainWindowViewModel.MongoDbManager.ReplaceQuestionPackAsync(_mainWindowViewModel.ActivePack);
        }
    }
}
