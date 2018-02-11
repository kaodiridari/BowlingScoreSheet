using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreSheet
{
    class Mongo : IPersistence
    {
        public void Save(string json)
        {
            System.Console.WriteLine(json);
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

        /// <summary>
        /// Get all game-ids in the given intervall.<br>
        /// The dates are stored in UTC. Be shure to convert the parameters to UTC
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns>An array of ids. May be empty but not null.</returns>
        public string[] Get(DateTime dtStart, DateTime dtEnd)
        {
            return new string[] { };
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
