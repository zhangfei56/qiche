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
    public class Policy
    {
        [BsonElement]
        public string PolicyId { get; set; }        

        [BsonElement]
        public DateTime StartDateTime { get; set; }

        [BsonElement]
        public DateTime EndDateTime { get; set; }

        [BsonElement]
        public bool NeedRemind { get; set; }

        [BsonElement]
        public List<PolicyType> PolicyTypes { get; set; }
    }
}
