using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinder.DataStructures;
using Tintool.Models.DataStructures;
using Tintool.Models.DataStructures.Responses.Like.Tintool.Models.DataStructures.Responses.Like;
using Tintool.Models.DataStructures.Responses.Nearby;

namespace Tintool.Models
{
    class Unitool
    {
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
                    NearbyResponse nearby = api.GetNearby();
                    if (nearby.data.results != null)
                    {
                        foreach (Tintool.Models.DataStructures.Responses.Nearby.Result user in nearby.data.results)
                        {
                            iterations++;
                            if (iterations <= size)
                            {
                                //orginal
                                var likeResult = api.SendLike(user.user._id);
                                //

                                //debug
                                //var likeResult = new LikeAndMatchResponse();
                                //Thread.Sleep(1000);
                                //likeResult.likes_remaining = 10;
                                //

                                if (likeResult != null)
                                {
                                    if (token.IsCancellationRequested)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        OnProgress(iterations);
                                    }
                                    if (likeResult.GetType() == typeof(LikeWithoutMatchResponse))
                                    {

                                    }
                                    else if (likeResult.GetType() == typeof(LikeAndMatchResponse))
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
                                    else
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    return;
                                }

                                if (likeResult?.likes_remaining == 0)
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
                return;
            });
        }
    }
}
