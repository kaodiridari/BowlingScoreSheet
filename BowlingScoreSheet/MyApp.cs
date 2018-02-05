using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreSheet
{
    public interface IMyApp
    {
        string[] Players { get; set; }        
    }

    public class MyApp : IMyApp
    {
        private static MyApp me;

        private MyApp()
        {
        }

        public static MyApp getInstance()
        {
            if (me == null)
            {
                me = new MyApp();
            }

            return me;
        }

        public string[] Players{get; set;}

        public void PlayersDialogDone()
        {
            //Open the BowlingDialog
            var bd = new BowlingDialog();
            bd.Show(); ;
        }
    }
}
