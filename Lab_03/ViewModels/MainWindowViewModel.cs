using Lab_03.Commands;
using Lab_03.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab_03.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public DelegateCommand SetActivePackCommand { get; }
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
                //PlayerViewModel.RaisePropertyChanged(nameof(ViewModels.PlayerViewModel.ActivePack));
            }
        }
        public MainWindowViewModel()
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            packs = new ObservableCollection<QuestionPackViewModel>();
            SetActivePackCommand = new DelegateCommand(SetActivePack);
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

            List<QuestionPack> importedPacks = JsonSerializer.Deserialize<List<QuestionPack>>(File.ReadAllText(("Questions.json")));
            foreach (var pack in importedPacks)
            {
                packs.Add(new QuestionPackViewModel(pack));
            }

        }

        private void SetActivePack(object? obj)
        {
            if (obj is QuestionPackViewModel selectedPack)
            {
                ActivePack = selectedPack;
            }
        }
    }
}
