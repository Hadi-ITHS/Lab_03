using Lab_03.Commands;
using Lab_03.Models;
using Lab_03.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand OpenPackOptionsCommand { get; }
        private List<QuestionPackViewModel> _allPacks = new List<QuestionPackViewModel>();
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private int _SelectedIndex;
        private Question _selectedQuestion;
        public QuestionPackViewModel ActivePack
        {
            get => _mainWindowViewModel.ActivePack;
            set => _mainWindowViewModel.ActivePack = value;
        }
        public List<QuestionPackViewModel> AllPacks
        {  
            get => _allPacks;
            set
            {
                _allPacks = value;
                RaisePropertyChanged();
            }
        }
        public int SelectedIndex 
        { 
            get => _SelectedIndex;
            set
            {
                _SelectedIndex = value;
                RaisePropertyChanged();
                if (value >= 0 && SelectedIndex != null)
                    SelectedQuestion = ActivePack?.Questions[SelectedIndex];
            }
        }
        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged();
                if (_selectedQuestion != null)
                    ActivePack.Questions[SelectedIndex] = value;
            }
        }
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, CanRemoveQuestion);
            AddQuestionCommand = new DelegateCommand(AddQuestion, CanAddQuestion);
            OpenPackOptionsCommand = new DelegateCommand(OpenPackOptions, CanOpenPackOptions);
            SelectedIndex = 0;
        }
        private void OpenPackOptions(object? obj)
        {
            var packOptionsDialog = new PackOptionsDialog(_mainWindowViewModel);
            packOptionsDialog.ShowDialog();
        }
        private void RemoveQuestion(object? obj)
        {
            if (ActivePack != null)
            {
                ActivePack.Questions.Remove(SelectedQuestion);
                SelectedIndex = ActivePack.Questions.Count - 1;
            }
        }
        private void AddQuestion(object? obj)
        {
            if (ActivePack != null)
            {
                ActivePack.Questions.Add(new Question("New Question", "CorrectAnswer", new string[3] { "IncorrectAnswer 01", "IncorrectAnswer 02", "IncorrectAnswer 03" }));
                SelectedIndex = ActivePack.Questions.Count - 1;
            }
        }
        private bool CanAddQuestion(object? obj)
        {
            if (_mainWindowViewModel.PlayerViewModel.playState == PlayState.Playing)
                return false;
            else
                return true;
        }
        private bool CanRemoveQuestion(object? obj)
        {
            if (_mainWindowViewModel.PlayerViewModel.playState == PlayState.Playing)
                return false;
            else
                return true;
        }
        private bool CanOpenPackOptions(object? obj)
        {
            if (_mainWindowViewModel.PlayerViewModel.playState == PlayState.Playing)
                return false;
            else
                return true;
        }
    }
}