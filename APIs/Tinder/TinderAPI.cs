

using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.IO;
using System.Text.Json;
using System.Net;
using Tintool.Models;
using Tintool.Models.Saveables;
using Tintool.APIs.Tinder.Responses.MatchesResponse;
using Tintool.APIs.Tinder.Responses.NearbyResponse;
using Tintool.APIs.Tinder.Responses.UserResponse;
using Tintool.APIs.Tinder.Responses.LikeResponse;
using Tintool.APIs.Tinder.Responses.ProfileResponse;
using Tintool.APIs.Tinder.Responses.MessageResponse;

namespace Models
{
    public class TinderAPI
    {
        private SessionModel _session;
        private string _uri = "https://api.gotinder.com/";
        HttpClientHandler handler;
        HttpClient client;
        private Random rand;
        private string _lastID = "";

        public TinderAPI()
        {
            rand = new Random();
            handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(_uri);
        }


        public List<MessageModel> GetMessages(string matchID, int amount = 100)
        {
            Delay();
            HttpResponseMessage response = client.GetAsync($"/v2/matches/{matchID}/messages?count={amount}").Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            List<MessageModel> result = new List<MessageModel>();
            foreach (Tintool.APIs.Tinder.Responses.MessageResponse.Message msg in JsonSerializer.Deserialize<MessagesResponse>(textResponse).data.messages)
            {
                MessageModel nextMessage = new MessageModel
                {
                    Text = msg.message,
                    ReceiverId = msg.from,
                    Date = msg.sent_date,
                };
                result.Add(nextMessage);
            }

            return result;
        }

        public List<MatchModel> GetMatches(int amount)
        {
            HttpResponseMessage response = client.GetAsync("/v2/matches?count=" + amount).Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            List<MatchModel> result = new List<MatchModel>();
            foreach(Tintool.APIs.Tinder.Responses.MatchesResponse.Match match in JsonSerializer.Deserialize<MatchesResponse>(textResponse).data.matches)
            {
                MatchModel newMatchData = new MatchModel(match);
                newMatchData.MatcherID = _lastID;
                result.Add(newMatchData);
            }

            return result;
        }

        public List<PersonModel> GetNearby()
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/v2/recs/core").Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            List<PersonModel> result = new List<PersonModel>();
            foreach(Result person in JsonSerializer.Deserialize<NearbyResponse>(textResponse).data.results)
            {
                PersonModel newPerson = new PersonModel
                {
                    Id = person.user._id,
                    Name = person.user.name,
                    SCode = ""+person.s_number
                };
                result.Add(newPerson);
            }
            return result;
        }

        public PersonModel GetUser(string userID)
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
                PersonModel result = new PersonModel
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

        [Obsolete("API requires sending SCode now")]
        public LikeModel SendLike(string userID)
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/like/"+userID).Result;


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            LikeModel result = new LikeModel();
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
                    result.ResultingMatch = new MatchModel(likeAndMatchResponse.match);
                    result.ResultingMatch.MatcherID = _lastID;
                    result.LikesRemaining = likeAndMatchResponse.likes_remaining;
                }
                catch (System.Text.Json.JsonException e2)
                {
                    return null;
                }
            }
            return result;
        }

        public LikeModel SendLike(string userID, string sCode)
        {
            Delay();
            string payloadContent = "{\"s_number\":" + sCode + "}";
            var payload = new StringContent(payloadContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("/like/" + userID, payload).Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            LikeModel result = new LikeModel();
            try
            {
                LikeWithoutMatchResponse likeWithoutMatchResponse = JsonSerializer.Deserialize<LikeWithoutMatchResponse>(textResponse);
                result.LikesRemaining = likeWithoutMatchResponse.likes_remaining;
            }
            catch (System.Text.Json.JsonException e)
            {
                try
                {
                    LikeAndMatchResponse likeAndMatchResponse = JsonSerializer.Deserialize<LikeAndMatchResponse>(textResponse);
                    result.ResultingMatch = new MatchModel(likeAndMatchResponse.match);
                    result.ResultingMatch.MatcherID = _lastID;
                    result.LikesRemaining = likeAndMatchResponse.likes_remaining;
                }
                catch (System.Text.Json.JsonException e2)
                {
                    return null;
                }
            }
            return result;
        }

        #region authentication
        public SessionModel TryRefresh(SessionModel session)
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

            string payloadContent = "RR\nP" + session.RefreshToken;

            Delay();
            var payload = new StringContent(payloadContent, Encoding.UTF8, "application/x-google-protobuf");
            HttpResponseMessage response = client.PostAsync("/v3/auth/login", payload).Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string responseAsString = response.Content.ReadAsStringAsync().Result;

            int tokenStartIndex = responseAsString.IndexOf("$") + 2;
            int tokenEndIndex = responseAsString.IndexOf("\"");
            int tokenLength = tokenEndIndex - tokenStartIndex;

            int refStartIndex = responseAsString.IndexOf("B�\u0001\nP") + 5;
            int refEndIndex = responseAsString.IndexOf("$");
            int refLength = refEndIndex - refStartIndex;

            SessionModel newSession = new SessionModel();
            if (tokenLength == 36 && tokenStartIndex - 2 != -1 && tokenEndIndex != -1)
            {
                if (refLength > 0 && refStartIndex - 5 != -1 && refEndIndex != -1)
                {
                    newSession.AuthToken = responseAsString.Substring(tokenStartIndex, tokenLength);
                    newSession.RefreshToken = responseAsString.Substring(refStartIndex, refLength);
                    return newSession;
                }
                else
                {
                    return null;
                }
            }

            if (refLength > 0 && refStartIndex - 5 != -1 && refEndIndex != -1)
            {
                if (tokenLength == 36 && tokenStartIndex - 2 != -1 && tokenEndIndex != -1)
                {
                    newSession.AuthToken = responseAsString.Substring(tokenStartIndex, tokenLength);
                    newSession.RefreshToken = responseAsString.Substring(refStartIndex, refLength);
                    return newSession;
                }
                else
                {
                    newSession.AuthToken = "";
                    newSession.RefreshToken = responseAsString.Substring(refStartIndex, refLength);
                    return newSession;
                }
            }
            return null;
        }
        public bool RequestLoginCode(string phoneNumber)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("tinder-version", "2.64.0");
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36");
            client.DefaultRequestHeaders.Add("platform", "web");
            client.DefaultRequestHeaders.Add("persistent-device-id", "89621f05-6135-426c-b74e-8b4a850ff1d8");
            client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate");
            client.DefaultRequestHeaders.Add("app-version", "1026400");
            client.DefaultRequestHeaders.Add("app-session-id", "180c1aab-4c2a-41b1-afad-cf713c1c8f3f");
            client.DefaultRequestHeaders.Add("x-supported-image-formats", "webp");
            client.DefaultRequestHeaders.Add("funnel-session-id", "f0b6eb6320d8d3fe");
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
        public SessionModel RequestNewSessionWithPhoneCode(string code, string phoneNumber)
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
                return null;
            }

            string responseAsString = response.Content.ReadAsStringAsync().Result;

            int tokenStartIndex = responseAsString.IndexOf("$")+2;
            int tokenEndIndex = responseAsString.IndexOf("\"");
            int tokenLength = tokenEndIndex - tokenStartIndex;

            int refStartIndex = responseAsString.IndexOf("B�\u0001\nP") + 5;
            int refEndIndex = responseAsString.IndexOf("$");
            int refLength = refEndIndex - refStartIndex;

            SessionModel session = new SessionModel();
            if (tokenLength == 36 && tokenStartIndex - 2 != -1 && tokenEndIndex != -1)
            {
                if (refLength > 0 && refStartIndex-5!=-1 && refEndIndex!=-1)
                {
                    session.AuthToken = responseAsString.Substring(tokenStartIndex, tokenLength);
                    session.RefreshToken = responseAsString.Substring(refStartIndex, refLength);
                    return session;
                }
                else
                {
                    return null;
                }
            }
            
            if(refLength > 0 && refStartIndex - 5 != -1 && refEndIndex != -1)
            {
                if (tokenLength == 36 && tokenStartIndex - 2 != -1 && tokenEndIndex != -1)
                {
                    session.AuthToken = responseAsString.Substring(tokenStartIndex, tokenLength);
                    session.RefreshToken = responseAsString.Substring(refStartIndex, refLength);
                    return session;
                }
                else
                {
                    session.AuthToken = "";
                    session.RefreshToken = responseAsString.Substring(refStartIndex, refLength);
                    return session;
                }
            }
            return null;
        }
        public SessionModel RequestNewSessionWithEmailCode(string code, SessionModel session)
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

            string payloadContent = "*\\"+code+"R\n" + session.RefreshToken;

            Delay();
            var payload = new StringContent(payloadContent, Encoding.UTF8, "application/x-google-protobuf");
            HttpResponseMessage response = client.PostAsync("/v3/auth/login", payload).Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return null;
            }

            string responseAsString = response.Content.ReadAsStringAsync().Result;

            int tokenStartIndex = responseAsString.IndexOf("$") + 2;
            int tokenEndIndex = responseAsString.IndexOf("\"");
            int tokenLength = tokenEndIndex - tokenStartIndex;

            if (tokenLength == 36 && tokenStartIndex - 2 != -1 && tokenEndIndex != -1)
            {
                session.AuthToken = responseAsString.Substring(tokenStartIndex, tokenLength);
            }
            else
            {
                return null;
            }

            return session;
        }

        public bool IsTokenWorking(bool quickCheck = false)
        {
            if (_session != null)
            {
                if (!quickCheck)
                {
                    Delay();
                }

                HttpResponseMessage response = client.GetAsync("/v2/recs/core").Result;
                return response.IsSuccessStatusCode;
            }
            else
            {
                return false;
            }
        }
        public void SetSession(SessionModel newSession)
        {
            this._session = newSession;
            client.DefaultRequestHeaders.Add("x-auth-token", newSession.AuthToken);
        }
        public SessionModel GetSession()
        {
            return _session;
        }
        #endregion

        public string GetProfileID()
        {
            Delay();
            HttpResponseMessage response = client.GetAsync("/v2/profile?locale=en&include=likes%2Cplus_control%2Cproducts%2Cpurchase%2Cuser").Result;

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                _lastID = "";
                return null;
            }

            string textResponse = response.Content.ReadAsStringAsync().Result;

            ProfileResponse profileResponse = JsonSerializer.Deserialize<ProfileResponse>(textResponse);
            if (profileResponse?.data?.user!=null)
            {
                _lastID = profileResponse.data.user._id;
                return profileResponse.data.user._id;
            }
            else
            {
                _lastID = "";
                return null;
            }
        }

        private void Delay()
        {
            System.Threading.Thread.Sleep(rand.Next() % 100 + 50);
        }
    }
}

