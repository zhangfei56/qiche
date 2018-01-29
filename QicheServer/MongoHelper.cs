using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QicheServer
{
    /// <summary>  
    /// Mongo db的数据库帮助类  
    /// </summary>  
    public class MongoDbHelper
    {

        private IMongoDatabase database;

        private static MongoDbHelper mongoDbHelper;

        public static MongoDbHelper GetInstance()
        {
            if(mongoDbHelper == null)
            {
                mongoDbHelper = new MongoDbHelper();
            }
            return mongoDbHelper;
        }
        /// <summary>  
        /// ObjectId的键  
        /// </summary>  
        private readonly string OBJECTID_KEY = "_id";

        public MongoDbHelper()
        {
            var client = new MongoClient("mongodb://localhost:27017");

            this.database = client.GetDatabase("test");

        }

        #region 插入数据  
        /// <summary>  
        /// 将数据插入进数据库  
        /// </summary>  
        /// <typeparam name="T">需要插入数据的类型</typeparam>  
        /// <param name="t">需要插入的具体实体</param>  
        public bool Insert<T>(T t)
        {
            //集合名称  
            string collectionName = typeof(T).Name;
            return Insert<T>(t, collectionName);
        }

        /// <summary>  
        /// 将数据插入进数据库  
        /// </summary>  
        /// <typeparam name="T">需要插入数据库的实体类型</typeparam>  
        /// <param name="t">需要插入数据库的具体实体</param>  
        /// <param name="collectionName">指定插入的集合</param>  
        public bool Insert<T>(T t, string collectionName)
        {
            IMongoCollection<BsonDocument> mc = this.database.GetCollection<BsonDocument>(collectionName);
            //将实体转换为bson文档  
            BsonDocument bd = t.ToBsonDocument();
            //进行插入操作  
            mc.InsertOne(bd);

            return true;

        }

        /// <summary>  
        /// 批量插入数据  
        /// </summary>  
        /// <typeparam name="T">需要插入数据库的实体类型</typeparam>  
        /// <param name="list">需要插入数据的列表</param>  
        public bool Insert<T>(List<T> list)
        {
            //集合名称  
            string collectionName = typeof(T).Name;
            return this.Insert<T>(list, collectionName);
        }

        /// <summary>  
        /// 批量插入数据  
        /// </summary>  
        /// <typeparam name="T">需要插入数据库的实体类型</typeparam>  
        /// <param name="list">需要插入数据的列表</param>  
        /// <param name="collectionName">指定要插入的集合</param>  
        public bool Insert<T>(List<T> list, string collectionName)
        {

            IMongoCollection<BsonDocument> mc = this.database.GetCollection<BsonDocument>(collectionName);
            //创建一个空间bson集合  
            List<BsonDocument> bsonList = new List<BsonDocument>();
            //批量将数据转为bson格式 并且放进bson文档  
            list.ForEach(t => bsonList.Add(t.ToBsonDocument()));
            //批量插入数据  
            mc.InsertMany(bsonList);
            return true;

        }
        #endregion

        #region 查询数据  

        #region 查询所有记录  
        /// <summary>  
        /// 查询一个集合中的所有数据  
        /// </summary>  
        /// <typeparam name="T">该集合数据的所属类型</typeparam>  
        /// <param name="collectionName">指定集合的名称</param>  
        /// <returns>返回一个List列表</returns>  
        private List<T> FindAll<T>(string collectionName)
        {

            IMongoCollection<T> mc = this.database.GetCollection<T>(collectionName);
            //以实体方式取出其数据集合  
            List<T> mongoCursor = mc.Find(new BsonDocument()).ToList();
            //直接转化为List返回  
            return mongoCursor;

        }

        /// <summary>  
        /// 查询一个集合中的所有数据 其集合的名称为T的名称  
        /// </summary>  
        /// <typeparam name="T">该集合数据的所属类型</typeparam>  
        /// <returns>返回一个List列表</returns>  
        public List<T> FindAll<T>()
        {
            string collectionName = typeof(T).Name;
            return FindAll<T>(collectionName);            
        }
        #endregion


        /// <summary>  
        /// 查询一条记录  
        /// </summary>  
        /// <typeparam name="T">该数据所属的类型</typeparam>  
        /// <param name="query">查询的条件 可以为空</param>  
        /// <param name="collectionName">去指定查询的集合</param>  
        /// <returns>返回一个实体类型</returns>  
        private T FindOne<T>(BsonDocument query, string collectionName)
        {
                IMongoCollection<T> mc = this.database.GetCollection<T>(collectionName);
                query = this.InitQuery(query);
                T t = mc.Find(query).FirstOrDefault();

                return t;
        }

        /// <summary>  
        /// 查询一条记录  
        /// </summary>  
        /// <typeparam name="T">该数据所属的类型</typeparam>  
        /// <param name="collectionName">去指定查询的集合</param>  
        /// <returns>返回一个实体类型</returns>  
        private T FindOne<T>(string collectionName)
        {
            return FindOne<T>(null, collectionName);
        }

        /// <summary>  
        /// 查询一条记录  
        /// </summary>  
        /// <typeparam name="T">该数据所属的类型</typeparam>  
        /// <returns>返回一个实体类型</returns>  
        public T FindOne<T>()
        {
            string collectionName = typeof(T).Name;
            return FindOne<T>(null, collectionName);
        }


        /// <summary>  
        /// 查询一条记录  
        /// </summary>  
        /// <typeparam name="T">该数据所属的类型</typeparam>  
        /// <param name="query">查询的条件 可以为空</param>  
        /// <returns>返回一个实体类型</returns>  
        public T FindOne<T>(BsonDocument query)
        {
            string collectionName = typeof(T).Name;
            return FindOne<T>(query, collectionName);
        }
        #endregion

        #region 普通的条件查询  
        /// <summary>  
        /// 根据指定条件查询集合中的数据  
        /// </summary>  
        /// <typeparam name="T">该集合数据的所属类型</typeparam>  
        /// <param name="query">指定的查询条件 比如Query.And(Query.EQ("username","admin"),Query.EQ("password":"admin"))</param>  
        /// <param name="collectionName">指定的集合的名称</param>  
        /// <returns>返回一个List列表</returns>  
        private List<T> Find<T>(BsonDocument query, string collectionName)
        {
            IMongoCollection<T> mc = this.database.GetCollection<T>(collectionName);
            query = this.InitQuery(query);
            var mongoCursor = mc.Find(query);
            return mongoCursor.ToList<T>();

        }

        /// <summary>  
        /// 根据指定条件查询集合中的数据  
        /// </summary>  
        /// <typeparam name="T">该集合数据的所属类型</typeparam>  
        /// <param name="query">指定的查询条件 比如Query.And(Query.EQ("username","admin"),Query.EQ("password":"admin"))</param>  
        /// <returns>返回一个List列表</returns>  
        public List<T> Find<T>(BsonDocument query)
        {
            string collectionName = typeof(T).Name;
            return this.Find<T>(query, collectionName);
        }
        #endregion




        #region 更新数据  

        /// <summary>  
        /// 更新数据  
        /// </summary>  
        /// <typeparam name="T">更新的数据 所属的类型</typeparam>  
        /// <param name="query">更新数据的查询</param>  
        /// <param name="update">需要更新的文档</param>  
        /// <param name="collectionName">指定更新集合的名称</param>  
        public bool Update<T>(BsonDocument query, BsonDocument update, string collectionName)
        {
            IMongoCollection<T> mc = this.database.GetCollection<T>(collectionName);
            query = this.InitQuery(query);
            //更新数据  
            mc.UpdateOne(query, update);
            return true;
        }

        /// <summary>  
        /// 更新数据  
        /// </summary>  
        /// <typeparam name="T">更新的数据 所属的类型</typeparam>  
        /// <param name="query">更新数据的查询</param>  
        /// <param name="update">需要更新的文档</param>  
        public bool Update<T>(BsonDocument query, BsonDocument update)
        {
            string collectionName = typeof(T).Name;
            return this.Update<T>(query, update, collectionName);
        }

        #endregion

        #region 移除/删除数据  
        /// <summary>  
        /// 移除指定的数据  
        /// </summary>  
        /// <typeparam name="T">移除的数据类型</typeparam>  
        /// <param name="query">移除的数据条件</param>  
        /// <param name="collectionName">指定的集合名词</param>  
        public bool Remove<T>(BsonDocument query, string collectionName)
        {
                IMongoCollection<T> mc = this.database.GetCollection<T>(collectionName);
                query = this.InitQuery(query);
                //根据指定查询移除数据  
                mc.DeleteOne(query);
                return true;
     
        }

        /// <summary>  
        /// 移除指定的数据  
        /// </summary>  
        /// <typeparam name="T">移除的数据类型</typeparam>  
        /// <param name="query">移除的数据条件</param>  
        public bool Remove<T>(BsonDocument query)
        {
            string collectionName = typeof(T).Name;
            return this.Remove<T>(query, collectionName);
        }

        /// <summary>  
        /// 移除实体里面所有的数据  
        /// </summary>  
        /// <typeparam name="T">移除的数据类型</typeparam>  
        public bool ReomveAll<T>()
        {
            string collectionName = typeof(T).Name;
            return this.Remove<T>(null, collectionName);
        }

        /// <summary>  
        /// 移除实体里面所有的数据  
        /// </summary>  
        /// <typeparam name="T">移除的数据类型</typeparam>  
        /// <param name="collectionName">指定的集合名称</param>  
        public bool RemoveAll<T>(string collectionName)
        {
            return this.Remove<T>(null, collectionName);
        }
        #endregion



        #region 获取表的行数  
        /// <summary>  
        /// 获取数据表总行数  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="query"></param>  
        /// <param name="collectionName"></param>  
        /// <returns></returns>  
        public long GetCount<T>(BsonDocument query, string collectionName)
        {
            IMongoCollection<T> mc = this.database.GetCollection<T>(collectionName);
            if (query == null)
            {
                return mc.Count(new BsonDocument());
            }
            else
            {
                return mc.Count(query);
            }
        }
        /// <summary>  
        /// 获取数据表总行数  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="query"></param>  
        /// <returns></returns>  
        public long GetCount<T>(BsonDocument query)
        {
            string collectionName = typeof(T).Name;
            return GetCount<T>(query, collectionName);
        }

        #endregion

        #region 获取集合的存储大小  
        /// <summary>  
        /// 获取集合的存储大小  
        /// </summary>  
        /// <typeparam name="T">该集合对应的实体类</typeparam>  
        /// <returns>返回一个long型</returns>  
        public long GetDataSize<T>()
        {
            string collectionName = typeof(T).Name;
            return GetDataSize(collectionName);
        }

        /// <summary>  
        /// 获取集合的存储大小  
        /// </summary>  
        /// <param name="collectionName">该集合对应的名称</param>  
        /// <returns>返回一个long型</returns>  
        private long GetDataSize(string collectionName)
        {
            IMongoCollection<BsonDocument> mc = this.database.GetCollection<BsonDocument>(collectionName);
            return mc.Count(new BsonDocument());
        }


        #endregion

        #region 私有的一些辅助方法  
        /// <summary>  
        /// 初始化查询记录 主要当该查询条件为空时 会附加一个恒真的查询条件，防止空查询报错  
        /// </summary>  
        /// <param name="query">查询的条件</param>  
        /// <returns></returns>  
        private BsonDocument InitQuery(BsonDocument query)
        {

            if (query == null)
            {
                //当查询为空时 附加恒真的条件 类似SQL：1=1的语法  
                query = new BsonDocument();
            }
            return query;
        }

        /// <summary>  
        /// 初始化排序条件  主要当条件为空时 会默认以ObjectId递增的一个排序  
        /// </summary>  
        /// <param name="sortBy"></param>  
        /// <param name="sortByName"></param>  
        /// <returns></returns>  
        private BsonDocument InitSortBy(BsonDocument sortBy, string sortByName)
        {
            if (sortBy == null)
            {
                //默认ObjectId 递增  
                sortBy = new BsonDocument(sortByName, -1);
            }
            return sortBy;
        }

        private BsonDocument InitUpdateDocument(BsonDocument update, string indexName)
        {
            if (update == null)
            {
                update = new BsonDocument("$inc", new BsonDocument(indexName, 0));
            }
            return update;
        }
        #endregion
    }
}
