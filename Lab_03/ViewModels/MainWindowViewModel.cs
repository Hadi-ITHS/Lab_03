using Lab_03.Commands;
using Lab_03.Database;
using Lab_03.Models;
using Lab_03.Views;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
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
        public DelegateCommand OpenCategoryOptionsCommand { get; }
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
        public MongoDbManager MongoDbManager { get; set; }
        private List<string> _categories ;
        public List<string> Categories 
        {
            get => _categories ?? (_categories = new List<string>()); 
            set
            {
                _categories = value;
                RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel ActivePack
        {
            get => _activePack;
            set
            {
                if (_activePack != null)
                    MongoDbManager.ReplaceQuestionPackAsync(_activePack);
                _activePack = value;
                RaisePropertyChanged();
                ConfigurationViewModel?.RaisePropertyChanged(nameof(ConfigurationViewModel.ActivePack));
                PlayerViewModel?.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
            }
        }
        public MainWindowViewModel(MainWindow mainWindow)
        {
            Categories = new List<string>();
            MongoDbManager = new MongoDbManager(this);
            packs = new ObservableCollection<QuestionPackViewModel>();
            LoadCategories(); 
            LoadQuestionPacksAsync();
            ActivePack = packs[0];
            MainWindow = mainWindow;
            ConfigurationView = new ConfigurationView();
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            SetActivePackCommand = new DelegateCommand(SetActivePack);
            OpenAddQuestionPackDialogCommand = new DelegateCommand(OpenAddQuestionPackDialog);
            DeleteQuestionPackCommand = new DelegateCommand(DeleteQuestionPack);
            OpenCategoryOptionsCommand = new DelegateCommand(OpenCategoryOptionsDialog);
            ExitCommand = new DelegateCommand(Exit);
            ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView, CanShowConfigurationView);
            ShowPlayerViewCommand = new DelegateCommand(ShowPlayerView, CanShowPlayerView);
            FullscreenCommand = new DelegateCommand(FullScreen);
            mainWindow.Closing += (s, e) => OnClosing(e);
            ActiveView = ConfigurationView;
            Grid.SetRow(ActiveView, 1);
            MainWindow.Grid.Children.Add(ActiveView);
        }
        private void OpenCategoryOptionsDialog (object obj)
        {
            if (PlayerViewModel.playState != PlayState.Playing)
            {
                var categoryOptionsDialog = new CategoryOptionsDialog (this);
                categoryOptionsDialog.ShowDialog();
            }
        }
        private async void OnClosing (CancelEventArgs e)
        {
            await MongoDbManager.ReplaceQuestionPackAsync(ActivePack);
        }
        private void SetActivePack(object? obj)
        {
            if (obj is QuestionPackViewModel selectedPack && PlayerViewModel.playState != PlayState.Playing)
            {
                ActivePack = selectedPack;
                ShowPlayerViewCommand.RaiseCanExecuteChanged();
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
        private async void DeleteQuestionPack (object? obj)
        {
            if (PlayerViewModel.playState != PlayState.Playing)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this question pack?", "Confirm Action", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes && packs.Count > 1)
                {
                    await MongoDbManager.RemoveQuestionPackAsync(ActivePack);
                    var packToRemove = ActivePack;
                    ActivePack = packs[0];
                    packs.Remove(packToRemove);
                    if (packs.Count > 0)
                    {
                        ActivePack = packs[0];
                        if (ActivePack.Questions.Count > 0)
                            ConfigurationViewModel.SelectedIndex = 0;
                        else
                            ConfigurationViewModel.SelectedIndex = -1;
                    }
                    return;
                }
                MessageBoxResult error = MessageBox.Show($"Question pack is not deleted!\nAt least one question pack should exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void ShowPlayerView (object? obj)
        {
            PlayerViewModel.StartQuiz();
            MainWindow.Grid.Children.Remove(ActiveView);
            PlayerView = new PlayerView(PlayerViewModel);
            ActiveView = PlayerView;
            Grid.SetRow(ActiveView, 1);
            MainWindow.Grid.Children.Add(ActiveView);
            await MongoDbManager.ReplaceQuestionPackAsync(ActivePack);
        }
        private bool CanShowPlayerView (object? obj)
        {
            if (PlayerViewModel.playState == PlayState.Playing || ActivePack?.Questions.Count <= 0)
                return false;
            else
                return true;
        }
        private bool CanShowConfigurationView(object? obj)
        {
            if (PlayerViewModel.playState == PlayState.NotPlaying)
                return false;
            else
                return true;
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
            PlayingStateChanged();
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
        private void PlayingStateChanged()
        {
            ConfigurationViewModel.AddQuestionCommand.RaiseCanExecuteChanged();
            ConfigurationViewModel.RemoveQuestionCommand.RaiseCanExecuteChanged();
            ConfigurationViewModel.OpenPackOptionsCommand.RaiseCanExecuteChanged();
            ShowConfigurationViewCommand.RaiseCanExecuteChanged();
            ShowPlayerViewCommand.RaiseCanExecuteChanged();
        }
        private async Task LoadQuestionPacksAsync()
        {
            var collection = MongoDbManager.LoadQuestionPacks();
            foreach (var document in collection)
            {
                document.RandomizedQueries = new List<string>();
                document.RandomizedQuestions = new List<string[]>();
                document.RandomizedCorrectAnswers = new List<string>();
                packs.Add(document);
            }
            if (packs.Count < 1)
            {
                var demoQuestion = new List<Question>();
                demoQuestion.Add(new Question("What is the capital of Sweden?", "Stockholm", ["Malmö", "Göteborg", "Uppsala"]));
                packs.Add(new QuestionPackViewModel(new QuestionPack("Default pack") { Questions = demoQuestion}));
                await MongoDbManager.InsertQuestionPackAsync(packs[0]);
                ActivePack = packs[0];
            }
            else
                ActivePack = packs[0];
        }
        private async Task LoadCategories ()
        {
            var collection = MongoDbManager.LoadCategories();
            foreach (var document in collection)
                Categories.Add(document);
            if (Categories.Count < 1)
            {
                Categories.Add("Default");
                await MongoDbManager.InsertCategoryAsync(Categories[0]);
            }
        }
    }
}