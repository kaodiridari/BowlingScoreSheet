using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace BowlingScoreSheet
{
    /// <summary>
    /// For several BowlingScoreControlModels and the pin-buttons.
    /// </summary>
    public class BowlingDialogModel : INotifyPropertyChanged
    {        
        /// <summary>
        /// For the pin buttons.
        /// </summary>
        private bool[] m_isButtonEnabled;
       
        private bool m_isNewGame;

        private BowlingScoreControlModel[] m_BowlingScoreControlModels;

        private string m_activeBowlingScoreControlModel;

        /// <summary>
        /// Constructor. Builds submodels for the given players.
        /// </summary>
        /// <param name="players">The players names.<br></br> Initials of the names are used. For empty strings the index is used.</param>
        public BowlingDialogModel(BowlingScoreControlModel[] bowlingScoreControlModels)
        {
            m_BowlingScoreControlModels = bowlingScoreControlModels;
            Init();            
        }

        public bool IsNewGame
        {
            get
            {
                return m_isNewGame;
            }
        }

        public string[] GetPlayersIds()
        {
            List <string> ids = new List<string>();
            foreach (var item in m_BowlingScoreControlModels)
            {
                ids.Add(item.PlayersInitials);
            }
            return ids.ToArray();
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

        internal BowlingScoreControlModel[] GetBowlingScoreControlModels()
        {
            return m_BowlingScoreControlModels;
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
        /// <param name="playerId">The player's id.</param>
        /// <param name="n">Number of pins.</param>
        public void justAnotherBallThrownFromPlayer(string playerId, int n)
        {
            int i = 0;
            for (; i < m_BowlingScoreControlModels.Length; i++) {
                if (m_BowlingScoreControlModels[i].PlayersInitials.Equals(playerId))
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

        //The selection model. The model for the now selected BowlingScoreControl.
        public string ActiveBowlingScoreControl
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