using Lab_03.Models;
using Lab_03.ViewModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Lab_03.Database
{
    public class MongoDbManager
    {
        public MainWindowViewModel MainWindowViewModel { get; set; }
        string connectionString = "mongodb://localhost:27017/";
        public MongoClient client;
        public MongoDbManager(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            client = new MongoClient(connectionString);
        }
        public void InsertQuestionPack (QuestionPack questionPack)
        {
            var questionPackCollection = client.GetDatabase("Quiz").GetCollection<QuestionPack>("question_packs");
            questionPackCollection.InsertOne(questionPack);
        }
        public void InsertCategory (string category)
        {
            var addedCategory = new Category(category);
            var categoryCollection = client.GetDatabase("Quiz").GetCollection<Category>("categories");
            categoryCollection.InsertOne(addedCategory);
        }
        public List<QuestionPack> LoadQuestionPacks ()
        {
            var filter = Builders<QuestionPack>.Filter.Empty;
            var collection = client.GetDatabase("Quiz").GetCollection<QuestionPack>("question_packs");
            var result = collection.Find(filter).ToList();
            return result;
        }
        public List<string> LoadCategories()
        {
            List<string> result = new List<string>();
            var filter = Builders<Category>.Filter.Empty;
            var collection = client.GetDatabase("Quiz").GetCollection<Category>("categories");
            var categories = collection.Find(filter).ToList();
            foreach (var category in categories)
                result.Add(category.category);
            return result;
        }
        public void UpdateCollection ()
        {
            var filter = Builders<QuestionPack>.Filter.Empty;
            var questionPackCollection = client.GetDatabase("Quiz").GetCollection<QuestionPack>("question_packs");
        }
    }
}
