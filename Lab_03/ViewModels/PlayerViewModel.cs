using Lab_03.Commands;
using Lab_03.Models;
using Lab_03.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace Lab_03.ViewModels
{   public enum PlayState { Playing, EndGame }
    public class PlayerViewModel : ViewModelBase
    {
        public int Points { get; set; }
        private PlayState playState;
        public int currentIndex = 0;
        public bool IsAnswerChosen { get; set; }
        private DispatcherTimer dispatcherTimer;
        private string _questionCountDescription;
        private readonly MainWindowViewModel _mainWindowViewModel;
        private string[] answers;
        private int _timer;
        private string _currentQuerry;
        private string[] _currentQuestion;
        public string GameOverViewText { get; set; }
        public string ChosenAnswer { get; set; }
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
        public int TimeLimit 
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
        }
        public void StartQuiz ()
        {
            if (ActivePack.Questions.Count > 0)
            {
                Points = 0;
                currentIndex = 0;
                QuestionCountDescription = $"Question {currentIndex + 1} of {ActivePack.Questions.Count}";
                RandomizeQuestions();
                CurrentQuestion = ActivePack.RandomizedQuestions[currentIndex];
                CurrentQuerry = ActivePack.RandomizedQuerries[currentIndex];
                TimeLimit = ActivePack.TimeLimitInSeconds;
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Interval = TimeSpan.FromSeconds(1f);
                dispatcherTimer.Tick += Timer_Tick;
                dispatcherTimer.Start();
                playState = PlayState.Playing;
            }
        }
        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (TimeLimit > 0)
            {
                TimeLimit--;
                if (IsAnswerChosen)
                {
                    Thread.Sleep(1000);
                    NextQuestion();
                }
            }
            else
            {
                NextQuestion();
            }
        }
        public void NextQuestion ()
        {
            ResetButtonColors();
            IsAnswerChosen = false;
            if (ValidateAnswer())
                Points++;
            if (currentIndex < ActivePack.Questions.Count - 1)
            {
                TimeLimit = ActivePack.TimeLimitInSeconds;
                currentIndex++;
                QuestionCountDescription = $"Question {currentIndex + 1} of {ActivePack.Questions.Count}";
                dispatcherTimer.Stop();
                CurrentQuestion = ActivePack.RandomizedQuestions[currentIndex];
                CurrentQuerry = ActivePack.RandomizedQuerries[currentIndex];
                dispatcherTimer.Start();
            }
            else
            {
                EndGame();
                _mainWindowViewModel.ShowGameOverView();
            }
        }
        public void EndGame()
        {
            dispatcherTimer.Stop();
            GameOverViewText = $"You got {Points} out of {ActivePack.Questions.Count} answers correct!";
            playState = PlayState.EndGame;
        }
        private void RandomizeQuestions()
        {
            ActivePack.RandomizedQuestions.Clear();
            ActivePack.RandomizedQuerries.Clear();
            ActivePack.RandomizedCorrectAnswers.Clear();
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
            ActivePack.RandomizedCorrectAnswers.Add(currentQuestion.CorrectAnswer);
            Random.Shared.Shuffle(answers);
            return answers;
        }
        private bool ValidateAnswer()
        {
            if (ChosenAnswer == ActivePack.RandomizedCorrectAnswers[currentIndex])
                return true;
            else
                return false;
        }
        private void ResetButtonColors()
        {
            _mainWindowViewModel.PlayerView.Button0.Background = Brushes.LightGray;
            _mainWindowViewModel.PlayerView.Button1.Background = Brushes.LightGray;
            _mainWindowViewModel.PlayerView.Button2.Background = Brushes.LightGray;
            _mainWindowViewModel.PlayerView.Button3.Background = Brushes.LightGray;
        }
    }
}