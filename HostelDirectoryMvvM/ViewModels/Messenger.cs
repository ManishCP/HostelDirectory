using System;
using System.Collections.Generic;
using System.Linq;

namespace HostelDirectoryMvvM.ViewModels
{
    public class Messenger
    {
        private static readonly Messenger instance = new Messenger();
        private readonly Dictionary<Type, List<Action<object>>> subscribers = new Dictionary<Type, List<Action<object>>>();

        public static Messenger Instance => instance;

        private Messenger() { }

        public void Subscribe<T>(Action<T> action)
        {
            var messageType = typeof(T);
            if (!subscribers.ContainsKey(messageType))
            {
                subscribers[messageType] = new List<Action<object>>();
            }

            subscribers[messageType].Add(o => action((T)o));
        }

        public void Publish<T>(T message)
        {
            var messageType = message.GetType();
            if (subscribers.ContainsKey(messageType))
            {
                foreach (var action in subscribers[messageType].ToList())
                {
                    try
                    {
                        action(message);
                    }
                    catch (Exception ex)
                    {
                        // Handle or log the exception
                        Console.WriteLine($"Error handling message of type {messageType}: {ex.Message}");
                    }
                }
            }
        }
    }
}
