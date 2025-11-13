using Lab_03.Commands;
using Lab_03.Models;
using Lab_03.Views;
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
        public DelegateCommand SetActivePackCommand { get; }
        public MainWindow MainWindow { get; set; }
        public PlayerView PlayerView { get; set; }
        public GameOverView GameOverView { get; set; }
        public ConfigurationView ConfigurationView { get; set; }
        public UserControl ActiveView { get; set; }
        private string appDataPath;
        private string jsonPath;
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
            appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Lab03");
            Directory.CreateDirectory(appDataPath);
            jsonPath = Path.Combine(appDataPath, "Questions.json");
            packs = new ObservableCollection<QuestionPackViewModel>();
            LoadPacksAsync();
            
            MainWindow = mainWindow;
            ConfigurationView = new ConfigurationView();
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            SetActivePackCommand = new DelegateCommand(SetActivePack);
            OpenAddQuestionPackDialogCommand = new DelegateCommand(OpenAddQuestionPackDialog);
            DeleteQuestionPackCommand = new DelegateCommand(DeleteQuestionPack);
            ExitCommand = new DelegateCommand(Exit);
            ShowConfigurationViewCommand = new DelegateCommand(ShowConfigurationView, CanShowConfigurationView);
            ShowPlayerViewCommand = new DelegateCommand(ShowPlayerView, CanShowPlayerView);
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
            JsonWriteAsync();
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
        private bool CanShowPlayerView (object? obj)
        {
            if (PlayerViewModel.playState == PlayState.Playing || ActivePack.Questions.Count <= 0)
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
        private async Task JsonWriteAsync()
        {
            //File.WriteAllText(jsonPath, JsonSerializer.Serialize(packs));
/*            if (!File.Exists(jsonPath))
            {
                File.Create(jsonPath);
            }*/
            using FileStream stream = File.OpenWrite(jsonPath);
            await JsonSerializer.SerializeAsync(stream, packs);
        }
        private async Task LoadPacksAsync()
        {
            var importPacksTask = JsonReadAsync();
            var importedPacks = await importPacksTask;
            foreach (var pack in importedPacks)
            {
                packs.Add(new QuestionPackViewModel(pack));
            }
            if (packs.Count > 0)
                ActivePack = packs[0];
        }
        private async Task<List<QuestionPack>> JsonReadAsync()
        {
            if (!File.Exists(jsonPath))
            {
                File.Create(jsonPath);
                var tmpPack = new List<QuestionPack>();
                tmpPack.Add(new QuestionPack("Default pack"));
                return tmpPack;
            }
            else
            {
                using FileStream stream = File.OpenRead(jsonPath);
                var importedPacks = await JsonSerializer.DeserializeAsync<List<QuestionPack>>(stream);
                return importedPacks ?? new List<QuestionPack>();
            }   
        }
    }
}