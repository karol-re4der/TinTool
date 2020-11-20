using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

    public class MatchData
    {
        public string Id { get; set; }
        public ResponseStatusTypes ResponseStatus { get; set; } = ResponseStatusTypes.Undefined;
        public int MessageCount { get; set; } = 0;
        public DateTime CreationDate { get; set; }
        public PersonData Person { get; set; }
        public bool Active { get; set; } = true;
        public List<MessageData> Conversation { get; set; }

        public MatchData()
        {

        }
        public MatchData(Tinder.DataStructures.Responses.Matches.Match match)
        {
            Id = match.id;
            CreationDate = match.created_date;
            Person = new PersonData
            {
                Name = match.person.name,
                Id = match.person._id
            };
        }
        public MatchData(Responses.Like.Match match)
        {
            Id = match._id;
            CreationDate = match.created_date;
        }

    }
}
