using DevIO.Business.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace DevIO.Business.Notifications
{
    public class Notifier : INotifier
    {
        private List<Notification> notifications;

        public Notifier()
        {
            notifications = new List<Notification>();
        }

        public List<Notification> GetNotifications()
        {
            return notifications;
        }

        public void Handle(Notification notification)
        {
            notifications.Add(notification);
        }

        public bool HasNotification()
        {
            return notifications.Any();
        }
    }
}
