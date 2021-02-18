using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.APIs.Tinder.Responses.UserResponse
{
    public class Image
    {
        public int height { get; set; }
        public int width { get; set; }
        public string url { get; set; }
    }

    public class Album
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Image> images { get; set; }
    }

    public class Artist
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class SpotifyThemeTrack
    {
        public string id { get; set; }
        public string name { get; set; }
        public Album album { get; set; }
        public List<Artist> artists { get; set; }
        public string preview_url { get; set; }
        public string uri { get; set; }
    }

    public class School
    {
        public string name { get; set; }
        public bool displayed { get; set; }
    }

    public class Teaser
    {
        public string type { get; set; }
        public string @string { get; set; }
    }

    public class SexualOrientation
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Badge
    {
        public string type { get; set; }
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

    public class CropInfo
    {
        public User user { get; set; }
        public Algo algo { get; set; }
        public bool processed_by_bullseye { get; set; }
        public bool user_customized { get; set; }
    }

    public class ProcessedFile
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
    }

    public class SelectedInterest
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class UserInterests
    {
        public List<SelectedInterest> selected_interests { get; set; }
    }

    public class City
    {
        public string name { get; set; }
    }

    public class Results
    {
        public List<object> common_friends { get; set; }
        public int common_friend_count { get; set; }
        public SpotifyThemeTrack spotify_theme_track { get; set; }
        public int distance_mi { get; set; }
        public int connection_count { get; set; }
        public List<object> common_connections { get; set; }
        public string bio { get; set; }
        public DateTime birth_date { get; set; }
        public string name { get; set; }
        public bool is_travelling { get; set; }
        public List<object> jobs { get; set; }
        public List<School> schools { get; set; }
        public List<Teaser> teasers { get; set; }
        public int gender { get; set; }
        public bool show_gender_on_profile { get; set; }
        public List<SexualOrientation> sexual_orientations { get; set; }
        public string birth_date_info { get; set; }
        public DateTime ping_time { get; set; }
        public bool show_orientation_on_profile { get; set; }
        public List<Badge> badges { get; set; }
        public List<Photo> photos { get; set; }
        public UserInterests user_interests { get; set; }
        public List<object> common_likes { get; set; }
        public int common_like_count { get; set; }
        public City city { get; set; }
        public List<object> common_interests { get; set; }
        public int s_number { get; set; }
        public string _id { get; set; }
        public bool is_tinder_u { get; set; }
    }

    public class UserResponse
    {
        public int status { get; set; }
        public Results results { get; set; }
    }


}
