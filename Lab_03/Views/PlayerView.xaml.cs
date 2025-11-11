using Lab_03.ViewModels;
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

namespace Lab_03.Views
{
    /// <summary>
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        private PlayerViewModel _playerViewmodel;
        public PlayerView(PlayerViewModel playerViewmodel)
        {
            _playerViewmodel = playerViewmodel;
            InitializeComponent();
        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            _playerViewmodel.ChosenAnswer = Button0.Content?.ToString();
            _playerViewmodel.NextQuestion();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            _playerViewmodel.ChosenAnswer = Button1.Content?.ToString();
            _playerViewmodel.NextQuestion();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            _playerViewmodel.ChosenAnswer = Button2.Content?.ToString();
            _playerViewmodel.NextQuestion();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            _playerViewmodel.ChosenAnswer = Button3.Content?.ToString();
            _playerViewmodel.NextQuestion();
        }
    }
}
