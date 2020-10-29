using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models.DataStructures.Responses.Like
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    namespace Tintool.Models.DataStructures.Responses.Like
    {
        public class Match
        {
            public string _id { get; set; }
            public bool closed { get; set; }
            public int common_friend_count { get; set; }
            public int common_like_count { get; set; }
            public DateTime created_date { get; set; }
            public bool dead { get; set; }
            public bool following { get; set; }
            public bool following_moments { get; set; }
            public DateTime last_activity_date { get; set; }
            public int message_count { get; set; }
            public List<object> messages { get; set; }
            public bool muted { get; set; }
            public List<string> participants { get; set; }
            public bool pending { get; set; }
            public bool is_super_like { get; set; }
            public bool is_boost_match { get; set; }
            public bool is_super_boost_match { get; set; }
            public bool is_fast_match { get; set; }
            public bool is_top_pick { get; set; }
            public bool is_experiences_match { get; set; }
        }

        public class LikeWithoutMatchResponse : LikeResponse
        {

            public bool match { get; set; }
        }

        public class LikeAndMatchResponse : LikeResponse
        {
            public Match match { get; set; }

        }

        public class LikeResponse
        {
            public int status { get; set; }
            public int likes_remaining { get; set; }
            public string XPadding { get; set; }
        }
    }
}
