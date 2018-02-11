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
    public class BowlingDialogControler
    {
        private BowlingDialogModel m_bowlingDialogModel;

        private BowlingScoreControlControler[] m_controlControlers;

        public BowlingDialogControler(BowlingDialogModel m,
            BowlingScoreControlControler[] controlControlers)
        {
            m_bowlingDialogModel = m;
            m_controlControlers = controlControlers;
        }

        public BowlingScoreControlControler GetControlControler(string id)
        {
            foreach (var item in m_controlControlers)
            {
                if(item.GetId().Equals(id))
                {
                    return item;
                }
            }
            return null;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        //WPF: comes from a click_Event.
        public void SetActiveControl(string id)
        {
            m_bowlingDialogModel.ActiveBowlingScoreControl = id;
        } 

        public void JustAnotherBallThrown(int numberOfPins)
        {
            //throw new NotImplementedException();
            //1. Which is the active BowlingScoreControl? 
            //2. Notify the controll. 
            
            string id = m_bowlingDialogModel.ActiveBowlingScoreControl;
            foreach (var item in m_controlControlers)
            {
                if (item.GetId().Equals(id))
                {
                    item.justAnotherBallThrown(numberOfPins);
                    break;
                }
            }            
        }

        public string[] GetPlayerIds()
        {
            string[] ids = m_bowlingDialogModel.GetPlayersIds();
            return ids;
        }

        public void Save()
        {            
            BowlingScoreControlModel[] models = m_bowlingDialogModel.GetBowlingScoreControlModels();
            string json = ThisAndThat.Jsonize<BowlingScoreControlModel[]>(models);
            MyApp.getInstance().Save(json);
        }
    }
}
