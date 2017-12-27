using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BowlingScoreSheet;

namespace TestBowling
{
    public class BowlingDialogModelFake : IBowlingScoreControlModel
    {
        public int[] frames = new int[10];
        public int total = 0;
        public bool isGameOver = false;
        public string[] balls;

        public BowlingDialogModelFake()
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
            i = (2 * (frame+1) - 1) + (ball - 1) - 1;
            balls[i] = Convert.ToString(pins);            
        }

        public void Clear()
        {
            //Nothing to test.
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestJustAnotherBallThrown0()
        {   
            //two strikes, 5 spares
            int[] example = new int[]
            {5,2,5,3,2,1,0,5,6,2,1,8,4,0,5,4,2,3,7,1};
            var m = new BowlingDialogModelFake();
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
            var m = new BowlingDialogModelFake();
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
            Assert.AreEqual(m.total,133);
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
            int[] example1 = new int[]
            {10,10,10,10,10,10,10,10,10,10,10,10};
            string[] exampleBalls = new string[]
                {"10","","10","","10","","10","","10","","10","","10","","10","","10","","10","10","10"};
            var m = new BowlingDialogModelFake();
            var bowling = new BowlingScoreControlControler(m);

            //play
            int loops = 0;
            foreach (int pins in example1)
            {
                loops++;
                bowling.justAnotherBallThrown(pins);
                if (m.isGameOver && loops < example1.Length)
                {
                    Assert.Fail("loops: " + loops );
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
}
