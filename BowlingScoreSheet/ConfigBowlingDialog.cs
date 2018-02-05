using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreSheet
{
    /// <summary>
    /// Builds the controllers and models.
    /// </summary>
    public class ConfigBowlingDialog
    {

        public ConfigBowlingDialog(IBowlingDialog bd, IMyApp myApp)
        {
            string[] players = myApp.Players;
            if ((players == null) || (players.Length == 0))
                return;
            var playersInitials = ThisAndThat.playersInitials(players);

            //
            var bowlingScoreControlModels = new BowlingScoreControlModel[players.Length];
            var bowlingScoreControlControlers = new BowlingScoreControlControler[players.Length];

            var bowlingDialogModel = new BowlingDialogModel(bowlingScoreControlModels);

            //Create Controlers for the submodels.
            for (int i = 0; i < playersInitials.Length; i++)
            {
                bowlingScoreControlModels[i] = new BowlingScoreControlModel(players[i], playersInitials[i]);
                BowlingScoreControlControler bowlingScoreControlControler =
                    new BowlingScoreControlControler(bowlingScoreControlModels[i]);
                bowlingScoreControlControlers[i] = bowlingScoreControlControler;
            }
            var bowlingDialogControler =
                new BowlingDialogControler(bowlingDialogModel, bowlingScoreControlControlers);

            bd.SetBowlingDialogControler(bowlingDialogControler);
            bd.SetBowlingDialogModel(bowlingDialogModel);
            bd.SetBowlingScoreControlControlers(bowlingScoreControlControlers);
            bd.SetBowlingScoreControlModels(bowlingScoreControlModels);
        }
    }

    public interface IBowlingDialog
    {
        void SetBowlingDialogControler(BowlingDialogControler bowlingDialogControler);
        void SetBowlingDialogModel(BowlingDialogModel bowlingDialogModel);
        void SetBowlingScoreControlControlers(BowlingScoreControlControler[] bowlingScoreControlControlers);
        void SetBowlingScoreControlModels(BowlingScoreControlModel[] bowlingScoreControlModels);
        void SetPlayers(string[] players);
    }
}
