using Lab_03.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Lab_03.ViewModels
{
    public class QuestionPackViewModel : ViewModelBase
    {
        public QuestionPack Model { get;}
        public string Name
        {
            get => Model.Name;
            set
            {
                Model.Name = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimitInSeconds
        {
            get => Model.TimeLimitInSeconds;
            set
            {
                Model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty Difficulty
        {
            get => Model.Difficulty;
            set
            {
                Model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Question> Questions { get; set; }
        public List<string[]> RandomizedQuestions { get; set; }
        public List<string> RandomizedQueries { get; set; }
        public List<string> RandomizedCorrectAnswers { get; set; }
        public QuestionPackViewModel(QuestionPack model)
        {
            Model = model;
            Questions = new ObservableCollection<Question>(Model.Questions);
            Questions.CollectionChanged += Questions_CollectionChanged;
            RandomizedQuestions = new List<string[]>();
            RandomizedQueries = new List<string>();
            RandomizedCorrectAnswers = new List<string>();
        }

        private void Questions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
                foreach (Question q in e.NewItems)
                    Model.Questions.Add(q);
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
                foreach (Question q in e.OldItems)
                    Model.Questions.Remove(q);
            if (e.Action == NotifyCollectionChangedAction.Replace && e.NewItems != null && e.NewItems != null)
                Model.Questions[e.OldStartingIndex] = (Question)e.NewItems[0]!;
            if (e.Action == NotifyCollectionChangedAction.Reset)
                Model.Questions.Clear();
        }
    }
}
