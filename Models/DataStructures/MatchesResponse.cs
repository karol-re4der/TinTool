using System;
using System.Collections.Generic;
using System.Text;

namespace Tinder.DataStructures.Responses.Matches
{

    public class Meta
    {
        public int status { get; set; }
    }

    public class Seen
    {
        public bool match_seen { get; set; }
        public string last_seen_msg_id { get; set; }
    }

    public class Message
    {
        public string _id { get; set; }
        public string match_id { get; set; }
        public DateTime sent_date { get; set; }
        public string message { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public int timestamp { get; set; }
        public bool? is_liked { get; set; }
    }

    public class User
    {
        public double width_pct { get; set; }
        public double x_offset_pct { get; set; }
        public double height_pct { get; set; }
        public double y_offset_pct { get; set; }
    }

    public class Algo
    {
        public double width_pct { get; set; }
        public double x_offset_pct { get; set; }
        public double height_pct { get; set; }
        public double y_offset_pct { get; set; }
    }

    public class Face
    {
        public double bounding_box_percentage { get; set; }
    }

    public class CropInfo
    {
        public User user { get; set; }
        public Algo algo { get; set; }
        public bool processed_by_bullseye { get; set; }
        public bool user_customized { get; set; }
        public List<Face> faces { get; set; }
    }

    public class ProcessedFile
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class ProcessedVideo
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class Photo
    {
        public string id { get; set; }
        public CropInfo crop_info { get; set; }
        public string url { get; set; }
        public List<ProcessedFile> processedFiles { get; set; }
        public string fileName { get; set; }
        public string extension { get; set; }
        public List<int> webp_qf { get; set; }
        public int? rank { get; set; }
        public double? score { get; set; }
        public int? win_count { get; set; }
        public List<ProcessedVideo> processedVideos { get; set; }
        public DateTime? last_update_time { get; set; }
        public bool? main { get; set; }
    }

    public class Badge
    {
        public string type { get; set; }
    }

    public class Person
    {
        public string _id { get; set; }
        public string bio { get; set; }
        public DateTime birth_date { get; set; }
        public int gender { get; set; }
        public string name { get; set; }
        public DateTime ping_time { get; set; }
        public List<Photo> photos { get; set; }
        public List<Badge> badges { get; set; }
    }

    public class Readreceipt
    {
        public bool enabled { get; set; }
    }

    public class Match
    {
        public Seen seen { get; set; }
        public string _id { get; set; }
        public string id { get; set; }
        public bool closed { get; set; }
        public int common_friend_count { get; set; }
        public int common_like_count { get; set; }
        public DateTime created_date { get; set; }
        public bool dead { get; set; }
        public DateTime last_activity_date { get; set; }
        public int message_count { get; set; }
        public List<Message> messages { get; set; }
        public bool muted { get; set; }
        public List<string> participants { get; set; }
        public bool pending { get; set; }
        public bool is_super_like { get; set; }
        public bool is_boost_match { get; set; }
        public bool is_super_boost_match { get; set; }
        public bool is_experiences_match { get; set; }
        public bool is_fast_match { get; set; }
        public bool is_opener { get; set; }
        public Person person { get; set; }
        public bool following { get; set; }
        public bool following_moments { get; set; }
        public Readreceipt readreceipt { get; set; }
        public bool? has_harassing_feedback { get; set; }
        public string harassing_message_id { get; set; }
    }

    public class Data
    {
        public List<Match> matches { get; set; }
    }

    public class MatchesResponse
    {
        public Meta meta { get; set; }
        public Data data { get; set; }
    }
}
