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
                PlayerViewModel.RaisePropertyChanged(nameof(ViewModels.PlayerViewModel.ActivePack));
            }
        }
        public MainWindowViewModel()
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            packs = new ObservableCollection<QuestionPackViewModel>();
            //test
            /*var pack01 = new QuestionPack("Medium Pack 01");
            pack01.Questions.Add(new Question("Querry01", "a", "b", "c", "d"));
            pack01.Questions.Add(new Question("Querry02", "A", "B", "C", "D"));
            ActivePack = new QuestionPackViewModel(pack01);
            packs.Add(ActivePack);


            var pack02 = new QuestionPack("Hard Pack 02");
            pack02.Questions.Add(new Question("Querry01", "g", "h", "j", "k"));
            pack02.Questions.Add(new Question("Querry02", "G", "H", "J", "K"));
            ActivePack = new QuestionPackViewModel(pack02);
            packs.Add(ActivePack);
            packs.Clear();*/

            //test

            /*string json = JsonSerializer.Serialize(packs);
            File.WriteAllText("Questions.json", json);*/

            List<QuestionPack>  importedPacks = JsonSerializer.Deserialize<List<QuestionPack>>(File.ReadAllText(("Questions.json")));
            foreach (var pack in importedPacks)
            {
                ActivePack = new QuestionPackViewModel(pack);
                packs.Add(ActivePack);
            }
            //packs.Add(ActivePack);
        }
    }
}
