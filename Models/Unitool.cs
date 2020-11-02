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

        public static List<string> ProximityCheck(List<DataStructures.UserResponse.Results> matches, int cutout, int distance)
        {
            List<string> results = new List<string>();

            foreach (DataStructures.UserResponse.Results match in matches)
            {
                if (match!=null)
                {
                    if (match.distance_mi > distance)
                    {
                        continue;
                    }

                    results.Add(match.name);
                }
            }

            return results;
        }
    }
}
