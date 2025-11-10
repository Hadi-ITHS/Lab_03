using Lab_03.Commands;
using Lab_03.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.ViewModels
{
    public class PlayerViewModel : ViewModelBase
    {
        private string _questionCountDescription;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private string[] answers;
        public string CurrentQuerry { get; set; }
        public string[] CurrentQuestion { get; set; }
        public string QuestionCountDescription
        {
            get => _questionCountDescription;
            set
            {
                _questionCountDescription = value;
                RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel? ActivePack
        {
            get => _mainWindowViewModel?.ActivePack;
        }
        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            RandomizeQuestions();
            CurrentQuestion = ActivePack.RandomizedQuestions[0];
            CurrentQuerry = ActivePack.RandomizedQuerries[0];
        }
        private void RandomizeQuestions()
        {
            Question[] randomizedQuestions = ActivePack.Questions.ToArray();
            Random.Shared.Shuffle(randomizedQuestions);
            for (int i = 0; i < randomizedQuestions.Length; i++)
            { 
                ActivePack.RandomizedQuestions.Add(RandomizeAnswers(randomizedQuestions[i]));
                ActivePack.RandomizedQuerries.Add(randomizedQuestions[i].Querry);
            }
        }
        private string[] RandomizeAnswers(Question currentQuestion)
        {
            answers = new string[4] { currentQuestion.CorrectAnswer, currentQuestion.IncorrectAnswers[0], currentQuestion.IncorrectAnswers[1], currentQuestion.IncorrectAnswers[2] };
            Random.Shared.Shuffle(answers);
            return answers;
        }
    }
}
