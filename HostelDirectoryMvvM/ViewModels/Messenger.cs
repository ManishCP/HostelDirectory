using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostelDirectoryMvvM.ViewModels
{
    public static class Messenger
    {
        private static readonly Dictionary<Type, List<Action<object>>> subscribers = new Dictionary<Type, List<Action<object>>>();

        public static void Subscribe<T>(Action<T> action)
        {
            var messageType = typeof(T);
            if (!subscribers.ContainsKey(messageType))
            {
                subscribers[messageType] = new List<Action<object>>();
            }

            subscribers[messageType].Add(o => action((T)o));
        }

        public static void Publish<T>(T message)
        {
            var messageType = message.GetType();
            if (subscribers.ContainsKey(messageType))
            {
                foreach (var action in subscribers[messageType])
                {
                    action(message);
                }
            }
        }
    }
}
