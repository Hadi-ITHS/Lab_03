using Lab_03.Commands;
using Lab_03.Models;
using Lab_03.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Lab_03.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand OpenPackOptionsCommand { get; }
        public DelegateCommand DeleteQuestionPackCommand { get; }
        public DelegateCommand OpenAddQuestionPackDialogCommand { get; }
        public DelegateCommand SetActivePackCommand { get; }
        public ConfigurationView ConfigurationView { get; }
        public MainWindow MainWindow { get;}
        public PlayerView PlayerView { get; }
        public UserControl ActiveView { get; set; }
        public ObservableCollection<QuestionPackViewModel> packs { get; }
        private QuestionPackViewModel _activePack;
        public PlayerViewModel? PlayerViewModel { get;}
        public ConfigurationViewModel? ConfigurationViewModel { get; }
        public QuestionPackViewModel ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                UpdatePacks();
            }
        }
        public MainWindowViewModel(MainWindow mainWindow)
        {
            packs = new ObservableCollection<QuestionPackViewModel>();
            List<QuestionPack> importedPacks = JsonSerializer.Deserialize<List<QuestionPack>>(File.ReadAllText(("Questions.json")));
            foreach (var pack in importedPacks)
            {
                packs.Add(new QuestionPackViewModel(pack));
            }
            if (packs.Count > 0)
                ActivePack = packs[0];
            MainWindow = mainWindow;
            PlayerView = new PlayerView();
            ConfigurationView = new ConfigurationView();
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            SetActivePackCommand = new DelegateCommand(SetActivePack);
            OpenAddQuestionPackDialogCommand = new DelegateCommand(OpenAddQuestionPackDialog);
            DeleteQuestionPackCommand = new DelegateCommand(DeleteQuestionPack);
            OpenPackOptionsCommand = new DelegateCommand(OpenPackOptions);
            ExitCommand = new DelegateCommand(Exit);
            ActiveView = ConfigurationView;
            //test
            /*string[] a = { "b", "c", "d" };
            string[] A = { "B", "C", "D" };
            string[] g = { "h", "j", "k" };
            string[] G = { "H", "J", "K" };
            var pack01 = new QuestionPack("Medium Pack 01");
            pack01.Questions.Add(new Question("Querry01", "a", a));
            pack01.Questions.Add(new Question("Querry02", "A", A));
            ActivePack = new QuestionPackViewModel(pack01);
            packs.Add(ActivePack);


            var pack02 = new QuestionPack("Hard Pack 02");
            pack02.Questions.Add(new Question("Querry01", "g", g));
            pack02.Questions.Add(new Question("Querry02", "G", G));
            ActivePack = new QuestionPackViewModel(pack02);
            packs.Add(ActivePack);
            string json = JsonSerializer.Serialize(packs);
            File.WriteAllText("Questions.json", json);*/
            //test
        }
        private void UpdatePacks()
        {
            for (int i = 0; i < packs.Count; i++)
            {
                if (packs[i].Equals(ActivePack))
                {
                    packs[i] = ActivePack;
                }
            }
            JsonWrite(packs);
        }
        private void SetActivePack(object? obj)
        {
            if (obj is QuestionPackViewModel selectedPack)
            {
                ActivePack = selectedPack;
            }
        }
        private void OpenAddQuestionPackDialog (object? obj)
        {
            var addQuestionPackDialog = new AddQuestionPackDialog(this);
            addQuestionPackDialog.ShowDialog();
        }
        private void DeleteQuestionPack (object? obj)
        {
            MessageBoxResult result=MessageBox.Show("Are you sure you want to delete this question pack?", "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            if (result == MessageBoxResult.Yes)
            {
                packs.Remove(ActivePack);
            }
        }
        private void OpenPackOptions (object? obj)
        {
            var packOptionsDialog = new PackOptionsDialog(this);
            packOptionsDialog.ShowDialog();
        }
        /*private void HandleActiveView (UserControl view)
        {
            MainWindow.
        }*/
        private void ShowPlayerView (object? obj)
        {
            ActiveView = PlayerView;
            //HandleActiveView(ActiveView);
        }
        private void ShowConfigurationView(object? obj)
        {
            ActiveView = ConfigurationView;
            //HandleActiveView(ActiveView);
        }
        private void Exit (object? obj)
        {
            UpdatePacks();
        }
    }
}
