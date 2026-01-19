using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_03.Models
{
    public class Category
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("category")]
        public String category = string.Empty;
        public Category (string input)
        {
            category = input;
        }
    }
}
