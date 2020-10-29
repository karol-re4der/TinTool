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
        public List<Responses.Matches.Match> matches { get; set; }
        private DateTime date = DateTime.MinValue;

        public Stats()
        {
            matches = new List<Responses.Matches.Match>();
        }

        public void ResetDate()
        {
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
                foreach (Responses.Matches.Match match in matches)
                {
                    if (match.created_date.CompareTo(date) < 0)
                    {
                        date = match.created_date;
                    }
                }
            }
        }

        public void Display()
        {
            Console.WriteLine("Average matches per day since "+date.ToString("d") + ": "+AverageMatchesPerDay());
        }

        public float AverageMatchesPerDay()
        {
            int daysPassed = DateTime.Now.Day-date.Day+1;
            int matchesCount = matches.Count;
            float result = (float)matchesCount/daysPassed;
            return result;
        }

        public void PlotMatchesThroughTime(PlotData dest)
        {
            dest.Points.Clear();

            for(DateTime i = date.Date; i<=DateTime.Now.Date;  i = i.AddDays(1))
            {
                int daysAgo = DateTime.Now.Date.Subtract(i.Date).Days;
                int matchesThatDay = matches.Where((x)=>x.created_date.Date.Equals(i)).Count();
                dest.Points.Add(new DataPoint(daysAgo, matchesThatDay));
            }
        }
    }
}
