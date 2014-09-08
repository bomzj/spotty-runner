using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes.Core
{
    public class MessageBus
    {
        private List<MessageBusSubscriber> subscribers = new List<MessageBusSubscriber>();
        
        public void Subscribe(MessageBusSubscriber subscriber)
        {
            subscriber.SetMessageBus(this);
        }

        public void UnSubscribe(MessageBusSubscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        public void SendMessage(object message)
        {
            var temp = subscribers.ToArray(); 
            
            foreach (var subscriber in temp)
            {
                subscriber.ReceiveMessage(message);
            }
        }


    }
}
