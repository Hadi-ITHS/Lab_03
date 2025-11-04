using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.Models
{
    public enum Dificulty { Easy, Medium, Hard}
    internal class QuestionPack
    {
        public QuestionPack(string name, int timeLimitInSeconds = 30, Dificulty dificulty = Dificulty.Medium)
        {
            Name = name;
            TimeLimitInSeconds = timeLimitInSeconds;
            Dificulty = dificulty;
            Questions = new List<Question>();
        }

        public string Name { get; set; }
        public int TimeLimitInSeconds { get; set; }
        public Dificulty Dificulty { get; set; }
        public List<Question> Questions { get; set; }
    }
}
