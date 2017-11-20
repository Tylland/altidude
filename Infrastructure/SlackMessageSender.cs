using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altidude.Contracts;
using Altidude.Contracts.Events;
using Altidude.Views;
using Serilog;

namespace Altidude.Infrastructure
{
    public class SlackMessageSender : IHandleEvent<UserCreated>, IHandleEvent<ProfileCreated>, IHandleEvent<ProfileDeleted>, IHandleEvent<KudosGiven>
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<SlackMessageSender>();

        private readonly SlackClient _slackClient;
        private IViews _views;
        public SlackMessageSender(IViews views, Uri webHookUri)
        {
            _views = views;
            _slackClient = new SlackClient(webHookUri);
        }
        private void SendMessage(string text)
        {
            var message = new SlackClient.SlackMessage()
            {
                Channel = "#general",
                UserName = "Tylland",
                Text = text
            };

            Log.Debug("Sending {SlackMessage} to SlackClient", message.Text);

            _slackClient.SendSlackMessage(message);
        }
        public void Handle(UserCreated evt)
        {
            SendMessage(evt.FirstName + " " + evt.LastName + " registred as new user");
        }

        private string FormatNull(string str)
        {
            return  str ?? "<null>";
        }
        public void Handle(KudosGiven evt)
        {
            var profile = _views.Profiles.GetById(evt.Id);
            var user = _views.Users.GetById(evt.UserId);
            var ownerUser = _views.Users.GetById(evt.OwnerUserId);

            SendMessage(FormatNull(user?.DisplayName) + " gave altitude to " + FormatNull(ownerUser?.DisplayName) + " and " + FormatNull(profile?.Name));
        }

        public void Handle(ProfileCreated evt)
        {
            var user = _views.Users.GetById(evt.UserId);

            SendMessage(FormatNull(user?.DisplayName) + " created profile " + evt.Name);
        }

        public void Handle(ProfileDeleted evt)
        {
            var user = _views.Users.GetById(evt.UserId);

            SendMessage(FormatNull(user?.DisplayName) + " deleted profile");
        }
    }
}
