using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json.Serialization;

namespace Tintool.Models.DataStructures
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

    public class MatchData
    {
        public string Id { get; set; }
        public ResponseStatusTypes ResponseStatus { get; set; } = ResponseStatusTypes.Undefined;
        public MatchTypes MatchType { get; set; } = MatchTypes.Undefined;
        public int MessageCount { get; set; } = 0;
        public DateTime CreationDate { get; set; }
        public PersonData Person { get; set; }
        public bool Active { get; set; } = true;
        public List<MessageData> Conversation { get; set; }
        public string MatcherID { get; set; }

        public MatchData()
        {

        }
        public MatchData(Tinder.DataStructures.Responses.Matches.Match match)
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

            Person = new PersonData
            {
                Name = match.person.name,
                Id = match.person._id,
                Birthday = match.person.birth_date
            };
        }
        public MatchData(Responses.Like.Match match)
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

        public bool IsSameMatch(MatchData match)
        {
            if (match.Id.Equals(Id) && match.MatcherID.Equals(MatcherID))
            {
                return true;
            }
            return false;
        }
    }
}
