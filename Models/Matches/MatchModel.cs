using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json.Serialization;

namespace Tintool.Models
{
    public enum ResponseStatusTypes
    {
        Undefined,
        Empty,
        MessagedNotResponded,
        MessagedResponded,
        GotMessageResponded,
        GotMessageNotResponded
    }

    public enum MatchTypes
    {
        Undefined,
        Regular,
        Super,
        Boost,
        SuperBoost,
        Experiences,
        Fast
    }

    public enum MatchPlatform
    {
        Tinder,
        Badoo,
        Undefined
    }

    public class MatchModel
    {
        public string Id { get; set; }
        public ResponseStatusTypes ResponseStatus { get; set; } = ResponseStatusTypes.Undefined;
        public MatchTypes MatchType { get; set; } = MatchTypes.Undefined;
        public int MessageCount { get; set; } = 0;
        public DateTime CreationDate { get; set; }
        public PersonModel Person { get; set; }
        public bool Active { get; set; } = true;
        public List<MessageModel> Conversation { get; set; }
        public string MatcherID { get; set; }
        public MatchPlatform platform { get; set; } = MatchPlatform.Undefined;

        public MatchModel()
        {

        }
        public MatchModel(APIs.Tinder.Responses.MatchesResponse.Match match)
        {

            Id = match.id;
            CreationDate = match.created_date;

            if (match.is_super_like)
            {
                MatchType = MatchTypes.Super;
            }
            else if(match.is_super_boost_match)
            {
                MatchType = MatchTypes.SuperBoost;
            }
            else if (match.is_boost_match)
            {
                MatchType = MatchTypes.Boost;
            }
            else if (match.is_experiences_match)
            {
                MatchType = MatchTypes.Experiences;
            }
            else if (match.is_fast_match)
            {
                MatchType = MatchTypes.Fast;
            }
            else
            {
                MatchType = MatchTypes.Regular;
            }


            Person = new PersonModel
            {
                Name = match.person.name,
                Id = match.person._id,
                Birthday = match.person.birth_date
            };
        }
        public MatchModel(APIs.Tinder.Responses.LikeResponse.Match match)
        {
            Id = match._id;
            CreationDate = match.created_date;

            if (match.is_super_like)
            {
                MatchType = MatchTypes.Super;
            }
            else if (match.is_super_boost_match)
            {
                MatchType = MatchTypes.SuperBoost;
            }
            else if (match.is_boost_match)
            {
                MatchType = MatchTypes.Boost;
            }
            else if (match.is_experiences_match)
            {
                MatchType = MatchTypes.Experiences;
            }
            else if (match.is_fast_match)
            {
                MatchType = MatchTypes.Fast;
            }
            else
            {
                MatchType = MatchTypes.Regular;
            }
        }

        public bool IsSameMatch(MatchModel match)
        {
            if (match.Id.Equals(Id) && match.MatcherID.Equals(MatcherID))
            {
                return true;
            }
            return false;
        }
    }
}
