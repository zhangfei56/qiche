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
    public class Vehicle
    {
        [BsonId]
        [JsonConverter(typeof(ConvertHelper))]
        public ObjectId Id { get; set; }

        // 类型 如 客车 轿车
        [BsonElement]
        public string VehicleType { get; set; }

        // 厂牌型号 比亚迪
        [BsonElement]
        public string CompanyType { get; set; }

        //车牌号
        [BsonElement]
        public string VehicleNumber { get; set; }

        [BsonElement]
        public VehicleOwner owner { get; set; }

        [BsonElement]
        public Policy policy { get; set; }
    }
}
