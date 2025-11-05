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
                SelectedQuestion = ActivePack.Questions[SelectedIndex];
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

            //test
            var pack01 = new QuestionPack("Medium Pack 01");
            ActivePack = new QuestionPackViewModel(pack01);
            ActivePack.Questions.Add(new Question("Querry01", "a", "b", "c", "d"));
            ActivePack.Questions.Add(new Question("Querry02", "A", "B", "C", "D"));
            _allPacks.Add(ActivePack);
            ActivePack.Questions.Clear();

            var pack02 = new QuestionPack("Hard Pack 02");
            ActivePack = new QuestionPackViewModel(pack01);
            ActivePack.Questions.Add(new Question("Querry01", "h", "j", "k", "l"));
            ActivePack.Questions.Add(new Question("Querry02", "H", "J", "K", "L"));
            _allPacks.Add(ActivePack);
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
            ActivePack = _allPacks[(int)arg];
        }

    }
}
