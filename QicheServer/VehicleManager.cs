using CommonLibrary.Model;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QicheServer
{
    public class VehicleManager
    {
        MongoDbHelper mongoHelper = MongoDbHelper.GetInstance();

        public List<UserAccount> GetAllUserAccountInfo()
        {
            return mongoHelper.FindAll<UserAccount>();
        }

        public List<Vehicle> GetAllVehicle()
        {
            return mongoHelper.FindAll<Vehicle>();
        }

        public bool UpdateVehicle(Vehicle vehicle)
        {
            BsonDocument query = new BsonDocument("_id", vehicle.Id);

            mongoHelper.Remove<Vehicle>(query);
            mongoHelper.Insert(vehicle);

            return true;
        }

        public void AddUser()
        {
            UserAccount user = new UserAccount();
            user.Password = "admin2";
            user.UserName = "admin2";
            user.CreateTime = DateTime.Now;
            user.MarkForDelete = false;
            mongoHelper.Insert(user);
        }

        public void AddVehicle()
        {
            Vehicle vehicle = new Vehicle();
            Policy policy = new Policy();

            List<PolicyType> types = new List<PolicyType>();
            PolicyType policyType = new PolicyType();
            policyType.Name = "保险1";
            policyType.PayMoney = "23";
            policyType.ReceiveMoney = "2";
            policyType.RateFloating = "0.4";
            types.Add(policyType);

            PolicyType policyType2 = new PolicyType();
            policyType2.Name = "保险2";
            policyType2.ReceiveMoney = "3.2";
            policyType2.PayMoney = "33";
            policyType2.RateFloating = "0.43";
            types.Add(policyType2);
            policy.PolicyTypes = types;

            policy.PolicyId = "2222";
            policy.StartDateTime = DateTime.Now;
            policy.EndDateTime = DateTime.Now;
            vehicle.VehicleNumber = "1234567";
            vehicle.policy = policy;
            vehicle.CompanyType = "比亚迪";

            VehicleOwner owner = new VehicleOwner();
            owner.UserName = "狗子";
            owner.IdentityCardId = "443333234234234";
            vehicle.owner = owner;

            mongoHelper.Insert(vehicle);
        }
    }
}
