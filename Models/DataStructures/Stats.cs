using OxyPlot;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Tintool.Models.DataStructures;

namespace Tinder.DataStructures
{
    public class Stats
    {
        public List<MatchData> Matches { get; set; }
        private DateTime date = DateTime.MinValue;

        public Stats()
        {
            Matches = new List<MatchData>();
        }

        public void ResetDate()
        {
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
                foreach (MatchData match in Matches)
                {
                    if (match.CreationDate.CompareTo(date) < 0)
                    {
                        date = match.CreationDate;
                    }
                }
            }
        }

        public float AverageMatchesPerDay()
        {
            int daysPassed = (DateTime.Now-date).Days+1;
            int matchesCount = Matches.Count;
            float result = (float)matchesCount/daysPassed;
            return result;
        }

        public void PlotMatchesThroughTime(PlotData dest)
        {
            dest.Points.Clear();

            for(DateTime i = date.Date; i<=DateTime.Now.Date;  i = i.AddDays(1))
            {
                int daysAgo = DateTime.Now.Date.Subtract(i.Date).Days;
                int matchesThatDay = Matches.Where((x)=>x.CreationDate.Date.Equals(i)).Count();
                dest.Points.Add(new DataPoint(daysAgo, matchesThatDay));
            }
        }

        public void PlotMessagesThroughTime(PlotData dest)
        {

        }
    }
}
