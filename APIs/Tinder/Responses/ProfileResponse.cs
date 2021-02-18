using System;
using System.Collections.Generic;
using System.Text;

namespace Tintool.APIs.Tinder.Responses.ProfileResponse
{
    public class Meta
    {
        public int status { get; set; }
    }

    public class Likes
    {
        public int likes_remaining { get; set; }
        public long rate_limited_until { get; set; }
    }

    public class PlusControl
    {
    }

    public class Sku
    {
        public string product_type { get; set; }
        public string purchase_type { get; set; }
        public string product_id { get; set; }
        public double price { get; set; }
        public int terms { get; set; }
        public bool is_best_value { get; set; }
        public double discount { get; set; }
        public bool require_zip { get; set; }
        public bool is_vat { get; set; }
        public double tax_rate { get; set; }
        public string currency { get; set; }
        public List<string> supported_payment_methods { get; set; }
        public string product_signature { get; set; }
        public bool? is_primary { get; set; }
        public bool? is_most_popular { get; set; }
        public bool? is_base_group { get; set; }
    }

    public class Regular
    {
        public List<Sku> skus { get; set; }
    }

    public class Plus
    {
        public Regular regular { get; set; }
    }

    public class Sku2
    {
        public string product_type { get; set; }
        public string purchase_type { get; set; }
        public string product_id { get; set; }
        public double price { get; set; }
        public int terms { get; set; }
        public bool is_best_value { get; set; }
        public double discount { get; set; }
        public bool require_zip { get; set; }
        public bool is_vat { get; set; }
        public double tax_rate { get; set; }
        public string currency { get; set; }
        public List<string> supported_payment_methods { get; set; }
        public string product_signature { get; set; }
        public bool? is_primary { get; set; }
        public bool? is_most_popular { get; set; }
        public bool? is_base_group { get; set; }
    }

    public class Regular2
    {
        public List<Sku2> skus { get; set; }
    }

    public class Gold
    {
        public Regular2 regular { get; set; }
    }

    public class Sku3
    {
        public string product_type { get; set; }
        public string purchase_type { get; set; }
        public string product_id { get; set; }
        public double price { get; set; }
        public int terms { get; set; }
        public bool is_best_value { get; set; }
        public double discount { get; set; }
        public bool require_zip { get; set; }
        public bool is_vat { get; set; }
        public double tax_rate { get; set; }
        public string currency { get; set; }
        public List<string> supported_payment_methods { get; set; }
        public string product_signature { get; set; }
        public bool? is_primary { get; set; }
        public bool? is_most_popular { get; set; }
        public bool? is_base_group { get; set; }
    }

    public class Regular3
    {
        public List<Sku3> skus { get; set; }
    }

    public class Platinum
    {
        public Regular3 regular { get; set; }
    }

    public class Sku4
    {
        public string product_type { get; set; }
        public string purchase_type { get; set; }
        public string product_id { get; set; }
        public double price { get; set; }
        public int amount { get; set; }
        public bool is_base_group { get; set; }
        public double discount { get; set; }
        public bool require_zip { get; set; }
        public bool is_vat { get; set; }
        public double tax_rate { get; set; }
        public string currency { get; set; }
        public List<string> supported_payment_methods { get; set; }
        public string product_signature { get; set; }
        public bool? is_primary { get; set; }
    }

    public class Regular4
    {
        public List<Sku4> skus { get; set; }
    }

    public class Boost
    {
        public Regular4 regular { get; set; }
    }

    public class Sku5
    {
        public string product_type { get; set; }
        public string purchase_type { get; set; }
        public string product_id { get; set; }
        public double price { get; set; }
        public int amount { get; set; }
        public bool is_base_group { get; set; }
        public double discount { get; set; }
        public bool require_zip { get; set; }
        public bool is_vat { get; set; }
        public double tax_rate { get; set; }
        public string currency { get; set; }
        public List<string> supported_payment_methods { get; set; }
        public string product_signature { get; set; }
        public bool? is_primary { get; set; }
    }

    public class Regular5
    {
        public List<Sku5> skus { get; set; }
    }

    public class Superlike
    {
        public Regular5 regular { get; set; }
    }

    public class Sku6
    {
        public string product_type { get; set; }
        public string purchase_type { get; set; }
        public string product_id { get; set; }
        public double price { get; set; }
        public int amount { get; set; }
        public int duration { get; set; }
        public bool is_base_group { get; set; }
        public double discount { get; set; }
        public bool require_zip { get; set; }
        public bool is_vat { get; set; }
        public double tax_rate { get; set; }
        public string currency { get; set; }
        public List<string> supported_payment_methods { get; set; }
        public string product_signature { get; set; }
        public bool? is_primary { get; set; }
    }

    public class Regular6
    {
        public List<Sku6> skus { get; set; }
    }

    public class Superboost
    {
        public Regular6 regular { get; set; }
    }

    public class Sku7
    {
        public string product_type { get; set; }
        public string purchase_type { get; set; }
        public string product_id { get; set; }
        public double price { get; set; }
        public int amount { get; set; }
        public bool is_base_group { get; set; }
        public double discount { get; set; }
        public bool require_zip { get; set; }
        public bool is_vat { get; set; }
        public double tax_rate { get; set; }
        public string currency { get; set; }
        public List<string> supported_payment_methods { get; set; }
        public string product_signature { get; set; }
        public bool? is_primary { get; set; }
    }

    public class Regular7
    {
        public List<Sku7> skus { get; set; }
    }

    public class Readreceipt
    {
        public Regular7 regular { get; set; }
    }

    public class Products
    {
        public Plus plus { get; set; }
        public Gold gold { get; set; }
        public Platinum platinum { get; set; }
        public Boost boost { get; set; }
        public Superlike superlike { get; set; }
        public Superboost superboost { get; set; }
        public Readreceipt readreceipt { get; set; }
    }

    public class Purchase
    {
        public List<object> purchases { get; set; }
        public bool subscription_expired { get; set; }
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

    public class User2
    {
        public int width_pct { get; set; }
        public int x_offset_pct { get; set; }
        public double height_pct { get; set; }
        public int y_offset_pct { get; set; }
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
        public User2 user { get; set; }
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

    public class Photo
    {
        public string id { get; set; }
        public CropInfo crop_info { get; set; }
        public string url { get; set; }
        public string fbId { get; set; }
        public List<ProcessedFile> processedFiles { get; set; }
    }

    public class Title
    {
        public bool displayed { get; set; }
        public string name { get; set; }
    }

    public class Job
    {
        public Title title { get; set; }
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

    public class City
    {
        public string name { get; set; }
        public string region { get; set; }
    }

    public class AvailableInterest
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class UserInterests
    {
        public List<object> selected_interests { get; set; }
        public List<AvailableInterest> available_interests { get; set; }
        public int min_interests { get; set; }
        public int max_interests { get; set; }
    }

    public class User
    {
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
        public List<Job> jobs { get; set; }
        public List<object> schools { get; set; }
        public List<Badge> badges { get; set; }
        public string phone_id { get; set; }
        public List<int> interested_in { get; set; }
        public Pos pos { get; set; }
        public string autoplay_video { get; set; }
        public bool top_picks_discoverable { get; set; }
        public bool photo_tagging_enabled { get; set; }
        public City city { get; set; }
        public UserInterests user_interests { get; set; }
        public bool recommended_sort_discoverable { get; set; }
        public string selfie_verification { get; set; }
        public bool noonlight_protected { get; set; }
        public bool sync_swipe_enabled { get; set; }
    }

    public class Data
    {
        public Likes likes { get; set; }
        public PlusControl plus_control { get; set; }
        public Products products { get; set; }
        public Purchase purchase { get; set; }
        public User user { get; set; }
    }

    public class ProfileResponse
    {
        public Meta meta { get; set; }
        public Data data { get; set; }
    }


}
