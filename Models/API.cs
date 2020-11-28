﻿

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
using Tintool.Models.DataStructures.Responses;
using System.Net;
using System.IO.Compression;

namespace Models
{

    public class API
    {
        private string _token;
        private string _uri = "https://api.gotinder.com/";
        HttpClientHandler handler;
        HttpClient client;
        private Random rand;

        public API()
        {
            rand = new Random();
            handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(_uri);
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
                    ReceiverId = msg.from,
                    Date = msg.sent_date,
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
                    Name = userResponse.results.name,
                    Birthday = userResponse.results.birth_date,
                    Distance = userResponse.results.distance_mi
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

        #region authentication
        public bool RequestLoginCode(string phoneNumber)
        {
            //string foo = "\u001f�\b\0\0\0\0\0\0\u0003r���\u0015�Z镑䞜���\u0019Z�i��e���n��l�\u000f�~!�zn�~�\u0011A~�N��y%f�\u0011�in���U��y�\u0011\u0001.y%�\u001e\u0005\u0006��I9�B*F�)F)�))��i�f�&��準��I��\u0016��I�\u0006�Ɇ\u0016J\u0012�i�i�i��&�fF)f\u0006�\u0006\u0006�\u0006�i�Zl\u001c7��n`\u0004\0\0\0��\u0003\0ӊ�>�\0\0\0";
            //var bar = Decompress(Encoding.UTF8.GetBytes(foo));
            //string foobar = Encoding.UTF8.GetString(bar);
            //return false;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("tinder-version", "2.64.0");
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36");
            client.DefaultRequestHeaders.Add("platform", "web");
            client.DefaultRequestHeaders.Add("persistent-device-id", "89621f05-6135-426c-b74e-8b4a850ff1d8");
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate");
            client.DefaultRequestHeaders.Add("app-version", "1026400");
            client.DefaultRequestHeaders.Add("app-session-id", "28cb573e-0727-4541-a605-75b9cea98767");
            client.DefaultRequestHeaders.Add("x-supported-image-formats", "webp");
            client.DefaultRequestHeaders.Add("funnel-session-id", "727442c23bbc4799");
            client.DefaultRequestHeaders.Add("app-session-time-elapsed", "45913");
            client.DefaultRequestHeaders.Add("accept-language", "en-US");

            string payloadContent = "\n\r\n\v" + phoneNumber;

            Delay();
            var payload = new StringContent(payloadContent, Encoding.UTF8, "application/x-google-protobuf");
            HttpResponseMessage response = client.PostAsync("/v3/auth/login", payload).Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return false;
            }

            return true;
        }
        public string RequestAuthToken(string code, string phoneNumber)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("tinder-version", "2.64.0");
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36");
            client.DefaultRequestHeaders.Add("platform", "web");
            client.DefaultRequestHeaders.Add("persistent-device-id", "89621f05-6135-426c-b74e-8b4a850ff1d8");
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate");
            client.DefaultRequestHeaders.Add("app-version", "1026400");
            client.DefaultRequestHeaders.Add("app-session-id", "28cb573e-0727-4541-a605-75b9cea98767");
            client.DefaultRequestHeaders.Add("x-supported-image-formats", "webp");
            client.DefaultRequestHeaders.Add("funnel-session-id", "727442c23bbc4799");
            client.DefaultRequestHeaders.Add("app-session-time-elapsed", "45913");
            client.DefaultRequestHeaders.Add("accept-language", "en-US");

            string payloadContent = "" + "\n\r\n\v" + phoneNumber + "" + code;

            Delay();
            var payload = new StringContent(payloadContent, Encoding.UTF8, "application/x-google-protobuf");
            HttpResponseMessage response = client.PostAsync("/v3/auth/login", payload).Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return "";
            }

            string responseAsString = response.Content.ReadAsStringAsync().Result;
            int tokenStartIndex = responseAsString.IndexOf("$")+2;
            int tokenEndIndex = responseAsString.IndexOf("\"");
            int tokenLength = tokenEndIndex - tokenStartIndex;
            return responseAsString.Substring(tokenStartIndex, tokenLength);
        }

        public bool IsTokenWorking()
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/v2/recs/core").Result;
            return response.IsSuccessStatusCode;
        }
        public void SetToken(string newToken)
        {
            this._token = newToken;
            client.DefaultRequestHeaders.Add("x-auth-token", newToken);
        }
        public string GetToken()
        {
            return _token;
        }
        #endregion

        public string GetProfileID()
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/v2/profile?locale=en&include=likes%2Cplus_control%2Cproducts%2Cpurchase%2Cuser").Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            ProfileResponse profileResponse = JsonSerializer.Deserialize<ProfileResponse>(textResponse);
            if (profileResponse?.data?.user!=null)
            {
                return profileResponse.data.user._id;
            }
            else
            {
                return null;
            }
        }

        private void Delay()
        {
            System.Threading.Thread.Sleep(rand.Next() % 50 + 25);
        }
        private byte[] Decompress(byte[] gzip)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}

