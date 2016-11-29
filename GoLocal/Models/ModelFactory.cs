using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GoLocal.Models
{
    public class ModelFactory
    {
        public CommentModel Wrap (Comment comment)
        {
            return new CommentModel()
            {
                ID = comment.ID,
                Content = comment.Content,
                Timestamp = comment.Timestamp,
                UserId = comment.User.ID,
                UserName = comment.User.Name,
                FeedId = comment.Feed.ID,
                FeedTitle = comment.Feed.Title
            };
        }

        public FeedModel Wrap (Feed feed)
        {
            return new FeedModel()
            {
                ID = feed.ID,
                Title = feed.Title,
                Content = feed.Content,
                Timestamp = feed.Timestamp,
                UserID = feed.User.ID,
                UserName = feed.User.Name,
                LocationName = feed.LocationName,
                Lat = feed.Lat,
                Long = feed.Long,
                Comments = feed.FeedComments.Select(c => Wrap(c)).ToList(),
                Votes = feed.FeedVotes.Select(v => Wrap(v)).ToList(),
                UpVote = feed.Upvote(),
                DownVote = feed.Downvote()
            };
        }

        public VoteModel Wrap(Vote vote)
        {
            return new VoteModel()
            {
                ID = vote.ID,
                UserID = vote.User.ID,
                UserName = vote.User.Name,
                FeedID = vote.Feed.ID,
                FeedTitle = vote.Feed.Title,
                Type = vote.Type,
                Timestamp = vote.Timestamp
            };
        }

        public UserModel Wrap(User user)
        {
            return new UserModel()
            {
                ID = user.ID,
                Email = user.Email,
                Name = user.Name,
                DateOfBirth = user.DateOfBirth,
                PhoneNumber = user.PhoneNumber,
                Comments = user.MyComments.Select(c => Wrap(c)).ToList(),
                Feeds = user.MyFeeds.Select(f => Wrap(f)).ToList(),
                Votes = user.MyVotes.Select(v => Wrap(v)).ToList()
            };
        }

        public Comment Unwrap(CommentModel commentModel)
        {
            return new Comment()
            {
                ID = commentModel.ID,
                Content = commentModel.Content,
                Timestamp = commentModel.Timestamp,
                UserID = commentModel.UserId,
                FeedID = commentModel.FeedId
            };
        }

        public Vote Unwrap(VoteModel voteModel)
        {
            return new Vote()
            {
                ID = voteModel.ID,
                UserID = voteModel.UserID,
                Timestamp = voteModel.Timestamp,
                FeedID = voteModel.FeedID,
                Type = voteModel.Type
            };
        }

        public Feed Unwrap(FeedModel feedModel)
        {
            return new Feed()
            {
                ID = feedModel.ID,
                Title = feedModel.Title,
                Content = feedModel.Content,
                Timestamp = feedModel.Timestamp,
                UserID = feedModel.UserID,
                LocationName = feedModel.LocationName,
                Lat = feedModel.Lat,
                Long = feedModel.Long,
                FeedComments = feedModel.Comments.Select(c => Unwrap(c)).ToList(),
                FeedVotes = feedModel.Votes.Select(v => Unwrap(v)).ToList()
            };
        }

        public User Unwrap(UserModel userModel)
        {
            return new User()
            {
                ID = userModel.ID,
                Email = userModel.Email,
                Name = userModel.Name,
                DateOfBirth = userModel.DateOfBirth,
                PhoneNumber = userModel.PhoneNumber,
                MyComments = userModel.Comments.Select(c => Unwrap(c)).ToList(),
                MyFeeds = userModel.Feeds.Select(f => Unwrap(f)).ToList(),
                MyVotes = userModel.Votes.Select(v => Unwrap(v)).ToList()
            };
        }
    }
}