using Lab_03.Commands;
using Lab_03.Models;
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
        private List<QuestionPackViewModel> _allPacks = new List<QuestionPackViewModel>();
        private readonly MainWindowViewModel? _mainWindowViewModel;
        public QuestionPackViewModel ActivePack;
        private int _SelectedIndex;
        private Question _selectedQuestion;
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
                if (value >= 0)
                    SelectedQuestion = _mainWindowViewModel.ActivePack.Questions[SelectedIndex];
            }
        }
        public Question SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                _selectedQuestion = value;
                RaisePropertyChanged();
            }
        }

        

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            ActivePack = _mainWindowViewModel.ActivePack;
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);
            AddQuestionCommand = new DelegateCommand(AddQuestion);
        }
        private void RemoveQuestion(object? obj)
        {
            _mainWindowViewModel.ActivePack.Questions.Remove(SelectedQuestion);
        }
        private void AddQuestion(object? obj)
        {
            _mainWindowViewModel.ActivePack.Questions.Add(new Question("New Question", "CorrectAnswer", new string[3] {null,null,null }));
        }
    }
}