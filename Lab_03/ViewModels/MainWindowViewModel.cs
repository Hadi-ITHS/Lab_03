using Lab_03.Commands;
using Lab_03.Models;
using Lab_03.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public DelegateCommand FullscreenCommand { get; }
        public DelegateCommand ShowPlayerViewCommand { get; }
        public DelegateCommand ShowConfigurationViewCommand { get; }
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand DeleteQuestionPackCommand { get; }
        public DelegateCommand OpenAddQuestionPackDialogCommand { get; }
        public DelegateCommand SetActivePackCommand { get; }
        public MainWindow MainWindow { get; set; }
        public PlayerView PlayerView { get; set; }
        public GameOverView GameOverView { get; set; }
        public ConfigurationView ConfigurationView { get; set; }
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
                ConfigurationViewModel?.RaisePropertyChanged(nameof(ConfigurationViewModel.ActivePack));
                PlayerViewModel?.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
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
            ConfigurationView = new ConfigurationView();
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            SetActivePackCommand = new DelegateCommand(SetActivePack);
            OpenAddQuestionPackDialogCommand = new DelegateCommand(OpenAddQuestionPackDialog);
            DeleteQuestionPackCommand = new DelegateCommand(DeleteQuestionPack);
            ExitCommand = new DelegateCommand(Exit);
            ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView);
            ShowPlayerViewCommand = new DelegateCommand(ShowPlayerView);
            FullscreenCommand = new DelegateCommand(FullScreen);
            mainWindow.Closing += (s, e) => OnClosing(e);
            ActiveView = ConfigurationView;
            Grid.SetRow(ActiveView, 1);
            MainWindow.Grid.Children.Add(ActiveView);
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
            //JsonWrite(packs);
        }
        private void OnClosing (CancelEventArgs e)
        {
            UpdatePacks();
            JsonWrite(packs);
        }
        private void SetActivePack(object? obj)
        {
            if (obj is QuestionPackViewModel selectedPack && PlayerViewModel.playState != PlayState.Playing)
            {
                ActivePack = selectedPack;
                if (ActivePack.Questions.Count > 0)
                    ConfigurationViewModel.SelectedIndex = 0;
                else
                    ConfigurationViewModel.SelectedQuestion = null;
            }
        }
        private void OpenAddQuestionPackDialog (object? obj)
        {
            if (PlayerViewModel.playState != PlayState.Playing)
            {
                var addQuestionPackDialog = new AddQuestionPackDialog(this);
                addQuestionPackDialog.ShowDialog();
                if ((bool)addQuestionPackDialog.DialogResult)
                {
                    ActivePack = packs[packs.Count - 1];
                    ConfigurationViewModel.SelectedQuestion = null;
                }
            }
        }
        private void DeleteQuestionPack (object? obj)
        {
            if (PlayerViewModel.playState != PlayState.Playing)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this question pack?", "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    packs.Remove(ActivePack);
                    ActivePack = packs[0];
                    if (ActivePack.Questions.Count > 0)
                        ConfigurationViewModel.SelectedIndex = 0;
                    else
                        ConfigurationViewModel.SelectedIndex = -1;
                }
            }
        }
        private void ShowPlayerView (object? obj)
        {
            PlayerViewModel.StartQuiz();
            MainWindow.Grid.Children.Remove(ActiveView);
            PlayerView = new PlayerView(PlayerViewModel);
            ActiveView = PlayerView;
            Grid.SetRow(ActiveView, 1);
            MainWindow.Grid.Children.Add(ActiveView);
        }
        public void ShowGameOverView()
        {
            MainWindow.Grid.Children.Remove(ActiveView);
            GameOverView = new GameOverView();
            ActiveView = GameOverView;
            Grid.SetRow(ActiveView, 1);
            MainWindow.Grid.Children.Add(ActiveView);
        }
        private void ShowConfigurationView(object? obj)
        {
            PlayerViewModel.EndGame();
            PlayerViewModel.playState = PlayState.NotPlaying;
            MainWindow.Grid.Children.Remove(ActiveView);
            ConfigurationView = new ConfigurationView();
            ActiveView = ConfigurationView;
            Grid.SetRow(ActiveView, 1);
            MainWindow.Grid.Children.Add(ActiveView);
        }
        private void Exit (object? obj)
        {
            MainWindow.Close();
        }
        private void FullScreen(object? obj)
        {
            MainWindow.WindowState = WindowState.Maximized;
        }
    }
}

/*TODO:
 * Disable all submenus in Edit menu during play
 * While playing, the play menu should be disabled
 * The game should not be started if a question pack is empty
 * Json file should be created in the desired path in
 * Design and resizability
 * Icons from FontAwesome
 */
