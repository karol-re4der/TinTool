using Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinder.DataStructures;
using Tintool.Models.DataStructures;
using Tintool.Models.DataStructures.Responses.Nearby;

namespace Tintool.Models
{
    class Unitool
    {
        public static void LogNewMatches(List<MatchData> potentialMatches, Stats stats)
        {
            foreach (MatchData match in potentialMatches)
            {
                //add
                MatchData existing = stats.Matches.Find((x) => x.Id.Equals(match.Id));
                if (existing == null)
                {
                    stats.Matches.Add(match);
                }
            }
        }


        public static List<string> ProximityCheck(List<DataStructures.UserResponse.Results> matches, int cutout, int distance)
        {
            List<string> results = new List<string>();

            foreach (DataStructures.UserResponse.Results match in matches)
            {
                if (match != null)
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


        public static Task SwipeAll(API api, int size, Action<int> OnProgress, Action<string> OnMatch, Action<string> OnFinish, CancellationToken cancellationToken)
        {
            return new Task(() =>
            {

                int iterations = 0;
                int matchesGained = 0;
                CancellationToken token = cancellationToken;

                while (iterations <= size)
                {
                    List<PersonData> nearby = api.GetNearby();
                    if (nearby?.Count>0)
                    {
                        foreach (PersonData person in nearby)
                        {
                            iterations++;
                            if (iterations <= size)
                            {
                                LikeData likeResult = api.SendLike(person.Id);
   
                                if (likeResult?.LikesRemaining>0)
                                {
                                    if (token.IsCancellationRequested)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        OnProgress((int)((float)iterations/size*100));
                                    }
                                    if (likeResult?.ResultingMatch!=null)
                                    {
                                        matchesGained++;
                                        if (token.IsCancellationRequested)
                                        {
                                            return;
                                        }
                                        else
                                        {
                                            OnMatch(likeResult.ResultingMatch.Person.Name);
                                        }
                                    }
                                }
                                else
                                {
                                    OnFinish.Invoke($"Swipe all: No more swipes available. {matchesGained} matches gained!");
                                    return;
                                }
                            }
                            else
                            {
                                OnFinish.Invoke($"Swipe all: Finished! {matchesGained} matches gained!");
                                return;
                            }
                        }
                    }
                    else
                    {
                        OnFinish.Invoke($"Swipe all: No one to swipe on! {matchesGained} matches gained!");
                        return;
                    }
                }
                OnFinish.Invoke($"Swipe all: Finished! {matchesGained} matches gained!");
                return;
            });
        }

        public static Task ValidateMatches(API api, Stats stats, Action<int> OnProgress, CancellationToken cancellationToken)
        {
            return new Task(() =>
            {
                int progress = 0;
                int maxProgress = stats.Matches.Where((x) => x.Active).Count();
                CancellationToken token = cancellationToken;
                foreach (MatchData match in stats.Matches)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    else if (match.Active)
                    {
                        List<MessageData> messages = api.GetMessages(match.Id);

                        //activity
                        if (messages == null)
                        {
                            match.Active = false;
                            continue;
                        }

                        //messages
                        if (messages == null)
                        {
                            continue;
                        }
                        else if (messages.Find((x) => !x.ReceiverId.Equals(messages.First().ReceiverId)) != null)
                        {
                            if (!messages.Last().ReceiverId.Equals(match.Person.Id))
                            {
                                match.ResponseStatus = ResponseStatusTypes.MessagedResponded;
                            }
                            else
                            {
                                match.ResponseStatus = ResponseStatusTypes.GotMessageResponded;
                            }
                        }
                        else if (messages.Count > 0)
                        {
                            if (!messages.Last().ReceiverId.Equals(match.Person.Id))
                            {
                                match.ResponseStatus = ResponseStatusTypes.MessagedNotResponded;
                            }
                            else
                            {
                                match.ResponseStatus = ResponseStatusTypes.GotMessageNotResponded;
                            }
                        }
                        else
                        {
                            match.ResponseStatus = ResponseStatusTypes.Empty;
                        }

                        match.Conversation = messages;
                        match.MessageCount = messages.Count;

                        if (token.IsCancellationRequested)
                        {
                            return;
                        }
                        else
                        {
                            progress++;
                            OnProgress((int)((float)progress/maxProgress*100));
                        }
                    }
                }

            });
        }
    }
}
