using System;
using System.Collections.Generic;

public class Messenger
{
    private static readonly Messenger _instance = new Messenger();
    public static Messenger Instance => _instance;

    private readonly Dictionary<Type, List<Action<object>>> _subscribers = new Dictionary<Type, List<Action<object>>>();

    public void Subscribe<TMessage>(Action<TMessage> action)
    {
        var messageType = typeof(TMessage);
        if (!_subscribers.ContainsKey(messageType))
        {
            _subscribers[messageType] = new List<Action<object>>();
        }
        _subscribers[messageType].Add(msg => action((TMessage)msg));
    }

    public void Publish<TMessage>(TMessage message)
    {
        var messageType = message.GetType();
        if (_subscribers.ContainsKey(messageType))
        {
            foreach (var action in _subscribers[messageType])
            {
                action(message);
            }
        }
    }
}
