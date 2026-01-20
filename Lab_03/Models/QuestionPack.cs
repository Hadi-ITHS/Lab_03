using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.Models
{
    public enum Difficulty { Easy, Medium, Hard}
    public class QuestionPack
    {
        public QuestionPack(string name, int timeLimitInSeconds = 30, Difficulty difficulty = Difficulty.Medium, string category = "Default")
        {
            Name = name;
            TimeLimitInSeconds = timeLimitInSeconds;
            Difficulty = difficulty;
            Questions = new List<Question>();
            Category = category;
        }
        public string Name { get; set; }
        public int TimeLimitInSeconds { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Category { get; set; }
        public List<Question> Questions { get; set; }
    }
}
