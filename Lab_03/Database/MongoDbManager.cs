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
        //TODO: change the QuestionPack mapping with QuestionPackViewModel
        public MainWindowViewModel MainWindowViewModel { get; set; }
        string connectionString = "mongodb://localhost:27017/";
        public MongoClient client;
        public MongoDbManager(MainWindowViewModel mainWindowViewModel)
        {
            MainWindowViewModel = mainWindowViewModel;
            client = new MongoClient(connectionString);
        }
        public void InsertQuestionPack (QuestionPackViewModel questionPack)
        {
            var questionPackCollection = client.GetDatabase("HadiDaliri").GetCollection<QuestionPackViewModel>("question_packs");
            questionPackCollection.InsertOne(questionPack);
        }
        public void InsertCategory (string category)
        {
            var addedCategory = new Category(category);
            var categoryCollection = client.GetDatabase("HadiDaliri").GetCollection<Category>("categories");
            categoryCollection.InsertOne(addedCategory);
        }
        public void RemoveCategory (string category)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.category, category);
            var categoryCollection = client.GetDatabase("HadiDaliri").GetCollection<Category>("categories");
            categoryCollection.DeleteOne(filter);
        }
        public List<QuestionPackViewModel> LoadQuestionPacks ()
        {
            var filter = Builders<QuestionPackViewModel>.Filter.Empty;
            var collection = client.GetDatabase("HadiDaliri").GetCollection<QuestionPackViewModel>("question_packs");
            var result = collection.Find(filter).ToList();
            return result;
        }
        public List<string> LoadCategories()
        {
            List<string> result = new List<string>();
            var filter = Builders<Category>.Filter.Empty;
            var collection = client.GetDatabase("HadiDaliri").GetCollection<Category>("categories");
            var categories = collection.Find(filter).ToList();
            foreach (var category in categories)
                result.Add(category.category);
            return result;
        }
        public void UpdateQuestionPacks ()
        {
            var filter = Builders<QuestionPackViewModel>.Filter.Empty;
            var questionPackCollection = client.GetDatabase("HadiDaliri").GetCollection<QuestionPackViewModel>("question_packs");
        }
    }
}
