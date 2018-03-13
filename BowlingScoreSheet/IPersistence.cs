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

        /// <summary>
        /// Get all games in the given (closed) intervall.<br>
        /// The dates are stored in UTC. Be shure to convert the parameters to UTC
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns>An array of json-strings. May be empty but not null.</returns>
        string[] Get(DateTime dtStart, DateTime dtEnd);
    }
}
