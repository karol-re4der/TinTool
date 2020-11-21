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
        public string ProfileID { get; set; }

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

        public float ResponseRate()
        {
            int totalResponses = 0;
            int totalMessaged = 0;
            foreach (MatchData match in Matches)
            {
                if (match.ResponseStatus != ResponseStatusTypes.Undefined && match.ResponseStatus != ResponseStatusTypes.Empty)
                {
                    if (match.ResponseStatus == ResponseStatusTypes.MessagedResponded)
                    {
                        totalResponses += 1;
                        totalMessaged += 1;
                    }
                    if (match.ResponseStatus != ResponseStatusTypes.MessagedNotResponded)
                    {
                        totalMessaged += 1;
                    }
                }
            }

            if (totalMessaged > 0)
            {
                return (float)totalResponses / totalMessaged;
            }
            else
            {
                return 0;
            }
        }

        public float AverageConversationLength()
        {
            int totalConversations = 0;
            int totalMessages = 0;
            foreach (MatchData match in Matches)
            {
                if (match.ResponseStatus != ResponseStatusTypes.Undefined && match.ResponseStatus != ResponseStatusTypes.Empty)
                {
                    totalMessages += match.MessageCount;
                    totalConversations += 1;
                }
            }

            if (totalConversations > 0)
            {
                return (float)totalMessages / totalConversations;
            }
            else
            {
                return 0;
            }
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

        public void PlotMessagesThroughTime(PlotData totalDest, PlotData sentDest, PlotData receivedDest)
        {
            totalDest.Points.Clear();
            sentDest.Points.Clear();
            receivedDest.Points.Clear();

            List<MessageData> allMsg = new List<MessageData>();
            foreach (MatchData match in Matches)
            {
                if (match.Conversation?.Count > 0)
                {
                    foreach (MessageData msg in match.Conversation)
                    {
                        allMsg.Add(msg);
                    }
                }
            }

            for (DateTime i = date.Date; i <= DateTime.Now.Date; i = i.AddDays(1))
            {
                List<MessageData> msgThatDay = allMsg.Where((x) => x.Date.Date.Equals(i)).ToList();

                int daysAgo = DateTime.Now.Date.Subtract(i.Date).Days;
                int totalThatDay = msgThatDay.Count;
                int sentThatDay = msgThatDay.Where((x) => x.ReceiverId.Equals(ProfileID)).Count();
                int receivedThatDay = totalThatDay-sentThatDay;
                totalDest.Points.Add(new DataPoint(daysAgo, totalThatDay));
                sentDest.Points.Add(new DataPoint(daysAgo, sentThatDay));
                receivedDest.Points.Add(new DataPoint(daysAgo, receivedThatDay));
            }
        }
    }
}
