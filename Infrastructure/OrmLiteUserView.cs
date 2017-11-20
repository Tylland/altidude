using Altidude.Views;
using System;
using System.Linq;
using Altidude.Contracts.Events;
using ServiceStack.OrmLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Altidude.Contracts.Types;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Altidude.Contracts;
using System.Data;
using System.Collections.Generic;
using Altidude.Domain;
using Altidude.Domain.Aggregates.Profile;

namespace Altidude.Infrastructure
{
    public class OrmLiteUserView : IUserView, IUserService, IHandleEvent<UserCreated>, IHandleEvent<UserSettingsUpdated>, IHandleEvent<UserGainedExperience>, IHandleEvent<UserGainedLevel>, IHandleEvent<UserFollowed>, IHandleEvent<ProfileCreated>, IHandleEvent<FollowingUsersCleared>
    {
        private IDbConnection _db;

        public OrmLiteUserView(IDbConnection db)
        {
            _db = db;
        }

        private User CreateUser(UserView view)
        {
            if (view == null)
                return null;

            var profileSummary = new UserProfileSummary(view.Distance, view.Ascending, view.Descending, view.HighestAltitude, view.LowestAltitude, view.NrOfClimbs, view.ClimbPoints, view.TimeSeconds);

            return new User(view.Id, view.UserName, view.Email, view.FirstName, view.LastName, view.AcceptsEmails, view.ExperiencePoints, view.Level, view.FollowingUserIds, view.FollowedByUserIds, profileSummary);
        }

        public User GetById(Guid id)
        {
            var view = _db.GetByIdOrDefault<UserView>(id);
            Debug.WriteLine(_db.GetLastSql());

            return CreateUser(view);
        }
        public List<User> GetAll()
        {
            var views = _db.Select<UserView>().OrderByDescending(row => row.CreatedTime);
            Debug.WriteLine(_db.GetLastSql());

            return views.Select(view => CreateUser(view)).ToList();
        }

        public void Handle(UserCreated evt)
        {
            var view = new UserView();
            view.Id = evt.Id;
            view.UserName = evt.UserName;
            view.Email = evt.Email;
            view.FirstName = evt.FirstName;
            view.LastName = evt.LastName;
            view.AcceptsEmails = true;
            view.ExperiencePoints = 0;
            view.Level = 0;
            view.CreatedTime = evt.CreatedTime;


            _db.Insert(view);
        }

        public void Handle(UserGainedExperience evt)
        {
           _db.Update<UserView>(new { ExperiencePoints = evt.TotalPoints }, p => p.Id == evt.Id);
        }

        public void Handle(UserSettingsUpdated evt)
        {
            _db.Update<UserView>(new { FirstName = evt.FirstName, LastName = evt.LastName, AcceptsEmails = evt.AcceptsEmails}, p => p.Id == evt.Id);
        }

        public void Handle(UserGainedLevel evt)
        {
            _db.Update<UserView>(new { Level = evt.Level.Level }, p => p.Id == evt.Id);
        }

        public void Handle(UserFollowed evt)
        {
            var user = _db.GetById<UserView>(evt.Id);

            if(user.FollowingUserIds == null)
                user.FollowingUserIds = new List<Guid>();

            user.FollowingUserIds.Add(evt.OtherUserId);

            _db.Update(user);


            var otherUser = _db.GetById<UserView>(evt.OtherUserId);

            if (otherUser.FollowedByUserIds == null)
                otherUser.FollowedByUserIds = new List<Guid>();

            otherUser.FollowedByUserIds.Add(evt.Id);

            _db.Update(otherUser);
        }
        public void Handle(UserUnfollowed evt)
        {
            var user = _db.GetById<UserView>(evt.Id);
            user.FollowedByUserIds.Remove(evt.OtherUserId);
            _db.Update<UserView>(user);

            var followingUser = _db.GetById<UserView>(evt.Id);
            followingUser.FollowingUserIds.Remove(evt.Id);
            _db.Update<UserView>(followingUser);
        }
        public void Handle(FollowingUsersCleared evt)
        {
            var user = _db.GetById<UserView>(evt.Id);

            user.FollowingUserIds.Clear();
            user.FollowedByUserIds.Clear();

            _db.Update<UserView>(user);
        }

        public void Handle(ProfileCreated evt)
        {
            var user = _db.GetById<UserView>(evt.UserId);

            var nrOfProfiles = user.NrOfProfiles + 1;
            var distance = user.Distance;
            var ascending = user.Ascending;
            var descending = user.Descending;
            var highestAltitude = user.HighestAltitude;
            var lowestAltitude = user.LowestAltitude;
            var nrOfClimbs = user.NrOfClimbs;
            var climbPoints = user.ClimbPoints;
            var timeSeconds = user.TimeSeconds;

            if (evt.HighestPoint == null && evt.LowestPoint == null && evt.Track != null)
            {
                var trackData = new TrackAnalyzer().Analyze(evt.Track);

                distance += evt.Track.Length;
                ascending += trackData.Ascending;
                descending += trackData.Descending;

                if (trackData.HighestPoint != null)
                    highestAltitude = Math.Max(user.HighestAltitude, trackData.HighestPoint.Altitude);


                if (trackData.LowestPoint != null)
                    lowestAltitude = Math.Min(user.LowestAltitude, trackData.LowestPoint.Altitude);

                if (trackData.Climbs != null)
                    nrOfClimbs += trackData.Climbs.Length;


                if (trackData.Climbs != null)
                {
                    foreach (var climb in trackData.Climbs)
                        climbPoints += (int)climb.Points;
                }
            }
            else
            {
                distance += evt.Track.Length;
                ascending += evt.Ascending;
                descending += evt.Descending;

                if (evt.HighestPoint != null)
                    highestAltitude = Math.Max(user.HighestAltitude, evt.HighestPoint.Altitude);

                if (evt.LowestPoint != null)
                    lowestAltitude = Math.Min(user.LowestAltitude, evt.LowestPoint.Altitude);

                if (evt.Climbs != null)
                    nrOfClimbs += evt.Climbs.Length;


                if (evt.Climbs != null)
                {
                    foreach (var climb in evt.Climbs)
                        climbPoints += (int)climb.Points;
                }
            }

            if (evt.Result != null)
                timeSeconds += evt.Result.TotalTimeSeconds;

            _db.Update<UserView>(new { NrOfProfiles = nrOfProfiles, Distance = distance, Ascending = ascending, Descending = descending, HighestAltitude = highestAltitude, LowestAltitude = lowestAltitude, NrOfClimbs = nrOfClimbs, ClimbPoints = climbPoints, TimeSeconds = timeSeconds }, p => p.Id == evt.UserId);
        }
    }

    [Alias("Users")]
    public class UserView
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string LastName { get; set; }
        public bool AcceptsEmails { get; set; }
        public int ExperiencePoints { get; set; }
        public int Level { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<Guid> FollowingUserIds { get; set; }
        public List<Guid> FollowedByUserIds { get; set; }
        [Default(typeof(double), "0")]
        public int NrOfProfiles { get; set; }
        [Default(typeof(double), "0")]
        public double Distance { get; set; }
        [Default(typeof(double), "0")]
        public double Ascending { get; set; }
        [Default(typeof(double), "0")]
        public double Descending { get; set; }
        [Default(typeof(double), "0")]
        public double HighestAltitude { get; set; }
        [Default(typeof(double), "0")]
        public double LowestAltitude { get; set; }
        [Default(typeof(int), "0")]
        public int NrOfClimbs { get; set; }
        [Default(typeof(int), "0")]
        public int ClimbPoints { get; set; }
        [Default(typeof(int), "0")]
        public int TimeSeconds { get; set; }
    }
}
