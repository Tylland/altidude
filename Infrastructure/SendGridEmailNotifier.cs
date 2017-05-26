using System.Configuration;
using System.Threading.Tasks;
using Altidude.Contracts;
using Altidude.Contracts.Events;
using Altidude.Views;
using Serilog;

namespace Altidude.Infrastructure
{
    public class SendGridEmailNotifier : IHandleEvent<KudosGiven>
    {
        private static ILogger _log = Log.ForContext<SendGridEmailNotifier>();

        private readonly IUserView _users;

        public SendGridEmailNotifier(IUserView users)
        {
            _users = users;
        }

        public void Handle(KudosGiven evt)
        {
            _log.Debug("Handle - KudosGiven {@event}", evt);

            Task.Run(() => SendKudosEmail(evt));
        }

        private void SendKudosEmail(KudosGiven evt)
        {
            _log.Debug("SendKudosEmail - Started {@event}", evt);

            var toUser = _users.GetById(evt.OwnerUserId);

            if (toUser != null)
            {
                if (toUser.AcceptsEmails)
                {
                    var fromUser = _users.GetById(evt.UserId);

                    if (fromUser != null)
                    {
                        var subject = fromUser.DisplayName + " gave you altitude (kudos)!";

                        var chartImageUrl = string.Format("https://altidude.blob.core.windows.net/chartimages/{0}.png",
                            evt.Id);
                        var profileUrl = string.Format("http://www.altidude.net/profile/detail/{0}", evt.Id);

                        var plainContent = "";
                        var htmlContent = "<h3>Good news! " + fromUser.DisplayName +
                                          " likes your activity and your profile gained altitude.</h3><a href=\"" +
                                          profileUrl + "\"><img src=\"" + chartImageUrl + "\" /></a>";

                        _log.Debug("SendKudosEmail from {user} to {@email}", fromUser.DisplayName, toUser.Email);

                        new SendGridMailSender().SendMessage("altidude.net@gmail.com", "Altidude", toUser.Email, null,
                            subject, plainContent, htmlContent);
                    }
                    else
                    {
                        _log.Debug("SendKudosEmail from user {OwnerUserId} not found!", evt.UserId);
                    }
                }
                else
                {
                    _log.Debug("SendKudosEmail - to {@user} do not accepts emails!", toUser);
                }

            }
            else

            {
                _log.Debug("SendKudosEmail to user {OwnerUserId} not found!", evt.OwnerUserId);
            }

        }
    }
}
