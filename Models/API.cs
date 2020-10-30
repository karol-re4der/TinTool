using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Text.Json;
using Tinder.DataStructures;
using Tintool.Models.DataStructures;
using Tintool.Models.DataStructures.Responses.Nearby;
using Tinder.DataStructures.Responses.Matches;
using Tintool.Models.DataStructures.Responses.Like;
using Tintool.Models.DataStructures.Responses.Like.Tintool.Models.DataStructures.Responses.Like;
using System.ComponentModel;
using System.Windows.Controls;

namespace Models
{
    public class API
    {
        private string token;
        private string uri = "https://api.gotinder.com/";
        HttpClient client;
        private Random rand = new Random();
        private ProgressBar progressIndicator;

        public API(string token)
        {
            this.token = token;

            client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Add("x-auth-token", token);
        }

        public MatchesResponse GetMatches(int amount)
        {
            HttpResponseMessage response = client.GetAsync("/v2/matches?count=" + amount).Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<MatchesResponse>(textResponse);
        }

        public NearbyResponse GetNearby()
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/v2/recs/core").Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<NearbyResponse>(textResponse);
        }

        public LikeResponse SendLike(string userID)
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/like/"+userID).Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;
            try
            {
                return JsonSerializer.Deserialize<LikeWithoutMatchResponse>(textResponse);
            }
            catch(System.Text.Json.JsonException e)
            {
                try
                {
                    return JsonSerializer.Deserialize<LikeAndMatchResponse>(textResponse);
                }
                catch (System.Text.Json.JsonException e2)
                {

                }
            }
            return null;
        }

        public bool Authenticate()
        {
            HttpResponseMessage response = client.GetAsync("/v2/recs/core").Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int SwipeAll()
        {
            int iterations = 0;
            int matchesGained = 0;

            while (iterations < 100) {
                NearbyResponse nearby = GetNearby();
                if (nearby.data.results != null)
                {
                    foreach (Tintool.Models.DataStructures.Responses.Nearby.Result user in nearby.data.results)
                    {
                        iterations++;
                        var likeResult = SendLike(user.user._id);

                        if (likeResult!=null)
                        {
                            if(likeResult.GetType() == typeof(LikeWithoutMatchResponse))
                            {

                            }
                            else if(likeResult.GetType() == typeof(LikeAndMatchResponse))
                            {
                                matchesGained++;
                            }
                            else
                            {
                                return matchesGained;
                            }
                        }
                        else
                        {
                            return matchesGained;
                        }
                        
                        if(likeResult?.likes_remaining == 0)
                        {
                            return matchesGained;
                        }
                    }
                }
                else
                {
                    return matchesGained;
                }
            }
            return matchesGained;
        }

        private void Delay()
        {
            System.Threading.Thread.Sleep(rand.Next() % 50 + 50);
        }
    }
}

