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
using Tintool.Models.DataStructures.UserResponse;
using System.Threading.Tasks;

namespace Models
{
    public class API
    {
        private string token;
        private string uri = "https://api.gotinder.com/";
        HttpClient client;
        private Random rand;

        public API(string token)
        {
            this.token = token;

            rand = new Random();
            client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            client.DefaultRequestHeaders.Add("x-auth-token", token);
        }

        public List<MatchData> GetMatches(int amount)
        {
            HttpResponseMessage response = client.GetAsync("/v2/matches?count=" + amount).Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            List<MatchData> result = new List<MatchData>();
            foreach(Tinder.DataStructures.Responses.Matches.Match match in JsonSerializer.Deserialize<MatchesResponse>(textResponse).data.matches)
            {
                MatchData newMatchData = new MatchData
                {
                    Id = match._id,
                    Name = match.person.name,
                    CreationDate = match.created_date,
                };
                result.Add(newMatchData);
            }

            return result;
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

        public UserResponse GetUser(string userID)
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/user/" + userID).Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<UserResponse>(textResponse);
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

        public async Task<bool> Authenticate()
        {
            HttpResponseMessage response = await client.GetAsync("/v2/recs/core");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Delay()
        {
            System.Threading.Thread.Sleep(rand.Next() % 50 + 50);
        }
    }
}

