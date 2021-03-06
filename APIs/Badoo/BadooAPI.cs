using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Tintool.Models.Saveables;

namespace Tintool.APIs.Badoo
{
    class BadooAPI
    {
        private SessionModel _session;
        private string _uri = "https://badoo.com/webapi.phtml";
        HttpClientHandler handler;
        HttpClient client;
        private Random rand;
        private string _lastID = "";
        private string _sessionCookieHeader = "session_cookie_name=session; session=";

        public BadooAPI()
        {
            rand = new Random();
            handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri(_uri);
        }

        #region authentication
        public bool PrepareSession()
        {
            client.DefaultRequestHeaders.Clear();

            string payloadContent = "{\"$gpb\":\"badoo.bma.BadooMessage\",\"body\":[{\"message_type\":2,\"server_app_startup\":{\"app_build\":\"Badoo\",\"app_name\":\"hotornot\",\"app_version\":\"1.0.00\",\"can_send_sms\":false,\"user_agent\":\"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.182 Safari/537.36\",\"screen_width\":1920,\"screen_height\":1080,\"language\":0,\"locale\":\"en-US\",\"app_platform_type\":5,\"app_product_type\":100,\"device_info\":{\"screen_density\":1,\"form_factor\":3},\"build_configuration\":2,\"supported_features\":[1,58,2,4,6,7,8,172,9,10,11,13,15,18,19,20,21,25,27,28,29,32,34,35,46,36,37,39,42,44,54,62,64,70,73,75,78,96,100,103,105,106,107,108,109,111,113,114,125,129,91,116,136,104,132,142,127,169,161,183,179,209,197,248,259,243,237,148],\"supported_minor_features\":[292,444,93,267,40,41,12,22,59,61,52,25,21,74,31,129,24,19,115,48,86,90,81,245,132,118,36,125,65,143,80,131,114,251,89,104,8,135,137,2,148,83,164,163,20,171,142,157,102,139,103,179,207,180,188,189,219,202,187,178,136,226,146,210,169,208,218,196,122,127,175,194,134,244,253,183,214,184,181,182,259,242,261,306,130,269,268,266,313,153,168,230,63,305,285,274,348,328,364,254,291,394,382,365,403,280,396,420,470,474,397,493,483,576,696,440,450,548,549,390,530,391,537,290,39,115,605,620,627],\"supported_notifications\":[100,3,25,40,41,47,50,55,39,38,42,62,60,66,76,73,81,96,98,108,35,33,44],\"supported_promo_blocks\":[{\"context\":1,\"position\":2,\"types\":[8,37,56,57]},{\"context\":1,\"position\":21,\"types\":[10]},{\"context\":2,\"position\":1,\"types\":[8,37,56,57]},{\"context\":2,\"position\":2,\"types\":[8,37,56,57]},{\"context\":22,\"position\":1,\"types\":[8,37,56,57]},{\"context\":22,\"position\":2,\"types\":[8,37,56,57]},{\"context\":6,\"position\":1,\"types\":[8,37,56,57]},{\"context\":6,\"position\":2,\"types\":[8,37,56,57]},{\"context\":23,\"position\":1,\"types\":[8,37,56,57]},{\"context\":23,\"position\":2,\"types\":[8,37,56,57]},{\"context\":26,\"position\":1,\"types\":[165,56,7,57]},{\"context\":26,\"position\":4,\"types\":[165]},{\"context\":26,\"position\":10,\"types\":[143]},{\"context\":10,\"position\":1,\"types\":[70]},{\"context\":32,\"position\":1,\"types\":[8,1,9,10,56,57]},{\"context\":45,\"position\":13,\"types\":[12]},{\"context\":45,\"position\":15,\"types\":[137,222,1,9,10,56,230,258,285,333,187,153]},{\"context\":92,\"position\":13,\"types\":[68,71,210,12]},{\"context\":106,\"position\":13,\"types\":[122]},{\"context\":43,\"position\":1,\"types\":[1,9,10,24,22,48,59,56]},{\"context\":43,\"position\":4,\"types\":[164]},{\"context\":27,\"position\":1,\"types\":[7,3,6,43,37,4,5]},{\"context\":27,\"position\":4,\"types\":[164]},{\"context\":35,\"position\":1,\"types\":[4,5,35,40,37]},{\"context\":35,\"position\":4,\"types\":[7,6]},{\"context\":35,\"position\":13,\"types\":[8]},{\"context\":151,\"position\":8,\"types\":[7,6,5,4,43,40,37,9,1,10,11,56,57]},{\"context\":153,\"position\":1,\"types\":[193]},{\"context\":153,\"position\":13,\"types\":[193]},{\"context\":153,\"position\":21,\"types\":[193]},{\"context\":153,\"position\":22,\"types\":[194]},{\"context\":3,\"position\":4,\"types\":[8]},{\"context\":3,\"position\":20,\"types\":[326]},{\"context\":245,\"position\":13,\"types\":[48]},{\"context\":53,\"position\":13,\"types\":[92]}],\"supported_onboarding_types\":[2,1,23,30,32,33],\"supported_payment_providers\":[110004,100,35,100001,26,102,143,501,502,170,160],\"supported_streaming_sdk\":[3,5,6],\"build_fingerprint\":\"23739\",\"start_source\":{\"current_url\":\"https://badoo.com/en/signin/?f=top\",\"http_referrer\":\"https://badoo.com/en/mobile/\"},\"user_field_filter_webrtc_start_call\":{\"projection\":[200]},\"hotpanel_session_id\":\"3b9240f2-fb68-41d1-8dc5-94d6a799c3c4\",\"device_id\":\"fccca394-a394-9470-7043-43441e9b168b\",\"a_b_testing_settings\":{\"tests\":[{\"test_id\":\"badoo__web__liked_you_screen_\"},{\"test_id\":\"badoo_dw__nonbinary_gender\"},{\"test_id\":\"badoo_web_profile_onboarding\"},{\"test_id\":\"cach_first_encounter_card\"},{\"test_id\":\"web_chat_filter\"},{\"test_id\":\"badoo_web_empty_encounter_profile\"},{\"test_id\":\"encounter_40\"},{\"test_id\":\"encounter_smart_about_me\"},{\"test_id\":\"log_out_instead_of_delete\"},{\"test_id\":\"web_fullscreen_paywall\"},{\"test_id\":\"new_entry_point_for_simplified_profile_quality_walk_through\"},{\"test_id\":\"xpdw__profile_quality_walkthrough_payer\"}]},\"dev_features\":[\"refactoring_gallery\",\"simplified_verification\",\"new_cookies_consent\"]}}],\"message_id\":1,\"message_type\":2,\"version\":1,\"is_background\":false}";

            Delay();
            var payload = new StringContent(payloadContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("?SERVER_APP_STARTUP", payload).Result;

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            string responseAsString = response.Headers.Where((x) => x.Key.Equals("Set-Cookie")).First().Value.ElementAt(1).ToString();

            int startIndex = responseAsString.IndexOf("=")+1;
            int endIndex = responseAsString.IndexOf(";");
            int length = endIndex-startIndex;

            if(startIndex>0 && endIndex>0 && length > 0)
            {
                string sessionString = responseAsString.Substring(startIndex, length);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Cookie", _sessionCookieHeader+sessionString);
                return true;
            }
            else
            {
                return false;
            }
        }
        public SessionModel RequestNewSessionWithCredentials(string number, string password)
        {
            if (!PrepareSession())
            {
                return null;
            }

            string payloadContent = 
                "{\"$gpb\":\"badoo.bma.BadooMessage\",\"body\":[{\"message_type\":15,\"server_login_by_password\":{\"remember_me\":true,\"user\":\""
                +number+
                "\",\"password\":\""
                +password+
                "\",\"stats_data\":\""
                +""+
                "\"}}],\"message_id\":2,\"message_type\":15,\"version\":1,\"is_background\":false}";

            Delay();
            var payload = new StringContent(payloadContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync("?SERVER_LOGIN_BY_PASSWORD", payload).Result;

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

            SessionModel session = new SessionModel();
            if (tokenLength == 36 && tokenStartIndex - 2 != -1 && tokenEndIndex != -1)
            {
                if (refLength > 0 && refStartIndex - 5 != -1 && refEndIndex != -1)
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

            if (refLength > 0 && refStartIndex - 5 != -1 && refEndIndex != -1)
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

        public bool IsTokenWorking(bool quickCheck = false)
        {
            if (_session != null)
            {
                if (!quickCheck)
                {
                    Delay();
                }

                //HttpResponseMessage response = client.GetAsync("/v2/recs/core").Result;
                //return response.IsSuccessStatusCode;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SetSession(SessionModel newSession)
        {
            this._session = newSession;
            //client.DefaultRequestHeaders.Add("x-auth-token", newSession.AuthToken);
        }
        public SessionModel GetSession()
        {
            return _session;
        }
        #endregion

        private void Delay()
        {
            System.Threading.Thread.Sleep(rand.Next() % 100 + 50);
        }
    }
}
