using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Web;

namespace BowlingScoreSheet
{
    public class Mongo : IPersistence
    {
        /// <summary>
        /// Adds a insertedAt field to the document and saves the document to the database.
        /// </summary>
        /// <param name="json"></param>
        public void Save(string json)
        {
            //string quoted = HttpUtility.JavaScriptStringEncode(json);
            //System.Console.WriteLine(quoted);
            var app = MyApp.getInstance();
            var client = new MongoClient(app.GetConfig("/config/database/mongo/connection_string"));
            var database = client.GetDatabase(app.GetConfig("/config/database/mongo/database"));
            var collection = database.GetCollection<BsonDocument>(app.GetConfig("/config/database/mongo/collection"));

            var jsonReader = new JsonReader(json);
            var serializer = new BsonArraySerializer();
            //It seems impossible to add a Json-Array without this?
            BsonArray bsonArray = serializer.Deserialize(BsonDeserializationContext.CreateRoot(jsonReader));

            var document = new BsonDocument();
            BsonDateTime bdt = BsonDateTime.Create(DateTime.Now.ToUniversalTime());
            document.Add("insertedAt", bdt);
            document.Add("aGame", bsonArray);
            collection.InsertOne(document); 
        }
        
        public string[] Get(DateTime dtStart, DateTime dtEnd)
        {
            var client = new MongoClient();

            var builder = Builders<BsonDocument>.Filter;
            var app = MyApp.getInstance();
            IMongoDatabase db = client.GetDatabase(app.GetConfig("/config/database/mongo/database"));
            var collection = db.GetCollection<BsonDocument>(app.GetConfig("/config/database/mongo/collection"));

            DateTime dt1 = DateTime.Now.ToUniversalTime().AddHours(-2);
            DateTime dt2 = DateTime.Now.ToUniversalTime().AddHours(2);
            var filter = builder.Lte("insertedAt", dt2) & builder.Gte("insertedAt", dt1);
            //execute
            var cursor = collection.Find(filter).Limit(
                Convert.ToInt32(app.GetConfig("/config/database/mongo/max_documents_limit")));
            var li = cursor.ToList<BsonDocument>();
            string[] retVal = new string[li.Count];
            int i = 0;
            foreach (BsonDocument aDoc in li)
            {   
                retVal[i] = aDoc.ToJson();
                i++;
            }
            return retVal;
        }

        /// <summary>
        /// Get a game.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Json string or "" for an invalid id.</returns>
        public string Get(string id)
        {
            return "";
        }

        /// <summary>
        /// Gets all games with the players.
        /// </summary>
        /// <param name="players"></param>
        /// <returns></returns>
        public string[] Get(string[] players)
        {
            return new string[] { };
        }
    }
}
