using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using BowlingScoreSheet;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Runtime.Serialization; //needs System.Runtime.Serialization.dll "Verweis"
using System.IO;
using System.Runtime.Serialization.Json;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using System.Windows;
using System.Threading;

namespace TestBowling
{
    [DataContract]
    internal class Person
    {
        [DataMember]
        internal string name;

        [DataMember]
        internal int age;
    }

    public class BowlingScoreControlModelFake : IBowlingScoreControlModel
    {
        public int[] frames = new int[10];
        public int total = 0;
        public bool isGameOver = false;
        public string[] balls;

        public BowlingScoreControlModelFake()
        {
            balls = new string[21];
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i] = "";
            }
        }

        public bool GameOver
        {
            get { return true; }
            set { }
        }

        public void SetFrameScore(int frameNumber, int score)
        {
            frames[frameNumber] = score;
        }

        public void SetTotalScore(int score)
        {
            total = score;
        }

        public void SetBall(int frame, int ball, int pins)
        {
            int i = -1111;
            i = (2 * (frame + 1) - 1) + (ball - 1) - 1;
            balls[i] = Convert.ToString(pins);
        }

        public void Clear()
        {
            //Nothing to test.
        }

        public string GetId()
        {
            throw new NotImplementedException();
        }
    }

    public class BowlingDialogFake : IBowlingDialog
    {   
        private BowlingDialogModel m_bowlingDialogModel;
        private BowlingDialogControler m_bowlingDialogControler;
        private BowlingScoreControlModel[] m_BowlingScoreControlModels;
        private BowlingScoreControlControler[] m_BowlingScoreControlControlers;
        private string[] m_players;

        public void SetBowlingDialogControler(BowlingDialogControler bowlingDialogControler)
        {
            m_bowlingDialogControler = bowlingDialogControler;
        }

        public void SetBowlingDialogModel(BowlingDialogModel bowlingDialogModel)
        {
            m_bowlingDialogModel = bowlingDialogModel;
        }

        public void SetBowlingScoreControlControlers(BowlingScoreControlControler[] bowlingScoreControlControlers)
        {
            m_BowlingScoreControlControlers = bowlingScoreControlControlers;
        }

        public void SetBowlingScoreControlModels(BowlingScoreControlModel[] bowlingScoreControlModels)
        {
            m_BowlingScoreControlModels = bowlingScoreControlModels;
        }

        public void SetPlayers(string[] players)
        {
            m_players = players;
        }

        internal BowlingDialogControler GetBowlingDialogControler()
        {
            return m_bowlingDialogControler;
        }

        internal BowlingDialogModel GetBowlingDialogModel()
        {
            return m_bowlingDialogModel;
        }

        internal BowlingScoreControlControler[] GetBowlingScoreControlControlers()
        {
            return m_BowlingScoreControlControlers;
        }

        internal BowlingScoreControlModel[] GetBowlingScoreControlModels()
        {
            return m_BowlingScoreControlModels;
        }
    }

    [TestClass]
    public class BowlingScoreTests
    {
        [TestMethod]
        public void TestJustAnotherBallThrown0()
        {
            //two strikes, 5 spares
            int[] example = new int[]
            {5,2,5,3,2,1,0,5,6,2,1,8,4,0,5,4,2,3,7,1};
            var m = new BowlingScoreControlModelFake();
            var bowling = new BowlingScoreControlControler(m);

            //play
            int loops = 0;
            foreach (int pins in example)
            {
                loops++;
                bowling.justAnotherBallThrown(pins);
                if (m.isGameOver)
                    break;
            }

            Assert.IsTrue(m.isGameOver);
            Assert.AreEqual(loops, example.Length);

            //check
            int[] frameScores = new int[]
                {7,15,18,23,31,40,44,53,58,66};
            Assert.AreEqual(m.frames.Length, 10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(frameScores[i], m.frames[i]);
            }
            Assert.AreEqual(66, m.total);
            Assert.IsTrue(m.isGameOver);

            //balls?
            for (int i = 0; i < example.Length; i++)
            {
                Assert.IsTrue(example[i] == Convert.ToInt32(m.balls[i]));

            }
        }

        [TestMethod]
        public void TestJustAnotherBallThrown1()
        {
            //two strikes, 5 spares
            int[] example1 = new int[]
            {1,4,4,5,6,4,5,5,10,0,1,7,3,6,4,10,2,8,6};    //19 balls

            string[] exampleBalls = new string[]
                {"1","4","4","5","6","4","5","5","10","","0","1","7","3","6","4","10","","2","8","6"};
            var m = new BowlingScoreControlModelFake();
            var bowling = new BowlingScoreControlControler(m);

            //play
            int loops = 0;
            foreach (int pins in example1)
            {
                loops++;
                bowling.justAnotherBallThrown(pins);
                if (m.isGameOver && loops < example1.Length)
                {
                    Assert.Fail("loops: " + loops);
                }
            }

            //check
            int[] frameScores = new int[]
                {5,14,29,49,60,61,77,97,117,133};
            Assert.AreEqual(m.frames.Length, 10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(frameScores[i], m.frames[i]);
            }
            Assert.AreEqual(m.total, 133);
            Assert.IsTrue(m.isGameOver);

            //balls?
            for (int i = 0; i < exampleBalls.Length; i++)
            {
                Assert.IsTrue(exampleBalls[i].Equals(m.balls[i].Trim()));
            }
        }

        [TestMethod]
        public void TestJustAnotherBallThrown2()
        {
            //only strikes
            int[] example2 = new int[]
            {10,10,10,10,10,10,10,10,10,10,10,10};
            string[] exampleBalls = new string[]
                {"10","","10","","10","","10","","10","","10","","10","","10","","10","","10","10","10"};
            var m = new BowlingScoreControlModelFake();
            var bowling = new BowlingScoreControlControler(m);

            //play
            int loops = 0;
            foreach (int pins in example2)
            {
                loops++;
                bowling.justAnotherBallThrown(pins);
                if (m.isGameOver && loops < example2.Length)
                {
                    Assert.Fail("loops: " + loops);
                }
            }

            //check
            int[] frameScores = new int[]
                {30,60,90,120,150,180,210,240,270,300};

            Assert.AreEqual(m.frames.Length, 10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(frameScores[i], m.frames[i]);
            }
            Assert.AreEqual(m.total, 300);
            Assert.IsTrue(m.isGameOver);

            //balls?
            for (int i = 0; i < exampleBalls.Length; i++)
            {
                Assert.IsTrue(exampleBalls[i].Equals(m.balls[i].Trim()));
            }
        }
    }

    [TestClass]
    public class ThisAndThatTest
    {
        [TestMethod]
        public void TestPlayersInitials1()
        {
            var players = new string[] { "Peter Müller", "Heiner Müller", "Üzgül Özalü" };
            var initials = ThisAndThat.playersInitials(players);
            
            Assert.AreEqual("P.M.", initials[0]);
            Assert.AreEqual("H.M.", initials[1]);
            Assert.AreEqual("Ü.Ö.", initials[2]); 
            
        }

        [TestMethod]
        public void TestPlayersInitials2()
        {
            var players = new string[] { "Peter Müller", "Peter Müller", "Peter Müller" };
            var initials = ThisAndThat.playersInitials(players);
           
            Assert.AreEqual("P.M.", initials[0]);
            Assert.AreEqual("P.M.(1)", initials[1]);
            Assert.AreEqual("P.M.(2)", initials[2]);            
        }

        [TestMethod]
        public void TestPlayersInitials3()
        {
            var players = new string[] 
            { "Peter Müller", "Heiner Müller", "Üzgül Özalü" , "Peter Müller",
                "Piet Mondrian", "T. Coraghessan Boyle", "James T. Kirk", "J. R. Ewing",
                "Karl-Heinz Schlüter", "Thomas Christian Bauer",
                "Hatschi Halef Omar Ibn Ben Gosarar"};
            var initials = ThisAndThat.playersInitials(players);
            
            Assert.AreEqual("P.M.", initials[0]);
            Assert.AreEqual("H.M.", initials[1]);
            Assert.AreEqual("Ü.Ö.", initials[2]);
            Assert.AreEqual("P.M.(1)", initials[3]);
            Assert.AreEqual("P.M.(2)", initials[4]);
            Assert.AreEqual("T.C.B.", initials[5]);
            Assert.AreEqual("J.T.K.", initials[6]);
            Assert.AreEqual("J.R.E.", initials[7]);
            Assert.AreEqual("K.S.", initials[8]);
            Assert.AreEqual("T.C.B.(1)", initials[9]);
            Assert.AreEqual("H.H.O.I.B.G.", initials[10]);            
        }

        [TestMethod]
        public void TestJsonizeDeJsonize()
        {
            Person p = new Person();
            p.name = "John";
            p.age = 42;

            string json = ThisAndThat.Jsonize<Person>(p);
            Console.Write("\n" + json + "\n");

            Person p2 = ThisAndThat.DeJsonize<Person>(json);
            Console.WriteLine("name: " + p2.name);
            Console.WriteLine("age: " + p2.age);
            Assert.AreEqual(p.name, p2.name);
            Assert.AreEqual(p.age, p2.age);
        }

        [TestMethod]
        public void TestJsonizeDeJsonizeList()
        {
            Person p1 = new Person();
            p1.name = "John";
            p1.age = 42;

            Person p2 = new Person();
            p2.name = "Peter";
            p2.age = 22;

            List<Person> persons = new List<Person>();
            persons.Add(p1);
            persons.Add(p2);

            string json = ThisAndThat.Jsonize< List<Person> >(persons);
            Console.Write("\n" + json + "\n");

            List < Person > dejsonList = ThisAndThat.DeJsonize<List<Person>>(json);
            Console.WriteLine("name: " + dejsonList[0].name);
            Console.WriteLine("age: " + dejsonList[0].age);
            Assert.AreEqual(dejsonList[0].name, p1.name);
            Assert.AreEqual(dejsonList[0].age, p1.age);
        }

        [TestMethod]
        public void TestConfigFile()
        {
            ThisAndThat.LoadConfigFile("testconfig.xml");
        }
    }

    public class MyAppFake : IMyApp
    {
        public string[] Players
        {
            get
            {
                return new string[] { "Peter Müller", "Heiner Müller", "Üzgül Özalü" }; 
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void SetConfigFile(string file)
        {
            throw new NotImplementedException();
        }

        public void SetPersistence(IPersistence p)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class BowlingDialogTests
    {
        private BowlingScoreControlControler[] bsccs;
        private BowlingScoreControlModel[] bscm;
        private BowlingDialogControler bdc;
        private BowlingDialogModel bdm;

        [TestInitialize()]
        public void Initialize()
        {
            var bdf = new BowlingDialogFake();
            var cbd = new ConfigBowlingDialog(bdf, new MyAppFake());
            bdc = bdf.GetBowlingDialogControler();
            bdm = bdf.GetBowlingDialogModel();
            bsccs = bdf.GetBowlingScoreControlControlers();
            bscm = bdf.GetBowlingScoreControlModels();
        }

        [TestMethod]
        public void TestInitialisation()
        {            
            Assert.AreEqual(3, bsccs.Length);           
            Assert.AreEqual(3, bscm.Length);
        }

        [TestMethod]
        public void TestMultipleControls()
        {
            int[] example  = new int[] {5,2,5,3,2,1,0,5,6,2,1,8,4,0,5,4,2,3,7,1};
            int[] example1 = new int[] {1,4,4,5,6,4,5,5,10,0,1,7,3,6,4,10,2,8,6};
            int[] example2 = new int[] {10,10,10,10,10,10,10,10,10,10,10,10};

            int[][] examples = new int[][] { example, example1, example2 };

            Console.WriteLine("blah");
            string[] ids = bdc.GetPlayerIds();
            Assert.AreEqual(3, ids.Length);
            List<string> li = new List<string>(ids);
            Assert.IsTrue(li.Contains("P.M."));
            Assert.IsTrue(li.Contains("H.M."));
            Assert.IsTrue(li.Contains("Ü.Ö."));
            #region play
            //Play
            //Set player to P.M. and throw a ball.
            //Set player to Ü.Ö. and throw a ball.
            //Set player to H.M. and throw a ball.
            int thrownBalls = 0;
            int maxIndex = Math.Max(example.Length, Math.Max(example1.Length, example2.Length));
            for (int i = 0; i < maxIndex; i++)
            {
                for (int j = 0; j < examples.Length /*3*/ ; j++)
                {
                    if (i < examples[j].Length)
                    {
                        bdc.SetActiveControl(ids[j]);
                        bdc.JustAnotherBallThrown(examples[j][i]);
                        thrownBalls++;
                    }
                }
            }
            #endregion

            #region check
            //check the results 
            Assert.AreEqual(example.Length + example1.Length + example2.Length, thrownBalls); //really all?
            {
                BowlingScoreControlModel m = null; 
                for (int k = 0; k < bscm.Length; k++)
                {
                    if (bscm[k].GetId().Equals("P.M."))
                    {
                        m = bscm[k];
                        break;
                    }
                }
                Assert.IsNotNull(m);

                int[] frameScores = new int[]
                    {7,15,18,23,31,40,44,53,58,66};
                Assert.AreEqual(m.FrameScore.Length, 10);
                
                for (int i = 0; i < 10; i++)
                {
                    Assert.AreEqual(frameScores[i], Convert.ToInt32(m.FrameScore[i]));
                }
                
                Assert.IsTrue(m.GameOver);
            }

            {
                BowlingScoreControlModel m = null;
                for (int k = 0; k < bscm.Length; k++)
                {
                    if (bscm[k].GetId().Equals("H.M."))
                    {
                        m = bscm[k];
                        break;
                    }
                }
                Assert.IsNotNull(m);
                int[] frameScores = new int[]
                   {5,14,29,49,60,61,77,97,117,133};
                Assert.AreEqual(m.FrameScore.Length, 10);
                for (int i = 0; i < 10; i++)
                {
                    Assert.AreEqual(frameScores[i], Convert.ToInt32(m.FrameScore[i]));
                }
                
                Assert.IsTrue(m.GameOver);
            }

            {
                BowlingScoreControlModel m = null;                
                for (int k = 0; k < bscm.Length; k++)
                {
                    if (bscm[k].GetId().Equals("Ü.Ö."))
                    {
                        m = bscm[k];
                        break;
                    }
                }
                int[] frameScores = new int[]
                    {30,60,90,120,150,180,210,240,270,300};
                Assert.AreEqual(m.FrameScore.Length, 10);
                for (int i = 0; i < 10; i++)
                {
                    Assert.AreEqual(frameScores[i], Convert.ToInt32(m.FrameScore[i]));
                }                 
                Assert.IsTrue(m.GameOver);
            }
            #endregion
        }       
    }

    [TestClass]
    public class MongoTests
    {   
        [TestMethod]
        //http://mongodb.github.io/mongo-csharp-driver/2.4/getting_started/quick_tour/
        public void TestTryIt()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("foo");

            var collection = database.GetCollection<BsonDocument>("bar");
            var document = new BsonDocument
                                {
                    { "name", "MongoDB" },
                    { "type", "Database" },
                    { "count", 1 },
                    { "info", new BsonDocument
                        {
                            { "x", 203 },
                            { "y", 102 }
                        }}
                };
            collection.InsertOne(document);

            var filter = Builders<BsonDocument>.Filter.Eq("type", "Database");
            var foundDocument = collection.Find(filter).First();
            BsonElement value;
            foundDocument.TryGetElement("type", out value);
            string vs = value.ToString();
            Console.WriteLine("value:" + vs);
        }

        [TestMethod]
        public void TestJsonBson()
        {
            Person p = new Person();
            p.name = "John";
            p.age = 42;

            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Person));
            ser.WriteObject(stream1, p);     //Verweis auf System.Xml

            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            string jsonString = sr.ReadToEnd();
            Console.Write("JSON form of Person object: ");
            Console.WriteLine(jsonString);

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("foo");

            var collection = database.GetCollection<BsonDocument>("persons");
            var document = BsonDocument.Parse(jsonString);
            collection.InsertOne(document);

            var filter = Builders<BsonDocument>.Filter.Eq("age", 42);
            var foundDocument = collection.Find(filter).First();
            BsonElement value;
            foundDocument.TryGetElement("age", out value);
            int age = value.Value.ToInt32();
            Console.WriteLine("value:" + age);
            Assert.AreEqual(42, age);
        }

        [TestMethod]
        public void TestJsonBsonList()
        {
            Person p1 = new Person();
            p1.name = "John";
            p1.age = 42;

            Person p2 = new Person();
            p2.name = "Peter";
            p2.age = 22;

            List<Person> persons = new List<Person>();
            persons.Add(p1);
            persons.Add(p2);

            string json = ThisAndThat.Jsonize<List<Person>>(persons);
            var jsonReader = new JsonReader(json);
            var serializer = new BsonArraySerializer();
            BsonArray bsonArray = serializer.Deserialize(BsonDeserializationContext.CreateRoot(jsonReader));
            
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("personList"); 
            var collection = database.GetCollection<BsonDocument>("persons");
            var document = new BsonDocument();
            DateTime dt = DateTime.Now.ToUniversalTime();
            BsonDateTime bdt = BsonDateTime.Create(dt);
            document.Add("insertedAt", bdt);
            document.Add("blah", bsonArray);
            collection.InsertOne(document);
            System.Console.WriteLine("dt: " + dt);
            System.Console.WriteLine("bdt: " + bdt);
            //find 2018-02-09T23:40:02.161Z

        }

        [TestMethod]
        public void TestJsonBsonListFindGt()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("personList");
            var collection = database.GetCollection<BsonDocument>("persons");
            var tosearch = DateTime.Parse("2018-02-10T01:39:57.296Z");
            tosearch = tosearch.AddYears(-5).ToUniversalTime();
            //find 2018-02-09T23:40:02.161Z
            var filter = Builders<BsonDocument>.Filter.Gt("insertedAt", tosearch);   //das geht
            var foundCollection = collection.Find(filter);
            if (foundCollection.Count() != 0)
            {
                var foundDocument = collection.Find(filter).First();
                BsonElement value;
                foundDocument.TryGetElement("blah", out value);
                string json = value.ToJson();
            }
        }

        [TestMethod]
        public void TestJsonBsonListFindEq()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("personList");
            var collection = database.GetCollection<BsonDocument>("persons");
            var tosearch = DateTime.Parse("2018-02-10T01:35:28.639Z");
            tosearch = tosearch.ToUniversalTime();
            //find 2018-02-09T23:40:02.161Z
            var filter = Builders<BsonDocument>.Filter.Eq("insertedAt", tosearch);   //das geht
            var foundCollection = collection.Find(filter);
            if (foundCollection.Count() != 0)
            {
                var foundDocument = collection.Find(filter).First();
                BsonElement value;
                foundDocument.TryGetElement("blah", out value);
                string json = value.ToJson();
                BsonElement time;
                foundDocument.TryGetElement("insertedAt", out time);
                
                Console.WriteLine("Mongo: " + time.ToString());
                var universial = time.Value.ToUniversalTime();
                Console.WriteLine("universial:" + universial);
                Console.WriteLine("universial.Ticks:" + universial.Ticks);
            }
        }

        [TestMethod]
        public void TestJsonBsonListFindEq2()
        {
            var tosearch = DateTime.Now;
            tosearch = tosearch.ToUniversalTime();
            {
                Person p1 = new Person();
                p1.name = "John";
                p1.age = 42;

                Person p2 = new Person();
                p2.name = "Peter";
                p2.age = 22;

                List<Person> persons = new List<Person>();
                persons.Add(p1);
                persons.Add(p2);

                string json = ThisAndThat.Jsonize<List<Person>>(persons);
                var jsonReader = new JsonReader(json);
                var serializer = new BsonArraySerializer();
                BsonArray bsonArray = serializer.Deserialize(BsonDeserializationContext.CreateRoot(jsonReader));

                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("personList");
                var collection = database.GetCollection<BsonDocument>("persons");
                var document = new BsonDocument();
                
                BsonDateTime bdt = BsonDateTime.Create(tosearch);
                document.Add("insertedAt", bdt);
                document.Add("blah", bsonArray);
                collection.InsertOne(document);
                System.Console.WriteLine("dt: " + tosearch);
                System.Console.WriteLine("bdt: " + bdt);
            }
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("personList");
                var collection = database.GetCollection<BsonDocument>("persons");
                
                //find 2018-02-09T23:40:02.161Z
                var filter = Builders<BsonDocument>.Filter.Eq("insertedAt", tosearch);   //das geht
                var foundCollection = collection.Find(filter);
                if (foundCollection.Count() != 0)
                {
                    var foundDocument = collection.Find(filter).First();
                    BsonElement be;
                    foundDocument.TryGetElement("blah", out be);
                    string json = be.Value.ToJson();
                    Console.WriteLine("json: " + json);
                    var li = ThisAndThat.DeJsonize<List<Person>>(json);
                    Assert.AreEqual(2, li.Count);
                    BsonElement time;
                    foundDocument.TryGetElement("insertedAt", out time);

                    Console.WriteLine("Mongo: " + time.ToString());
                    var universial = time.Value.ToUniversalTime();
                    Console.WriteLine("universial:" + universial);
                    Console.WriteLine("universial.Ticks:" + universial.Ticks);
                } else
                {
                    Assert.Fail("Nothing found. Date: " + tosearch);
                }
            }
            
        }

        [TestMethod]
        public void TestBowlingScoreModelSerialization()
        {
            BowlingScoreControlModel m = new BowlingScoreControlModel("Elmer Fudd", "E.F.");
            
            //two strikes, 5 spares
            int[] example = new int[]
            {5,2,5,3,2,1,0,5,6,2,1,8,4,0,5,4,2,3,7,1};            
            var bowling = new BowlingScoreControlControler(m);

            //play
            int loops = 0;
            foreach (int pins in example)
            {
                loops++;
                bowling.justAnotherBallThrown(pins);
                if (m.GameOver)
                    break;
            }

            Assert.IsTrue(m.GameOver);
            Assert.AreEqual(loops, example.Length);

            //check
            int[] frameScores = new int[]
                {7,15,18,23,31,40,44,53,58,66};
            Assert.AreEqual(m.FrameScore.Length, 10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(frameScores[i], Convert.ToInt32(m.FrameScore[i]));
            }            

            //balls?
            for (int i = 0; i < example.Length; i++)
            {
                Assert.IsTrue(example[i] == Convert.ToInt32(m.Balls[i]));

            }

            string json = ThisAndThat.Jsonize<BowlingScoreControlModel>(m);
            Console.WriteLine(json);

            BowlingScoreControlModel modelBackAgain = ThisAndThat.DeJsonize<BowlingScoreControlModel>(json);
           
            Assert.AreEqual(modelBackAgain.FrameScore.Length, 10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(frameScores[i], Convert.ToInt32(modelBackAgain.FrameScore[i]));
            }

            //balls?
            for (int i = 0; i < example.Length; i++)
            {
                Assert.IsTrue(example[i] == Convert.ToInt32(modelBackAgain.Balls[i]));

            }
            Assert.AreEqual("Elmer Fudd", modelBackAgain.Player);
            Assert.AreEqual("E.F.", modelBackAgain.PlayersInitials);
        }

        [TestMethod]
        public void TestMongoBowlingSave()
        {
            #region prepare
            BowlingScoreControlModel m = new BowlingScoreControlModel("Elmer Fudd", "E.F.");

            //two strikes, 5 spares
            int[] example = new int[]
            {5,2,5,3,2,1,0,5,6,2,1,8,4,0,5,4,2,3,7,1};
            var bowling = new BowlingScoreControlControler(m);

            //play
            int loops = 0;
            foreach (int pins in example)
            {
                loops++;
                bowling.justAnotherBallThrown(pins);
                if (m.GameOver)
                    break;
            }
            string json = ThisAndThat.Jsonize<BowlingScoreControlModel>(m);
            Console.WriteLine(json);
            #endregion

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("bowling_games"); 
            var collection = database.GetCollection<BsonDocument>("game");

            var document = new BsonDocument();
            DateTime dt = DateTime.Now.ToUniversalTime();
            BsonDateTime bdt = BsonDateTime.Create(dt);
            document.Add("insertedAt", bdt);
                       
            collection.InsertOne(document);

            
        }

        [TestMethod]
        public void TestMongoBowlingSaveGet()
        {
            DateTime tosearch;
            string[] names = new string[] { "Elmer Fudd", "Bugs Bunny", "Daffy Duck" };
            var initials = ThisAndThat.playersInitials(names);
            {
                #region prepare
                BowlingScoreControlModel m1 = new BowlingScoreControlModel(names[0], initials[0]);
                BowlingScoreControlModel m2 = new BowlingScoreControlModel(names[1], initials[1]);
                BowlingScoreControlModel m3 = new BowlingScoreControlModel(names[2], initials[2]);
                BowlingScoreControlModel[] ms = new BowlingScoreControlModel[] { m1, m2, m3 };
                //two strikes, 5 spares
                int[] example = new int[] { 5, 2, 5, 3, 2, 1, 0, 5, 6, 2, 1, 8, 4, 0, 5, 4, 2, 3, 7, 1 };
                int[] example1 = new int[] { 1, 4, 4, 5, 6, 4, 5, 5, 10, 0, 1, 7, 3, 6, 4, 10, 2, 8, 6 };
                int[] example2 = new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
                List<int[]> li = new List<int[]>();
                li.Add(example);
                li.Add(example1);
                li.Add(example2);
                var jsons = new List<string>();

                for (int i = 0; i < ms.Length; i++)
                {
                    var bowling = new BowlingScoreControlControler(ms[i]);

                    //play
                    int loops = 0;
                    foreach (int pins in li[i])
                    {
                        loops++;
                        bowling.justAnotherBallThrown(pins);
                        if (ms[i].GameOver)
                            break;
                    }


                    string json1 = ThisAndThat.Jsonize<BowlingScoreControlModel>(ms[i]);
                    Console.WriteLine(json1);
                    jsons.Add(json1);
                }

                #endregion

                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("bowling_games");
                var collection = database.GetCollection<BsonDocument>("game");

                string json = ThisAndThat.Jsonize<BowlingScoreControlModel[]>(ms);
                var jsonReader = new JsonReader(json);
                var serializer = new BsonArraySerializer();
                BsonArray bsonArray = serializer.Deserialize(BsonDeserializationContext.CreateRoot(jsonReader));

                var document = new BsonDocument();
                tosearch = DateTime.Now.ToUniversalTime();
                BsonDateTime bdt = BsonDateTime.Create(tosearch);
                document.Add("insertedAt", bdt);
                document.Add("aGame", bsonArray);
                collection.InsertOne(document);
                System.Console.WriteLine("dt: " + tosearch);
                System.Console.WriteLine("bdt: " + bdt);
            } 
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("bowling_games");
                var collection = database.GetCollection<BsonDocument>("game");
                
                var filter = Builders<BsonDocument>.Filter.Eq("insertedAt", tosearch);   //das geht
                var foundCollection = collection.Find(filter);
                if (foundCollection.Count() != 0)
                {
                    var foundDocument = collection.Find(filter).First();
                    BsonElement be;
                    foundDocument.TryGetElement("aGame", out be);
                    string json = be.Value.ToJson();
                    Console.WriteLine("json: " + json);
                    var li = ThisAndThat.DeJsonize<List<BowlingScoreControlModel>>(json);
                    Assert.AreEqual(3, li.Count);
                    for (int i = 0; i < names.Length; i++)
                    {
                        Assert.IsTrue(li.Exists(m => m.Player.Equals(names[i])));
                    }
                    
                    BsonElement time;
                    foundDocument.TryGetElement("insertedAt", out time);

                    Console.WriteLine("Mongo: " + time.ToString());
                    var universial = time.Value.ToUniversalTime();
                    Console.WriteLine("universial:" + universial);
                    Console.WriteLine("universial.Ticks:" + universial.Ticks);
                    
                } else
                {
                    Assert.Fail();
                }
                
            }

        }
       
    }

    [TestClass]
    public class MongoPersistenceTests
    {
        private string testconfig = "testconfig.xml";
        private DateTime afterFirst;
        private DateTime afterSecond;

        [TestInitialize()]
        public void Initialize()
        {
            Cleanup();
            //write values to MongoDb
            string json1 = "[{\"m_FrameScore\":[\"3\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"3\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Peter Handke\",\"m_playersId\":\"P.H.\"},{\"m_FrameScore\":[\"6\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"6\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Günther Grass\",\"m_playersId\":\"G.G.\"},{\"m_FrameScore\":[\"8\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"8\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Siegfried Lenz\",\"m_playersId\":\"S.L.\"},{\"m_FrameScore\":[\"9\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"9\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Heinrich Böll\",\"m_playersId\":\"H.B.\"},{\"m_FrameScore\":[\"10\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"10\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Wolfgang Borchert\",\"m_playersId\":\"W.B.\"},{\"m_FrameScore\":[\"0\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"0\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Sten Nadolny\",\"m_playersId\":\"S.N.\"},{\"m_FrameScore\":[\"9\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"9\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Wolf Haas\",\"m_playersId\":\"W.H.\"}]";
            string json2 = "[{\"m_FrameScore\":[\"4\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"4\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Elmer Fudd\",\"m_playersId\":\"E.F.\"},{\"m_FrameScore\":[\"7\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"7\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Bugs Bunny\",\"m_playersId\":\"B.B.\"},{\"m_FrameScore\":[\"9\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"9\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Daffy Duck\",\"m_playersId\":\"D.D.\"},{\"m_FrameScore\":[\"10\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_ball\":[\"10\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"m_gameOver\":false,\"m_player\":\"Porky Pig\",\"m_playersId\":\"P.P.\"}]";
        
            
            MyApp app = MyApp.getInstance();
            app.SetPersistence(new Mongo());
            app.SetConfigFile(testconfig);

            app.GetPersistence().Save(json1);
            afterFirst = DateTime.Now.ToUniversalTime();
            Thread.Sleep(2000);
            app.GetPersistence().Save(json2);
            afterSecond = DateTime.Now.ToUniversalTime();
            Console.WriteLine("Initialized");           
        }

        //[TestMethod]
        //public void TestGetTimeintervallProbiers()
        //{
        //    //http://mongodb.github.io/mongo-csharp-driver/2.4/reference/driver/definitions/ 
        //    var client = new MongoClient();

        //    var builder = Builders<BsonDocument>.Filter;
        //    var app = MyApp.getInstance();
        //    IMongoDatabase db = client.GetDatabase(app.GetConfig("/config/database/mongo/database"));
        //    var collection = db.GetCollection<BsonDocument>(app.GetConfig("/config/database/mongo/collection"));

        //    DateTime dt1 = DateTime.Now.ToUniversalTime().AddHours(-2);
        //    DateTime dt2 = DateTime.Now.ToUniversalTime().AddHours(2);
        //    var filter = builder.Lte("insertedAt", dt2) & builder.Gte("insertedAt", dt1);
        //    //execute
        //    var cursor = collection.Find(filter);
        //    Assert.AreEqual(2, cursor.Count());

        //    filter = builder.Gt("insertedAt", this.afterFirst) & builder.Lte("insertedAt", this.afterSecond);
        //    cursor = collection.Find(filter);
        //    Assert.AreEqual(1, cursor.Count());
        //    var doc2 = cursor.Single();
        //    //m_player\":\"Porky Pig\"
        //    var val = doc2.GetElement("aGame");
        //    Console.WriteLine(val.ToJson().ToString());            
        //}

        [TestMethod]
        public void TestGetTimeintervall()
        {
            //http://mongodb.github.io/mongo-csharp-driver/2.4/reference/driver/definitions/ 
            var app = MyApp.getInstance();
            string[] ret = app.GetPersistence().Get(this.afterFirst, this.afterSecond);
            Assert.AreEqual(2, ret.Length);
        }

        [TestMethod]
        public void TestGetPlayers()
        {
            
        }

        [TestCleanup()]
        public void Cleanup()
        {
            //delete all
            var navi = ThisAndThat.LoadConfigFile(testconfig);
            var client = new MongoClient(navi.SelectSingleNode("/config/database/mongo/connection_string").Value);
            var database = client.GetDatabase(navi.SelectSingleNode("/config/database/mongo/database").Value);
            //This removes also the database ...
            database.DropCollection(navi.SelectSingleNode("/config/database/mongo/collection").Value);
            Console.WriteLine("Cleaned up.");
        }
    }
}