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
using System.ComponentModel;
using System.Windows.Controls;
using Tintool.Models.DataStructures.UserResponse;
using System.Threading.Tasks;
using Tintool.Models.DataStructures.Responses.Messages;

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

        public List<MessageData> GetMessages(string matchID, int amount = 100)
        {
            Delay();
            HttpResponseMessage response = client.GetAsync($"/v2/matches/{matchID}/messages?count={amount}").Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            List<MessageData> result = new List<MessageData>();
            foreach (Tintool.Models.DataStructures.Responses.Messages.Message msg in JsonSerializer.Deserialize<MessagesResponse>(textResponse).data.messages)
            {
                MessageData nextMessage = new MessageData
                {
                    Text = msg.message,
                    ReceiverId = msg.from
                };
                result.Add(nextMessage);
            }

            return result;
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
                MatchData newMatchData = new MatchData(match);
                result.Add(newMatchData);
            }

            return result;
        }

        public List<PersonData> GetNearby()
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/v2/recs/core").Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            List<PersonData> result = new List<PersonData>();
            foreach(Result person in JsonSerializer.Deserialize<NearbyResponse>(textResponse).data.results)
            {
                PersonData newPerson = new PersonData
                {
                    Id = person.user._id,
                    Name = person.user.name
                };
                result.Add(newPerson);
            }
            return result;
        }

        public PersonData GetUser(string userID)
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/user/" + userID).Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            UserResponse userResponse = JsonSerializer.Deserialize<UserResponse>(textResponse);
            if (userResponse?.results!=null)
            {
                PersonData result = new PersonData
                {
                    Id = userResponse.results._id,
                    Name = userResponse.results.name
                };
                return result;
            }
            else
            {
                return null;
            }
        }

        public LikeData SendLike(string userID)
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/like/"+userID).Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            LikeData result = new LikeData();
            try
            {
                LikeWithoutMatchResponse likeWithoutMatchResponse = JsonSerializer.Deserialize<LikeWithoutMatchResponse>(textResponse);
                result.LikesRemaining = likeWithoutMatchResponse.likes_remaining;
            }
            catch(System.Text.Json.JsonException e)
            {
                try
                {
                    LikeAndMatchResponse likeAndMatchResponse = JsonSerializer.Deserialize<LikeAndMatchResponse>(textResponse);
                    result.ResultingMatch = new MatchData(likeAndMatchResponse.match);
                }
                catch (System.Text.Json.JsonException e2)
                {
                    return null;
                }
            }
            return result;
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

