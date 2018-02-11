using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingScoreSheet
{
    public interface IPersistence
    {
        /// <summary>
        /// Saves a Json-String.
        /// </summary>
        /// <param name="json"></param>
        void Save(string json);
    }
}
