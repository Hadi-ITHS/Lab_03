using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.Models
{
    public class Question
    {
        public Question(string querry, string correctAnswer, string[] incorrectAnswers)
        {
            Querry = querry;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = incorrectAnswers;
        }

        public string Querry {  get; set; }
        public string CorrectAnswer { get; set; }
        public string [] IncorrectAnswers { get; set; }
    }
}
