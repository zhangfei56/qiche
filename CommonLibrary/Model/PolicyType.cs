using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Model
{
    public class PolicyType
    {

        //承保险种
        [BsonElement]
        public string Name { get; set; }

        //费率浮动
        [BsonElement]
        public string RateFloating { get; set; }

        // 保险金额
        [BsonElement]
        public string ReceiveMoney { get; set; }

        // 保险费
        [BsonElement]
        public string PayMoney { get; set; }


    }
}
