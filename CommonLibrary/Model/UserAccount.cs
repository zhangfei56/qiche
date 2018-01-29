using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Model
{
    public class UserAccount
    {
        [BsonId]
        [JsonConverter(typeof(ConvertHelper))]
        public ObjectId Id { get; set; }

        [BsonElement]
        public string UserName { get; set; }

        [BsonElement]
        public string Password { get; set; }

        [BsonElement]
        public DateTime CreateTime { get; set; }

        [BsonElement]
        public DateTime PreLoginTime { get; set; }

        [BsonElement]
        public bool MarkForDelete { get; set; }
    }
}
