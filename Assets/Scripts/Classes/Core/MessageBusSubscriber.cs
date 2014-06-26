using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes.Core
{
    public class MessageBusSubscriber
    {
        private MessageBus messageBus;

        public void SetMessageBus(MessageBus messageBus)
        {
            this.messageBus = messageBus;
        }

        public virtual void ReceiveMessage(object message)
        {

        }

        public void SendMessage(object message)
        {
            messageBus.SendMessage(message);
        }
    }
}
