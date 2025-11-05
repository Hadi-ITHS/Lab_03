using Lab_03.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<QuestionPackViewModel> packs { get; }
        private QuestionPackViewModel _activePack;
        public PlayerViewModel? playerViewModel { get;}
        public ConfigurationViewModel? configurationViewModel { get; }
        public QuestionPackViewModel ActivePack
        {
            get => _activePack;
            set
            {
                _activePack = value;
                RaisePropertyChanged();
                playerViewModel.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
            }
        }
        public MainWindowViewModel()
        {
            playerViewModel = new PlayerViewModel(this);
            configurationViewModel = new ConfigurationViewModel(this);

            var pack = new QuestionPack("MyQuestionPack");
            ActivePack = new QuestionPackViewModel(pack);
        }


    }
}
