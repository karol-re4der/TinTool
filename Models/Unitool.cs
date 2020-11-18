using Models;
using System;
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
        public static List<MessageData> GetMessages(API api)
        {
            List<MessageData> result = new List<MessageData>();

            string foo = "5cbb0a3178d4ba1500457ad55f9fcfa74962d601007a07f7";

            var bar = api.GetMessages(foo);

            return result;
        }

        public static void LogNewMatches(List<MatchData> potentialMatches, Stats stats)
        {
            foreach (MatchData match in potentialMatches)
            {
                MatchData existing = stats.Matches.Find((x) => x.Id.Equals(match.Id));
                if (existing != null)
                {
                    stats.Matches.Remove(existing);
                }
                stats.Matches.Add(match);
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


        public static Task SwipeAll(API api, int size, Action<int> OnProgress, Action<int> OnMatch, CancellationToken cancellationToken)
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
                                //orginal
                                LikeData likeResult = api.SendLike(person.Id);
                                //

                                //debug
                                //var likeResult = new LikeAndMatchResponse();
                                //Thread.Sleep(1000);
                                //likeResult.likes_remaining = 10;
                                //

                                if (likeResult?.LikesRemaining>0)
                                {
                                    if (token.IsCancellationRequested)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        OnProgress(iterations);
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
                                            OnMatch(matchesGained);
                                        }
                                    }
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            });
        }
    }
}
