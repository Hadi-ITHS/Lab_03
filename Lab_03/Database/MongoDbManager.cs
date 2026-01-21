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
        public async Task InsertQuestionPackAsync (QuestionPackViewModel questionPack)
        {
            var questionPackCollection = client.GetDatabase("HadiDaliri").GetCollection<QuestionPackViewModel>("question_packs");
            await questionPackCollection.InsertOneAsync(questionPack);
        }
        public async Task RemoveQuestionPackAsync (QuestionPackViewModel questionPack)
        {
            var filter = Builders<QuestionPackViewModel>.Filter.Eq(q => q.Id, questionPack.Id);
            var questionPackCollection = client.GetDatabase("HadiDaliri").GetCollection<QuestionPackViewModel>("question_packs");
            await questionPackCollection.DeleteOneAsync(filter);
        }
        public async Task InsertCategoryAsync (string category)
        {
            var addedCategory = new Category(category);
            var categoryCollection = client.GetDatabase("HadiDaliri").GetCollection<Category>("categories");
            await categoryCollection.InsertOneAsync(addedCategory);
        }
        public async Task RemoveCategoryAsync (string category)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.category, category);
            var categoryCollection = client.GetDatabase("HadiDaliri").GetCollection<Category>("categories");
            await categoryCollection.DeleteOneAsync(filter);
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
        public async Task ReplaceQuestionPackAsync (QuestionPackViewModel questionPack)
        {
            var filter = Builders<QuestionPackViewModel>.Filter.Eq(q => q.Id, questionPack.Id);
            var questionPackCollection = client.GetDatabase("HadiDaliri").GetCollection<QuestionPackViewModel>("question_packs");
            await questionPackCollection.ReplaceOneAsync(filter, questionPack);
        }
    }
}