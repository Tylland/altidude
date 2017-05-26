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
using Altidude.Logging;
using Serilog;
using ServiceStack.ServiceHost;

namespace Altidude.Infrastructure
{
    public class OrmLiteProfileView : IProfileView, IHandleEvent<ProfileCreated>, IHandleEvent<ChartChanged>, IHandleEvent<ProfileViewRegistred>, IHandleEvent<KudosGiven>, IHandleEvent<ProfileDeleted>
    {
        private static string ProfileTableName = "Profiles";
        private static readonly ILogger Log = Serilog.Log.ForContext<MigrationManager>();

        private readonly IDbConnection _db;
        private readonly JsonSerializerSettings _serializerSettings;

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

        private Profile CreateProfile(ProfileEnvelope env)
        {
            if (env != null)
                return new Profile(env.Id, env.UserId, env.ChartId, env.Name, env.Track, env.HighestPoint, env.LowestPoint,
                    env.Ascending, env.Descending, env.Climbs, env.Places, env.Legs, env.Result);

            return null;
        }

        private ProfileSummary CreateSummary(ProfileSummaryRow row)
        {
            return new ProfileSummary(row.Id, row.UserId, row.ChartId, row.Name, row.Distance, row.Ascending, row.Descending, row.CreatedTime, row.TimeSeconds, row.NrOfViews, row.Kudos, row.CreatedTime);
        }

        public Profile GetById(Guid id)
        {
            var envelope = _db.GetByIdOrDefault<ProfileEnvelope>(id);
            Debug.WriteLine(_db.GetLastSql());

            return CreateProfile(envelope);
        }
        public List<Profile> GetByUser(Guid userId)
        {
            //SqlCommand command = new SqlCommand()

            var envelopes = _db.Select<ProfileEnvelope>(envelope => envelope.UserId == userId);
            Debug.WriteLine(_db.GetLastSql());

            return envelopes.Select(CreateProfile).ToList();
        }
        public void Handle(ProfileCreated evt)
        {
            Log.Verbose("Start handling event '{@evt}' in '{view}'", evt, this);

            var envelope = new ProfileEnvelope
            {
                Id = evt.Id,
                UserId = evt.UserId,
                Name = evt.Name,
                ChartId = evt.ChartId,
                Distance = evt.Track.Length,
                Ascending = evt.Ascending,
                Descending = evt.Descending,
                StartTime = evt.Result?.StartTime ?? evt.CreatedTime,
                TimeSeconds = evt.Result?.TotalTimeSeconds ?? 0,
                NrOfViews = 0,
                Kudos = 0,
                Track = evt.Track,
                HighestPoint = evt.HighestPoint,
                LowestPoint = evt.LowestPoint,
                Climbs = evt.Climbs,
                Places = evt.Places,
                Legs = evt.Legs,
                Result = evt.Result,
                CreatedTime = evt.CreatedTime
            };

             try
            {
                Log.Debug("Insert profile {@profile} started", envelope);
                _db.Insert(envelope);
            }
            catch (Exception e)
            {
                Log.Error(e, "Insert profile {@profile} failed {sql}", envelope, _db.GetLastSql());
            }

        }

        public void Handle(ChartChanged evt)
        {
            _db.Update<ProfileEnvelope>(new { ChartId = evt.ChartId }, p => p.Id == evt.Id);
        }

        public void Handle(ProfileViewRegistred evt)
        {
            _db.Update<ProfileEnvelope>(new { NrOfViews = evt.NrOfViews }, p => p.Id == evt.Id);
        }

        public void Handle(KudosGiven evt)
        {
            var envelope = _db.GetById<ProfileEnvelope>(evt.Id);
            Debug.WriteLine(_db.GetLastSql());

            if (envelope != null)
            {
                envelope.Kudos = evt.TotalKudos;

                _db.Update(envelope);
            }
        }

        public void Handle(ProfileDeleted evt)
        {
            var affected = _db.Delete<ProfileEnvelope>(p => p.Id == evt.Id);
            Debug.WriteLine(_db.GetLastSql());
        }

        public List<Profile> GetAll()
        {
            var envelopes = _db.Select<ProfileEnvelope>().OrderBy(row => row.CreatedTime);

            Debug.WriteLine(_db.GetLastSql());

            return envelopes.Select(CreateProfile).ToList();
        }


        public List<Profile> GetLatest(int nrOfProfiles)
        {
            IEnumerable<ProfileEnvelope> envelopes;

            using (Log.StartTiming("GetLatest - Select from db").WithWarning(5000))
            {
                envelopes = _db.Select<ProfileEnvelope>().OrderByDescending(row => row.CreatedTime).Take(nrOfProfiles);
                    //envelopes = _db.Select<ProfileEnvelope>().Take(nrOfProfiles);

                var sql = _db.GetLastSql();

                Debug.WriteLine(sql);
                Log.Debug(sql);
            }

            using (Log.StartTiming("GetLatest - Deserialize").WithWarning(1000))
            {
                return envelopes.Select(CreateProfile).ToList();
            }
        }

        //private const string SelectProfileSummary =
        //    "SELECT Id, UserId, Name, NrOfViews, Kudos, CreatedTime FROM " + ProfileTableName;

        public List<ProfileSummary> GetSummaries()
        {
            var envelopes = _db.Select<ProfileSummaryRow>().OrderByDescending(row => row.CreatedTime);

            Debug.WriteLine(_db.GetLastSql());

            return envelopes.Select(CreateSummary).ToList();
        }

        public List<ProfileSummary> GetLatestSummaries(int nrOfProfiles)
        {
            using (Log.StartTiming("GetLatestSummaries").WithWarning(5000))
            {
                var envelopes = _db.Select<ProfileSummaryRow>().OrderByDescending(row => row.CreatedTime).Take(nrOfProfiles).ToList();

                Debug.WriteLine(_db.GetLastSql());

                return envelopes.Select(CreateSummary).ToList();
            }
        }

        public List<ProfileSummary> GetLatestSummaries(Guid[] userIds, int start, int nrOfProfiles)
        {
            using (Log.StartTiming("GetLatestSummaries").WithWarning(5000))
            {
                var envelopes = _db.Select<ProfileSummaryRow>().Where(row => Sql.In(row.UserId, userIds)).OrderByDescending(row => row.CreatedTime).Skip(start).Take(nrOfProfiles).ToList();

                Debug.WriteLine(_db.GetLastSql());

                return envelopes.Select(CreateSummary).ToList();
            }
        }

        public int GetTotalNrOfViews()
        {
            return _db.Select<ProfileSummaryRow>().Sum(env => env.NrOfViews);
        }

        public long Count()
        {
            var count = _db.Select<ProfileSummaryRow>().Count;
            var sql = _db.GetLastSql();

            return count;
        }

        public ProfileSummary GetSummaryById(Guid id)
        {
            var envelope = _db.GetById<ProfileSummaryRow>(id);
            Debug.WriteLine(_db.GetLastSql());

            return CreateSummary(envelope);

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

    [Alias("Profiles")]
    public class ProfileSummaryRow
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(100)]
        public Guid ChartId { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public double Ascending { get; set; }
        public double Descending { get; set; }
        public int TimeSeconds { get; set; }
        public int NrOfViews { get; set; }
        public int Kudos { get; set; }
        public DateTime CreatedTime { get; set; }
    }


    [Alias("Profiles")]
    public class ProfileEnvelope
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(100)]
        public Guid ChartId { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public double Ascending { get; set; }
        public double Descending { get; set; }
        public DateTime StartTime { get; set; }
        public int TimeSeconds { get; set; }
        public int NrOfViews { get; set; }
        public int Kudos { get; set; }
        public DateTime CreatedTime { get; set; }
        public Track Track { get; set; }
        public TrackPoint HighestPoint { get; set; }
        public TrackPoint LowestPoint { get; set; }
        public Climb[] Climbs { get; set; }
        public ProfilePlace[] Places { get; set; }
        public Leg[] Legs { get; set; }
        public Result Result { get; set; }
    }


    public class ProfileBackup
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(int.MaxValue)]
        public string Payload { get; set; }

        public int NrOfViews { get; set; }
        public int Kudos { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
