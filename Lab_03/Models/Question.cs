using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.Models
{
    internal class Question
    {
        public Question(string querry, string correctAnswer, string incorrectAnswer_01, string incorrectAnswer_02, string incorrectAnswer_03)
        {
            Querry = querry;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[] {incorrectAnswer_01, incorrectAnswer_02, incorrectAnswer_03};
        }

        public string Querry {  get; set; }
        public string CorrectAnswer { get; set; }
        public string [] IncorrectAnswers { get; set; }
    }
}
