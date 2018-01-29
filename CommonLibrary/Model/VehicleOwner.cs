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
    public class VehicleOwner
    {
        [BsonElement]
        public string UserName { get; set; }

        [BsonElement]
        public string IdentityCardId { get; set; }
    }
}
