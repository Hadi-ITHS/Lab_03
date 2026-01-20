using Lab_03.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.ViewModels
{
    [BsonIgnoreExtraElements]
    public class QuestionPackViewModel : ViewModelBase
    {
        private QuestionPack _model;
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name
        {
            get => _model.Name;
            set
            {
                if (_model is not null)
                {
                _model.Name = value;
                RaisePropertyChanged();
                }
                else
                {
                    _model = new QuestionPack(string.Empty);
                    _model.Name = value;
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("time_limit_in_seconds")]
        public int TimeLimitInSeconds
        {
            get => _model.TimeLimitInSeconds;
            set
            {
                if (_model is not null)
                {
                    _model.TimeLimitInSeconds = value;
                    RaisePropertyChanged();
                }
                else
                {
                    _model = new QuestionPack(string.Empty);
                    _model.TimeLimitInSeconds = value;
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("difficulty")]
        public Difficulty Difficulty
        {
            get => _model.Difficulty;
            set
            {
                if (_model is not null)
                {
                    _model.Difficulty = value;
                    RaisePropertyChanged();
                }
                else
                {
                    _model = new QuestionPack(string.Empty);
                    _model.Difficulty = value;
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("category")]
        public string Category
        {
            get => _model.Category;
            set
            {
                if (_model is not null)
                {
                    _model.Category = value;
                    RaisePropertyChanged();
                }
                else
                {
                    _model = new QuestionPack(string.Empty);
                    _model.Category = value;
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("questions")]
        public ObservableCollection<Question> Questions { get; set; }
        [BsonIgnore]
        public List<string[]> RandomizedQuestions { get; set; }
        [BsonIgnore]
        public List<string> RandomizedQueries { get; set; }
        [BsonIgnore]
        public List<string> RandomizedCorrectAnswers { get; set; }
        public QuestionPackViewModel(QuestionPack model)
        {
            _model = model;
            Questions = new ObservableCollection<Question>(_model.Questions);
            Questions.CollectionChanged += Questions_CollectionChanged;
            RandomizedQuestions = new List<string[]>();
            RandomizedQueries = new List<string>();
            RandomizedCorrectAnswers = new List<string>();
        }

        private void Questions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
                foreach (Question q in e.NewItems)
                    _model.Questions.Add(q);
            if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
                foreach (Question q in e.OldItems)
                    _model.Questions.Remove(q);
            if (e.Action == NotifyCollectionChangedAction.Replace && e.NewItems != null && e.NewItems != null)
                _model.Questions[e.OldStartingIndex] = (Question)e.NewItems[0]!;
            if (e.Action == NotifyCollectionChangedAction.Reset)
                _model.Questions.Clear();
        }
    }
}
