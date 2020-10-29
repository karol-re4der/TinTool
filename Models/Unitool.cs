using System;
using System.Collections.Generic;
using System.Text;
using Tinder.DataStructures;

namespace Tintool.Models
{
    class Unitool
    {
        public static void LogNewMatches(List<Tinder.DataStructures.Responses.Matches.Match> potentialMatches, Stats stats)
        {
            foreach (Tinder.DataStructures.Responses.Matches.Match match in potentialMatches)
            {
                Tinder.DataStructures.Responses.Matches.Match existing = stats.matches.Find((x) => x.id.Equals(match.id));
                if (existing != null)
                {
                    stats.matches.Remove(existing);
                }
                stats.matches.Add(match);
            }
        }
    }
}
