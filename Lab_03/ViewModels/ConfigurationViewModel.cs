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
        private List<QuestionPackViewModel> _allPacks = new List<QuestionPackViewModel>();
        public DelegateCommand SetActivePackCommand {  get; }
        private readonly MainWindowViewModel? _mainWindowViewModel;
        public QuestionPackViewModel ActivePack { get; set; }
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
            SetActivePackCommand = new DelegateCommand(SetActivePack);
        }
        private bool CanSetActivePack (object? arg)
        {
            if (_allPacks is not null)
                return true;
            else
                return false;
        }

        private void SetActivePack (object? arg)
        {
            ActivePack =  _allPacks[(int)arg];
        }

    }
}
