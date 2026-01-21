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
    public partial class AddQuestionPackDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private MainWindowViewModel MainWindowViewModel {  get;}
        public List<string> Categories { get; set; }
        public string SelectedCategory { get; set; }
        public string Name
        {
            get => PackNameTextBox.Text;
            set => PackNameTextBox.Text = value;
        }
        public Difficulty Difficulty
        {
            get
            {
                if (DifficultyComboBox.SelectedIndex == 0)
                    return Difficulty.Easy;
                else if (DifficultyComboBox.SelectedIndex == 1)
                    return Difficulty.Medium;
                else if (DifficultyComboBox.SelectedIndex == 2)
                    return Difficulty.Hard;
                else
                    return Difficulty.Medium;
            }
            set
            {
                DifficultyComboBox.SelectedItem = value.ToString();
            }
        }
        public int TimeLimit
        {
            get => (int) TimeLimitSlider.Value;
            set
            {
                TimeLimitSlider.Value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeLimit"));
            }
        }
        public AddQuestionPackDialog(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            Categories = mainWindowViewModel.Categories;
            InitializeComponent();
            DataContext = this;
        }
        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var pack = new QuestionPackViewModel(new QuestionPack(Name, TimeLimit, Difficulty, SelectedCategory));
            await MainWindowViewModel.MongoDbManager.InsertQuestionPackAsync(pack);
            MainWindowViewModel.packs.Add(pack);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
