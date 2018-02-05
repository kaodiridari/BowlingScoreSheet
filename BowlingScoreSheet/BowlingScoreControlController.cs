using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreSheet
{
    /// <summary>
    /// 
    /// </summary>
    internal class BowlingFrame
    {
        public List<int> balls = new List<int>(2);

        //Set it to 1 for a spare-frame and decrement it with the next ball.
        public int isSpare = 0;

        //Set it to 2 for a strike-frame and decrement it with the next two balls.
        public int isStrike = 0;

        //The score of this frame.  
        public int score = 0; 
        
        public void addBall(int pins)
        {
            balls.Add(pins);
            score += pins;
            if (pins == 10 && balls.Count == 1)
            {
                isStrike += 2;
            }
            if (balls.Count == 2 && (balls[0] + balls[1] == 10))
            {
                isSpare += 1;
            } 
        }       
    }

    /// <summary>
    /// The controller for a single supercalifragilisticexpialidocious bowling control.
    /// </summary>
    public class BowlingScoreControlControler
    {
        private BowlingFrame[] m_frames;

        private int m_frame = -1; //the index of the current frame

        private IBowlingScoreControlModel m_bowlingScoreControlModel;         

        public BowlingScoreControlControler(IBowlingScoreControlModel bowlingScoreControlModel)
        {
            this.m_bowlingScoreControlModel = bowlingScoreControlModel;
            m_frames = new BowlingFrame[10];            
        }

        internal void Clear()
        {
            m_frames = new BowlingFrame[10];
            m_frame = -1;
            m_bowlingScoreControlModel.Clear();
        }

        /// <summary>
        /// About frames, strike or spare we do here care.
        /// </summary>
        /// <param name="pins">Score of the current ball.</param>
        public void justAnotherBallThrown(int pins)
        {
            var currentFrame = getFrame();
            
            //spares and strikes in previous frames           
            if (m_frame > 0)   //for first ball there is no previous
            {
                //add to previous spare
                //look back
                if (m_frames[m_frame - 1].isSpare == 1)
                {
                    m_frames[m_frame - 1].score += pins;
                    m_frames[m_frame].score += pins;   ///
                    m_frames[m_frame - 1].isSpare--;
                }

                //add to previous strike(s)- there may be consecutive strikes -> the ball adds to two scores
                if (m_frames[m_frame - 1].isStrike > 0)
                {
                    m_frames[m_frame - 1].score += pins;
                    m_frames[m_frame].score += pins;
                    m_frames[m_frame - 1].isStrike--;
                }
                if ((m_frame > 1) && (m_frames[m_frame - 2].isStrike > 0))
                {
                    m_frames[m_frame - 2].score += pins;
                    m_frames[m_frame - 1].score += pins;
                    m_frames[m_frame].score += pins;
                    m_frames[m_frame - 2].isStrike--;
                } 
            }

            //just so
            currentFrame.addBall(pins);
            m_bowlingScoreControlModel.SetBall(m_frame, m_frames[m_frame].balls.Count, pins);

            //for (int i = 0; i <= m_frame; i++)  //vieleicht nur die letzten beiden falls zu teuer
            for (int i = Math.Max(0, m_frame - 2); i <= m_frame; i++)
            {
                m_bowlingScoreControlModel.SetFrameScore(i, m_frames[i].score);
            }

            //Game over?
            //9.Frame is a strike -> three balls in frame 10
            //10. Frame is a strike -> three balls in frame 10
            //10. Frame is a spare -> three balls in frame 10
            if (m_frame == 9 && m_frames[m_frame].balls.Count == 3)
            {
                //m_bowlingScoreControlModel.SetTotalScore(m_frames[m_frame].score);
                m_bowlingScoreControlModel.GameOver = true;
            }
            if (m_frame == 9 && m_frames[m_frame].isSpare == 0 && m_frames[m_frame].isStrike == 0 && m_frames[m_frame].balls.Count == 2)
            {
                //m_bowlingScoreControlModel.SetTotalScore(m_frames[m_frame].score);
                m_bowlingScoreControlModel.GameOver = true;
            }
        }

        private BowlingFrame getFrame()
        {
            //first
            if (m_frame == -1)
            {
                return newFrame();

            }
            //new
            if (m_frames[m_frame].balls.Count == 0)
            {
                return m_frames[m_frame];
            }
            //old: current Frame is a strike -> new Frame (except in 10th frame)
            if (m_frames[m_frame].isStrike > 0)
            {
                if (m_frame < 9)
                {
                    return newFrame();
                } else
                {
                    return m_frames[m_frame];
                }             
            }
            //old: current Frame is a spare in 10th frame (you get a third ball)
            if (m_frames[m_frame].isSpare > 0 && m_frame == 9)
            {                   
                return m_frames[m_frame];
            }
            //old: current Frame is done
            if (m_frames[m_frame].balls.Count == 2 && m_frame < 9)
            {
                return newFrame();
            }
            //old: another attempt for current Frame
            return m_frames[m_frame];            
        }
        
        private BowlingFrame newFrame()
        {
            var frame = new BowlingFrame();
            if (m_frame >= 0)
            {
                frame.score = m_frames[m_frame].score;
            }
            m_frame++;
            m_frames[m_frame] = frame;
            return frame;
        }

        internal string GetId()
        {
            return m_bowlingScoreControlModel.GetId();
        }
    }

    
}
