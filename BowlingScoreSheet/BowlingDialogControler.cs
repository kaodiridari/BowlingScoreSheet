using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreSheet
{
    /// <summary>
    /// The Controler for the (main) Window.
    /// </summary>
    internal class BowlingDialogControler
    {
        BowlingDialogModel m_bowlingDialogModel;

        internal BowlingDialogControler(BowlingDialogModel m)
        {
            m_bowlingDialogModel = m;
        }

        internal void Clear()
        {
            throw new NotImplementedException();
        }

        internal void JustAnotherBallThrown(int numberOfPins)
        {
            //throw new NotImplementedException();
            //1. Which is the active BowlingScoreControl? 
            //2. Notify the controll. 
            BowlingScoreControlControler activeBowlingScoreControlControler;
            int i = m_bowlingDialogModel.ActiveBowlingScoreControl;
        }
    }
}
