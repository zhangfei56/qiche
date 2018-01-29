using CommonLibrary;
using CommonLibrary.Model;
using HslCommunication;
using HslCommunication.BasicFramework;
using HslCommunication.Enthernet;
using HslCommunication.LogNet;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QicheServer
{
    class Program
    {
       

        static void Main(string[] args)
        {
            //var client = new MongoClient("mongodb://localhost:27017");

            //var database = client.GetDatabase("test");
            //var collection = database.GetCollection<BsonDocument>("user");

            //AccountUser user = new AccountUser();
            //user.UserName = "zhangsan";
            //user.PreLoginTime = DateTime.Now;
            //user.MarkForDelete = false;

            //collection.InsertOne(user.ToBsonDocument());
            //var document = collection.Find(new BsonDocument()).Skip(1).FirstOrDefault();
            //var myObj = BsonSerializer.Deserialize<UserAccount>(document);

            //Timer timer = new Timer();

            SimpleServer server = new SimpleServer();
            server.StartServer();

            while (true)
            {
                string common = Console.ReadLine();
                if(common == "stop")
                {
                    break;
                }

            }
            
        }

   
    }
}
