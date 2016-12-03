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

namespace Altidude.Infrastructure
{
    public class OrmLiteUserView : IUserView, IUserService, IHandleEvent<UserCreated>, IHandleEvent<UserSettingsUpdated>, IHandleEvent<UserGainedExperience>, IHandleEvent<UserGainedLevel>
    {
        private IDbConnection _db;

        public OrmLiteUserView(IDbConnection db)
        {
            _db = db;
        }

        private User CreateUser(UserView view)
        {
            return new User(view.Id, view.UserName, view.Email, view.FirstName, view.LastName, view.AcceptsEmails, view.ExperiencePoints, view.Level);
        }

        public User GetById(Guid id)
        {
            var view = _db.GetById<UserView>(id);
            Debug.WriteLine(_db.GetLastSql());

            return CreateUser(view);
        }
        public List<User> GetAll()
        {
            var views = _db.Select<UserView>().OrderBy(row => row.CreatedTime);
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
    }

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
    }
}
