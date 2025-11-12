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
        public string CorrectAnswer 
        {
            get => _playerViewmodel.ActivePack.RandomizedCorrectAnswers[_playerViewmodel.currentIndex]; 
        }
        public PlayerView(PlayerViewModel playerViewmodel)
        {
            _playerViewmodel = playerViewmodel;
            InitializeComponent();
        }
        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            _playerViewmodel.ChosenAnswer = Button0.Content?.ToString();
            Button0.Background = Brushes.DarkRed;
            RevealCorrectAnswer();
            _playerViewmodel.IsAnswerChosen = true;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            _playerViewmodel.ChosenAnswer = Button1.Content?.ToString();
            Button1.Background = Brushes.DarkRed;
            RevealCorrectAnswer();
            _playerViewmodel.IsAnswerChosen = true;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            _playerViewmodel.ChosenAnswer = Button2.Content?.ToString();
            Button2.Background = Brushes.DarkRed;
            RevealCorrectAnswer();
            _playerViewmodel.IsAnswerChosen = true;
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            _playerViewmodel.ChosenAnswer = Button3.Content?.ToString();
            Button3.Background = Brushes.DarkRed;
            RevealCorrectAnswer();
            _playerViewmodel.IsAnswerChosen = true;
        }
        private void RevealCorrectAnswer()
        {
            if (Button0.Content != null && Button0.Content == CorrectAnswer)
            {
                Button0.Background = Brushes.DarkOliveGreen;
            }
            else if (Button1.Content != null && Button1.Content == CorrectAnswer)
            {
                Button1.Background = Brushes.DarkOliveGreen;
            }
            else if (Button2.Content != null && Button2.Content == CorrectAnswer)
            {
                Button2.Background = Brushes.DarkOliveGreen;
            }
            else if (Button3.Content != null && Button3.Content == CorrectAnswer)
            {
                Button3.Background = Brushes.DarkOliveGreen;
            }
        }
    }
}
