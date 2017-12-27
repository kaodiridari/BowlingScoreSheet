using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BowlingScoreSheet
{
    /// <summary>
    /// The model for a supercalifragilisticexpialidocious BowlingScoreControl.
    /// </summary>
    public class BowlingScoreControlModel : INotifyPropertyChanged, IBowlingScoreControlModel
    { 
        private string[] m_ball;

        private string[] m_FrameScore;         

        private string m_player;

        private bool m_gameOver = false;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="players">The number of BowlingScoreControls.</param>
        public BowlingScoreControlModel(string player)
        {
            m_player = player;
            Init();
        }

        private void Init()
        {
            m_FrameScore = new string[10];
            for (int i = 0; i < m_FrameScore.Length; i++)
            {
                m_FrameScore[i] = "";
            }            

            m_ball = new string[21];
            for (int i = 0; i < m_ball.Length; i++)
            {
                m_ball[i] = "";
            }
        }        

        public void Clear()
        {
            Init();
            UpdateProperty("IsButtonEnabled");
            UpdateProperty("IsNewGame");
            UpdateProperty("FrameScore");
            UpdateProperty("Balls");
        }

        public void SetFrameScore(int frameNumber, int score)
        {
            m_FrameScore[frameNumber] = Convert.ToString(score);
            UpdateProperty("FrameScore");
        }

        public void SetTotalScore(int score)
        {
            TotalScore = Convert.ToString(score);
            UpdateProperty("TotalScore");
        }

        #region interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected void UpdateProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion         

        /// <summary>
        /// Each ball's score.
        /// </summary>
        /// <param name="frame">0..9; The number of the frame.</param>
        /// <param name="ball">1 or 2. 3 for 10th frame allowed. First or second ball in the frame.</param>
        /// <param name="pins">0 .. 10; The score for the given ball.</param>
        public void SetBall(int frame, int ball, int pins)
        {
            if (frame < 0 || frame > 9)
                throw new Exception("Illegal frame: " + frame);
            if (ball < 1 || ball > 2)
            {
                if (frame == 9 && ball != 3)
                {
                    throw new Exception("Illegal parameter for ball: " + ball);
                }
            }
            if (pins > 10 || pins < 0)
            {
                throw new Exception("Illegal number of pins: " + pins);
            }

            //index
            int i = (2 * (frame + 1) - 1) + (ball - 1) - 1;
            m_ball[i] = Convert.ToString(pins);
            UpdateProperty("Balls");             
        }

        public void SetPlayer(string player)
        {
            throw new NotImplementedException();
        }

        public string[] Balls
        {
            get
            {
                return m_ball;
            }
        }        

        public string[] FrameScore
        {
            get
            {
                return m_FrameScore;
            }
        }

        public string TotalScore
        {
            get
            {
                return "000";
            }

            set
            {

            }
        }

        public string PlayersInitials
        {
            get
            {
                return "A.B.";
            }

            set
            {

            }
        }

        public string Player
        {
            get
            {
                return this.m_player;
            }

            set
            {
                m_player = value;
            }
        }

        public bool GameOver
        {
            get
            {
                return m_gameOver;
            }

            set
            {
                m_gameOver = value;
               //TODO control dann irgendwie einfärben
            }           
        }

        public BowlingScoreControlControler Controler
        {
            get;
            //internal set;
        }
    }
}
