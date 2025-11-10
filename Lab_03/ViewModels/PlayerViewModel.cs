using Lab_03.Commands;
using Lab_03.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Lab_03.ViewModels
{   public enum PlayState { Playing, EndGame }
    public class PlayerViewModel : ViewModelBase
    {
        private PlayState playState;
        private int currentIndex = 0;
        private DispatcherTimer dispatcherTimer;
        private string _questionCountDescription;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private string[] answers;
        private int _timer;
        private string _currentQuerry;
        private string[] _currentQuestion;
        public string CurrentQuerry
        {
            get => _currentQuerry;
            set
            {
                _currentQuerry = value;
                RaisePropertyChanged();
            }
        }
        public string[] CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                RaisePropertyChanged();
            }
        }
        public int Timer 
        {
            get => _timer;
            set
            {
                _timer = value;
                RaisePropertyChanged();
            }
        }
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
            CurrentQuestion = ActivePack.RandomizedQuestions[currentIndex];
            CurrentQuerry = ActivePack.RandomizedQuerries[currentIndex];
            Timer = ActivePack.TimeLimitInSeconds;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1f);
            dispatcherTimer.Tick += Timer_Tick;
            dispatcherTimer.Start();
            playState = PlayState.Playing;
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Timer > 0)
                Timer--;
            else
            {
                NextQuestion();
                Timer = ActivePack.TimeLimitInSeconds;
            }
        }
        private void NextQuestion ()
        {
            currentIndex++;
            if (currentIndex < ActivePack.RandomizedQuestions.Count)
            {
                dispatcherTimer.Stop();
                CurrentQuestion = ActivePack.RandomizedQuestions[currentIndex];
                CurrentQuerry = ActivePack.RandomizedQuerries[currentIndex];
                dispatcherTimer.Start();
            }
            else
            { 
                playState = PlayState.EndGame;
                dispatcherTimer.Stop();
            }
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