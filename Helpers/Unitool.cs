﻿using Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tintool.APIs.Badoo;
using Tintool.Models.Saveables;

namespace Tintool.Models
{
    class Unitool
    {
        public static Task LogNewMatches(TinderAPI api, Action<bool> OnWorkFinished, StatsModel stats, CancellationToken cancellationToken)
        {
            return new Task(() =>
            {
                CancellationToken token = cancellationToken;
                if (api.IsTokenWorking())
                {
                    foreach (MatchModel match in api.GetMatches(100))
                    {
                        //add
                        if (!token.IsCancellationRequested)
                        {
                            MatchModel existing = stats.Matches.Find((x) => x.IsSameMatch(match));
                            if (existing == null)
                            {
                                stats.Matches.Add(match);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                OnWorkFinished(true);
            });
        }

        public static Task ProximityCheck(TinderAPI api, StatsModel stats, int distance, Action<int> OnProgress, Action<string> OnResults, Action<string> OnFinish, CancellationToken token)
        {
            return new Task(() =>
            {
                string resultText = "";
                List<string> results = new List<string>();
                int progress = 0;
                int maxProgress = stats.Matches.Count();
                List<MatchModel> allMatches = api.GetMatches(100);
                

                foreach (MatchModel match in stats.Matches)
                {
                    if (token.IsCancellationRequested)
                    {
                        resultText = "Proximity check:";
                        foreach(string result in results)
                        {
                            resultText += "\n"+result;
                        }
                        OnFinish.Invoke(resultText);
                        return;
                    }
                    else
                    {
                        progress++;
                        OnProgress.Invoke((int)((float)progress / maxProgress * 100));
                    }

                    if (match.Active && (match.ResponseStatus == ResponseStatusTypes.Empty || match.ResponseStatus == ResponseStatusTypes.Undefined))
                    {
                        if (allMatches.Any((x) => x.Person.Id.Equals(match.Person.Id)))
                        {
                            PersonModel upToDateMatch = api.GetUser(match.Person.Id);
                            if (upToDateMatch?.Distance <= distance)
                            {
                                results.Add(upToDateMatch.Name);
                                if (token.IsCancellationRequested)
                                {
                                    resultText = "Proximity check: Canceled!";
                                    OnFinish.Invoke(resultText);
                                    return;
                                }
                                else
                                {
                                    OnProgress.Invoke((int)((float)progress / maxProgress * 100));
                                    OnResults.Invoke(upToDateMatch.Name);
                                }
                            }
                        }
                        else
                        {
                            match.Active = false;
                        }
                    }
                }
                resultText = "Proximity check: Complete! Result:";
                foreach (string result in results)
                {
                    resultText += "\n" + result;
                }
                OnFinish.Invoke(resultText);
                return;
            });
        }

        public static Task SwipeAll(TinderAPI api, int size, Action<int> OnProgress, Action<string> OnMatch, Action<string> OnFinish, CancellationToken cancellationToken)
        {
            return new Task(() =>
            {

                int iterations = 0;
                int matchesGained = 0;
                CancellationToken token = cancellationToken;

                while (iterations <= size)
                {
                    List<PersonModel> nearby = api.GetNearby();
                    if (nearby?.Count>0)
                    {
                        foreach (PersonModel person in nearby)
                        {
                            iterations++;
                            if (iterations <= size)
                            {
                                LikeModel likeResult = api.SendLike(person.Id, person.SCode);
   
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
                                            OnMatch(matchesGained+" matches thus far!");
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

        public static Task ValidateMatches(TinderAPI api, StatsModel stats, Action<int> OnProgress, CancellationToken cancellationToken)
        {
            return new Task(() =>
            {
                int progress = 0;
                int maxProgress = stats.Matches.Where((x) => x.Active).Count();
                List<MatchModel> allMatches = api.GetMatches(100);
                CancellationToken token = cancellationToken;
                foreach (MatchModel match in stats.Matches)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    else if (match.Active)
                    {
                        List<MessageModel> messages = api.GetMessages(match.Id);

                        //activity
                        if (messages == null || !allMatches.Any((x)=>x.Person.Id.Equals(match.Person.Id)))
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

        public static Task StartUp(Action<SettingsModel> OnSettingsLoaded, Action<TinderAPI, bool> OnTinderAPILoaded, Action<BadooAPI, bool> OnBadooAPILoaded, Action<StatsModel> OnStatsLoaded, Action<bool> OnFinished, CancellationToken cancellationToken)
        {
            return new Task(() =>
            {
                CancellationToken token = cancellationToken;

                //Settings
                FileManager.Prepare();
                string newFileName = FileManager.CreateUniqueStatsName();
                SettingsModel loadedSettings = FileManager.LoadSettings();
                OnSettingsLoaded(loadedSettings);
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                //APIs
                TinderAPI tinderAPI = new TinderAPI();
                SessionModel loadedSession = FileManager.LoadSession("tinder");
                if (loadedSession?.AuthToken.Length > 0)
                {
                    tinderAPI.SetSession(loadedSession);
                    if (tinderAPI.IsTokenWorking())
                    {

                    }
                    else if (loadedSession?.RefreshToken.Length > 0)
                    {
                        loadedSession = tinderAPI.TryRefresh(loadedSession);

                        if (loadedSession != null)
                        {
                            tinderAPI.SetSession(loadedSession);
                        }
                    }
                }
                OnTinderAPILoaded(tinderAPI, tinderAPI.IsTokenWorking());

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                OnBadooAPILoaded(new BadooAPI(), false);

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                //Stats
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                StatsModel stats = FileManager.LoadStatsWithFileName(loadedSettings.DefaultSaveFile);
                if (stats == null)
                {
                    stats = new StatsModel(newFileName);
                    loadedSettings.DefaultSaveFile = newFileName;
                    FileManager.SaveStats(stats);
                    stats.ResetDate();
                }
                if (tinderAPI.IsTokenWorking())
                {
                    stats.ProfileIDs.Add(tinderAPI.GetProfileID());
                    stats.ProfileIDs = stats.ProfileIDs.Distinct().ToList();
                }
                OnStatsLoaded(stats);

                OnFinished(true);
            });
        }
    }
}
