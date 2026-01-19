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
    [BsonIgnoreExtraElements]
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
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("time_limit_in_seconds")]
        public int TimeLimitInSeconds { get; set; }

        [BsonElement("difficulty")]
        public Difficulty Difficulty { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("questions")]
        public List<Question> Questions { get; set; }
    }
}
