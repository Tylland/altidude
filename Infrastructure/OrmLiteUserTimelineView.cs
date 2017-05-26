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
    //public class OrmLiteUserTimelineView : IUserTimelineView, IUserService, IHandleEvent<UserCreated>, IHandleEvent<UserGainedExperience>, IHandleEvent<UserGainedLevel>, IHandleEvent<UserFollowed>, IHandleEvent<UserUnfollowed>
    //{
    //    private IDbConnection _db;

    //    public OrmLiteUserTimelineView(IDbConnection db)
    //    {
    //        _db = db;
    //    }

    //}

}
