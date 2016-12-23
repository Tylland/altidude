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
using System.Data.SqlClient;

namespace Altidude.Infrastructure
{
    public class OrmLiteProfileView : IProfileView, IHandleEvent<ProfileCreated>, IHandleEvent<ChartChanged>, IHandleEvent<ProfileViewRegistred>, IHandleEvent<ProfileDeleted>
    {
        private IDbConnection _db;
        private JsonSerializerSettings _serializerSettings;

        public OrmLiteProfileView(IDbConnection db)
        {
            _db = db;

            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        private T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _serializerSettings);
        }

        public string GetAsJson(Guid id)
        {
            throw new NotImplementedException();
        }

        public Profile GetById(Guid id)
        {
            var envelope = _db.GetById<ProfileEnvelope>(id);
            Debug.WriteLine(_db.GetLastSql());


            if (envelope != null && envelope.Payload != null)
                return Deserialize<Profile>(envelope.Payload);

            return null;
        }
        public List<Profile> GetByUser(Guid userId)
        {
            //SqlCommand command = new SqlCommand()

            var envelopes = _db.Select<ProfileEnvelope>(envelope => envelope.UserId == userId);
            Debug.WriteLine(_db.GetLastSql());

            return envelopes.Select(envelope => Deserialize<Profile>(envelope.Payload)).ToList();
        }
        public List<Profile> GetAll()
        {
            var envelopes = _db.Select<ProfileEnvelope>().OrderBy(row => row.CreatedTime);

            Debug.WriteLine(_db.GetLastSql());

            return envelopes.Select(envelope => Deserialize<Profile>(envelope.Payload)).ToList();
        }

        public void Handle(ProfileCreated evt)
        {
            var profile = new Profile(evt.Id, evt.UserId, evt.Name, evt.Track, evt.Places, evt.Legs, evt.Result);

            var envelope = new ProfileEnvelope();
            envelope.Id = evt.Id;
            envelope.UserId = evt.UserId;
            envelope.Name = evt.Name;
            envelope.CreatedTime = evt.CreatedTime;
            envelope.Payload = JsonConvert.SerializeObject(profile, _serializerSettings);
            envelope.NrOfViews = 0;

            _db.Insert(envelope);
        }

        public void Handle(ChartChanged evt)
        {
            var envelope = _db.GetById<ProfileEnvelope>(evt.Id);
            Debug.WriteLine(_db.GetLastSql());

            if (envelope != null && envelope.Payload != null)
            { 
                var profile = Deserialize<Profile>(envelope.Payload);

                profile.ChartId = evt.ChartId;

                envelope.Payload = JsonConvert.SerializeObject(profile, _serializerSettings);

                _db.Update(envelope);
            }
        }

        public void Handle(ProfileViewRegistred evt)
        {
            _db.Update<ProfileEnvelope>(new { NrOfViews = evt.NrOfViews }, p => p.Id == evt.Id);
        }

        public void Handle(ProfileDeleted evt)
        {
            _db.Delete<ProfileEnvelope>(p => p.Id == evt.Id);
        }

        public List<Profile> GetLatest(int nrOfProfiles)
        {
            var envelopes = _db.Select<ProfileEnvelope>().OrderByDescending(row => row.CreatedTime).Take(nrOfProfiles);

            Debug.WriteLine(_db.GetLastSql());

            return envelopes.Select(envelope => Deserialize<Profile>(envelope.Payload)).ToList();
        }

        public List<ProfileSummary> GetSummaries()
        {
            var envelopes = _db.Select<ProfileEnvelope>().OrderBy(row => row.CreatedTime);

            Debug.WriteLine(_db.GetLastSql());

            return envelopes.Select(env => new ProfileSummary(env.Id, env.UserId, env.Name, env.NrOfViews, env.CreatedTime)).ToList();

        }
    }

    public class PayloadHelper
    {
        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }

    public class ProfileEnvelope
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public int NrOfViews { get; set; }
        public DateTime CreatedTime { get; set; }

        [StringLength(int.MaxValue)]
        public string Payload { get; set; }

        //[Ignore]
        //public string JsonPayload
        //{
        //    get
        //    {
        //        return PayloadHelper.GetString(Payload);
        //    }
        //    set
        //    {
        //        Payload = PayloadHelper.GetBytes(value);
        //    }
        //}

    }
}
