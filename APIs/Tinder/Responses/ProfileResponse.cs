using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.APIs.Tinder.Responses.ProfileResponse
{
    public class Meta
    {
        public int status { get; set; }
    }

    public class Country
    {
        public string name { get; set; }
        public string cc { get; set; }
        public string alpha3 { get; set; }
    }

    public class PosInfo
    {
        public Country country { get; set; }
        public string timezone { get; set; }
    }

    public class LanguagePreference
    {
        public string language { get; set; }
        public bool is_selected { get; set; }
    }

    public class GlobalMode
    {
        public string display_language { get; set; }
        public List<LanguagePreference> language_preferences { get; set; }
    }

    public class User
    {
        public int width_pct { get; set; }
        public int x_offset_pct { get; set; }
        public double height_pct { get; set; }
        public double y_offset_pct { get; set; }
        public string _id { get; set; }
        public int age_filter_max { get; set; }
        public int age_filter_min { get; set; }
        public string bio { get; set; }
        public DateTime birth_date { get; set; }
        public DateTime create_date { get; set; }
        public string crm_id { get; set; }
        public PosInfo pos_info { get; set; }
        public bool discoverable { get; set; }
        public int distance_filter { get; set; }
        public GlobalMode global_mode { get; set; }
        public int gender { get; set; }
        public int gender_filter { get; set; }
        public bool show_gender_on_profile { get; set; }
        public string name { get; set; }
        public List<Photo> photos { get; set; }
        public bool photos_processing { get; set; }
        public bool photo_optimizer_enabled { get; set; }
        public DateTime ping_time { get; set; }
        public List<object> jobs { get; set; }
        public List<School> schools { get; set; }
        public List<Badge> badges { get; set; }
        public string phone_id { get; set; }
        public List<int> interested_in { get; set; }
        public Pos pos { get; set; }
        public BillingInfo billing_info { get; set; }
        public string autoplay_video { get; set; }
        public bool top_picks_discoverable { get; set; }
        public bool photo_tagging_enabled { get; set; }
        public bool show_orientation_on_profile { get; set; }
        public ShowSameOrientationFirst show_same_orientation_first { get; set; }
        public List<SexualOrientation> sexual_orientations { get; set; }
        public UserInterests user_interests { get; set; }
        public bool recommended_sort_discoverable { get; set; }
        public string selfie_verification { get; set; }
        public bool noonlight_protected { get; set; }
        public bool sync_swipe_enabled { get; set; }
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
        public int faces_count { get; set; }
    }

    public class ProcessedFile
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class Asset
    {
        public string url { get; set; }
        public string format { get; set; }
        public DateTime created_at { get; set; }
        public List<string> enhancements { get; set; }
        public double face_ratio { get; set; }
        public double requested_face_ratio { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int qf { get; set; }
        public string asset_type { get; set; }
    }

    public class Photo
    {
        public string id { get; set; }
        public CropInfo crop_info { get; set; }
        public string url { get; set; }
        public string fbId { get; set; }
        public List<ProcessedFile> processedFiles { get; set; }
        public List<Asset> assets { get; set; }
        public string media_type { get; set; }
    }

    public class School
    {
        public string name { get; set; }
        public bool displayed { get; set; }
    }

    public class Badge
    {
        public string type { get; set; }
    }

    public class Pos
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class BillingInfo
    {
        public List<string> supported_payment_methods { get; set; }
    }

    public class ShowSameOrientationFirst
    {
        public bool @checked { get; set; }
        public bool should_show_option { get; set; }
    }

    public class SexualOrientation
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class SelectedInterest
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class AvailableInterest
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class UserInterests
    {
        public List<SelectedInterest> selected_interests { get; set; }
        public List<AvailableInterest> available_interests { get; set; }
        public int min_interests { get; set; }
        public int max_interests { get; set; }
    }

    public class Data
    {
        public User user { get; set; }
    }

    public class ProfileResponse
    {
        public Meta meta { get; set; }
        public Data data { get; set; }
    }
}
