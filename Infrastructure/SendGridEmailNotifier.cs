using Altidude.Contracts;
using Altidude.Contracts.Events;
using Altidude.Views;

namespace Altidude.Infrastructure
{
    public class SendGridEmailNotifier : IHandleEvent<KudosGiven>
    {
        private readonly IUserView _users;

        public SendGridEmailNotifier(IUserView users)
        {
            _users = users;
        }

        public void Handle(KudosGiven evt)
        {
            var toUser = _users.GetById(evt.OwnerUserId);

            if (toUser != null && toUser.AcceptsEmails)
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

                    new SendGridMailSender().SendMessage("altidude.net@gmail.com", "Altidude", toUser.Email, null,
                        subject, plainContent, htmlContent);
                }
            }
        }
    }
}
