using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.Models.DataStructures.Responses.Nearby
{
    public class Meta
    {
        public int status { get; set; }
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
        public string _id { get; set; }
        public List<Badge> badges { get; set; }
        public string bio { get; set; }
        public DateTime birth_date { get; set; }
        public string name { get; set; }
        public List<Photo> photos { get; set; }
        public int gender { get; set; }
        public List<Job> jobs { get; set; }
        public List<School> schools { get; set; }
        public bool show_gender_on_profile { get; set; }
        public List<SexualOrientation> sexual_orientations { get; set; }
        public bool recently_active { get; set; }
        public bool? is_traveling { get; set; }
        public City city { get; set; }
        public bool? hide_age { get; set; }
        public bool? hide_distance { get; set; }
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
        public bool processed_by_bullseye { get; set; }
        public bool user_customized { get; set; }
        public User user { get; set; }
        public Algo algo { get; set; }
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
        public List<ProcessedVideo> processedVideos { get; set; }
        public bool? main { get; set; }
    }

    public class Title
    {
        public string name { get; set; }
    }

    public class Job
    {
        public Title title { get; set; }
    }

    public class School
    {
        public string name { get; set; }
    }

    public class SexualOrientation
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class City
    {
        public string name { get; set; }
    }

    public class Facebook
    {
        public List<object> common_connections { get; set; }
        public int connection_count { get; set; }
        public List<object> common_interests { get; set; }
    }

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

    public class TopTrack
    {
        public string id { get; set; }
        public string name { get; set; }
        public Album album { get; set; }
        public List<Artist> artists { get; set; }
        public string preview_url { get; set; }
        public string uri { get; set; }
    }

    public class SpotifyTopArtist
    {
        public string id { get; set; }
        public string name { get; set; }
        public TopTrack top_track { get; set; }
        public bool selected { get; set; }
    }

    public class Spotify
    {
        public bool spotify_connected { get; set; }
        public SpotifyThemeTrack spotify_theme_track { get; set; }
        public List<SpotifyTopArtist> spotify_top_artists { get; set; }
    }

    public class Teaser
    {
        public string type { get; set; }
        public string @string { get; set; }
    }

    public class Teaser2
    {
        public string type { get; set; }
        public string @string { get; set; }
    }

    public class SelectedInterest
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool is_common { get; set; }
    }

    public class UserInterests
    {
        public List<SelectedInterest> selected_interests { get; set; }
    }

    public class ExperimentInfo
    {
        public UserInterests user_interests { get; set; }
    }

    public class Instagram
    {
        public DateTime last_fetch_time { get; set; }
        public bool completed_initial_fetch { get; set; }
        public int media_count { get; set; }
    }

    public class Result
    {
        public string type { get; set; }
        public string rec_type { get; set; }
        public User user { get; set; }
        public Facebook facebook { get; set; }
        public Spotify spotify { get; set; }
        public int distance_mi { get; set; }
        public string content_hash { get; set; }
        public int s_number { get; set; }
        public Teaser teaser { get; set; }
        public List<Teaser> teasers { get; set; }
        public ExperimentInfo experiment_info { get; set; }
        public bool is_superlike_upsell { get; set; }
        public Instagram instagram { get; set; }
    }

    public class Data
    {
        public List<Result> results { get; set; }
    }

    public class NearbyResponse
    {
        public Meta meta { get; set; }
        public Data data { get; set; }
    }
}
