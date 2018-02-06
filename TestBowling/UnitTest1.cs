﻿using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using BowlingScoreSheet;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Runtime.Serialization; //needs System.Runtime.Serialization.dll "Verweis"
using System.IO;
using System.Runtime.Serialization.Json;

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
            var document = BsonDocument.Parse(json);
            collection.InsertOne(document); 
        }
    }
}