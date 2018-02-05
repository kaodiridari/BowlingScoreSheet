using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreSheet
{
    class DataItem
    {
        public DataItem(string name)
        {
            DIPlayersName = name;
        }

        public string DIPlayersName { get; set; }
    }
}
