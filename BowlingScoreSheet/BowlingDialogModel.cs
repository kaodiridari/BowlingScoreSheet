using System;
using System.ComponentModel;
using System.Text;

namespace BowlingScoreSheet
{
    /// <summary>
    /// For several BowlingScoreControlModels and the pin-buttons.
    /// </summary>
    internal class BowlingDialogModel : INotifyPropertyChanged
    {
        #region score
        /// <summary>
        /// For the pin buttons.
        /// </summary>
        private bool[] m_isButtonEnabled;
        #endregion
        private bool m_isNewGame;

        private BowlingScoreControlModel[] m_playersScores;

        private int m_activeBowlingScoreControlModel;

        /// <summary>
        /// Constructor. Builds submodels for the given players.
        /// </summary>
        /// <param name="players">The players names.<br></br> Initials of the names are used. For empty strings the index is used.</param>
        public BowlingDialogModel(string[] players)
        {
            Init();

            //Initialise sub-models
            if (players.Length == 0)
            {
                throw new Exception("At least one player is necessary.");
            }

            m_playersScores = new BowlingScoreControlModel[players.Length];

            for (int i = 0; i < players.Length; i++)
            {
                string name;
                if (players[i] != null && (players[i]) != string.Empty)
                {
                    //initials
                    var s = players[i].Trim().Split(' ');
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in s)
                    {
                        sb.Append(item.Trim().Substring(0, 1)).Append(".");
                    }
                    name = sb.ToString();
                } else
                {
                    name = Convert.ToString(i); 
                }

                //Must be unique.
                int n = 0;
                for (int j = 0; j < i; j++)
                {
                    if (players[j].StartsWith(name))
                    {
                        n++;
                    }    
                }
                if (n > 0)
                {
                    //add (n)
                    name = (new StringBuilder(name)).Append('(').Append(n+1).Append(')').ToString();
                }

                m_playersScores[i] = new BowlingScoreControlModel(name);
            }
        }

        public bool IsNewGame
        {
            get
            {
                return m_isNewGame;
            }
        }

        private void Init()
        {
            m_isButtonEnabled = new bool[11];
            for (int i = 0; i < m_isButtonEnabled.Length; i++)
            {
                m_isButtonEnabled[i] = true;
            }
            m_isNewGame = false;
        }

        //Disable all buttons, exept of "new game".
        public void GameOver()
        {
            for (int i = 0; i < m_isButtonEnabled.Length; i++)
            {
                m_isButtonEnabled[i] = false;
            }
            m_isNewGame = true;
            UpdateProperty("IsButtonEnabled");
            UpdateProperty("IsNewGame");
        }

        public void setButtonsEnabled(int lastEnabledIndex)
        {
            m_isButtonEnabled = new bool[11];
            int i = 0;
            for (; i <= lastEnabledIndex; i++)
            {
                m_isButtonEnabled[i] = true;
            }
            for (int j = i; j < m_isButtonEnabled.Length; j++)
            {
                m_isButtonEnabled[j] = false;
            }
            //System.Console.WriteLine("setButtonsEnabled: " + lastEnabledIndex);
            UpdateProperty("IsButtonEnabled");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The player's id.</param>
        /// <param name="n">Number of pins.</param>
        public void justAnotherBallThrownFromPlayer(string player, int n)
        {
            int i = 0;
            for (; i < m_playersScores.Length; i++) {
                if (m_playersScores[i].Player.Equals(player))
                {
                    break;
                }                     
            }

            //Continue here
            //m_playersScores[i].
        }

        //For a new frame all pin-buttons must be enabled.
            //If ball is two   or    ball is one and pin is ten 
            //a new frame is entered.
            //Last frame: First ball is a strike -> all buttons enabled.
            //            Second ball is strike or second ball is a spare -> all buttons enabled.
            //TODO This is a controller business?
        private void enablePinButtons(int frame, int ball, int pins, BowlingScoreControlModel m)
        { 
            if (frame< 9)
            {
                if (ball == 2)
                {
                    setButtonsEnabled(10);
    }
                else if (ball == 1 && pins == 10)
                {
                    setButtonsEnabled(10);
}
                else
                {
                    setButtonsEnabled(10 - pins);
                }
            }
            else  //last frame
            {
                if (pins == 10)  //strike
                {
                    setButtonsEnabled(10);
                }
                else if (!"".Equals(m.Balls[18]) && !"".Equals(m.Balls[19]))  //avoid FormatException
                {
                    if (Convert.ToInt32(m.Balls[18]) + Convert.ToInt32(m.Balls[19]) == 10)  //spare
                    {
                        setButtonsEnabled(10);
                    }
                }
                else
                {
                    setButtonsEnabled(10 - pins);
                }
            }
        }

        public bool[] IsButtonEnabled
        {
            get
            {
                return m_isButtonEnabled;
            }
        }

        //selection model
        public int ActiveBowlingScoreControl
        {
            get
            {
                return m_activeBowlingScoreControlModel;
            }

            set
            {
                m_activeBowlingScoreControlModel = value;
            }
        }

        #region interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected void UpdateProperty(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        #endregion   
    }
}