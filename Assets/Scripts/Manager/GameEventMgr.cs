using System;
using System.Collections;
using System.Collections.Generic;

public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T, U>(T arg1, U arg2);
public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

public class GameEventMgr : XSingleton<GameEventMgr>
{
    public class FireEventException : Exception
    {
        public FireEventException(string msg) : base(msg)
        {

        }
        public static FireEventException FireEventSignatureException(int eventType)
        {
            return new FireEventException(string.Format("FireEvent message \"{0}\" but listeners have a different signature than the broadcaster.", eventType));
        }
    }
    public class ListenerException : Exception
    {
        public ListenerException(string msg) : base(msg)
        {

        }
    }

    public Dictionary<int, Delegate> mEventTable = new Dictionary<int, Delegate>();
    //public List<

    public void OnListenerAdding(int eventType, Delegate listenerBeingAdded)
    {
        if (!mEventTable.ContainsKey(eventType))
        {
            mEventTable.Add(eventType, null);
        }
        Delegate d = mEventTable[eventType];
        if (d != null && d.GetType() != listenerBeingAdded.GetType())
        {
            throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
        }
    }
    public void OnListenerRemoving(int eventType, Delegate listenerBeingRemoved)
    {
        if (mEventTable.ContainsKey(eventType))
        {
            Delegate d = mEventTable[eventType];
            if (d == null)
            {
                throw new ListenerException(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", eventType));
            }
            else if (d.GetType() != listenerBeingRemoved.GetType())
            {
                throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listener have type {1} and listener being removed has type {2}.", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
        }
        else
        {
            throw new ListenerException(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", eventType));
        }
    }
    public void OnListenerRemoved(int eventType)
    {
        if (mEventTable[eventType] == null)
        {
            mEventTable.Remove(eventType);
        }
    }
    public void OnFireEvent(int eventType)
    {

    }


    public void AddListener(int eventType, Callback handler)
    {
        OnListenerAdding(eventType, handler);
        mEventTable[eventType] = (Callback)mEventTable[eventType] + handler;
    }
    public void AddListener<T>(int eventType, Callback<T> handler)
    {
        OnListenerAdding(eventType, handler);
        mEventTable[eventType] = (Callback<T>)mEventTable[eventType] + handler;
    }
    public void AddListener<T, U>(int eventType, Callback<T, U> handler)
    {
        OnListenerAdding(eventType, handler);
        mEventTable[eventType] = (Callback<T, U>)mEventTable[eventType] + handler;
    }
    public void AddListener<T, U, V>(int eventType, Callback<T, U, V> handler)
    {
        OnListenerAdding(eventType, handler);
        mEventTable[eventType] = (Callback<T, U, V>)mEventTable[eventType] + handler;
    }


    public void RemoveListener(int eventType, Callback handler)
    {
        OnListenerRemoving(eventType, handler);
        mEventTable[eventType] = (Callback)mEventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }
    public void RemoveListener<T>(int eventType, Callback<T> handler)
    {
        OnListenerRemoving(eventType, handler);
        mEventTable[eventType] = (Callback<T>)mEventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }
    public void RemoveListener<T, U>(int eventType, Callback<T, U> handler)
    {
        OnListenerRemoving(eventType, handler);
        mEventTable[eventType] = (Callback<T, U>)mEventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }
    public void RemoveListener<T, U, V>(int eventType, Callback<T, U, V> handler)
    {
        OnListenerRemoving(eventType, handler);
        mEventTable[eventType] = (Callback<T, U, V>)mEventTable[eventType] - handler;
        OnListenerRemoved(eventType);
    }


    public void FireEvent(int eventType)
    {
        OnFireEvent(eventType);
        Delegate d;
        if (mEventTable.TryGetValue(eventType, out d))
        {
            Callback callback = d as Callback;
            if (callback != null)
            {
                callback();
            }
            else
            {
                throw FireEventException.FireEventSignatureException(eventType);
            }
        }
    }
    public void FireEvent<T>(int eventType, T arg1)
    {
        OnFireEvent(eventType);
        Delegate d;
        if (mEventTable.TryGetValue(eventType, out d))
        {
            Callback<T> callback = d as Callback<T>;
            if (callback != null)
            {
                callback(arg1);
            }
            else
            {
                throw FireEventException.FireEventSignatureException(eventType);
            }
        }
    }
    public void FireEvent<T, U>(int eventType, T arg1, U arg2)
    {
        OnFireEvent(eventType);
        Delegate d;
        if (mEventTable.TryGetValue(eventType, out d))
        {
            Callback<T, U> callback = d as Callback<T, U>;
            if (callback != null)
            {
                callback(arg1, arg2);
            }
            else
            {
                throw FireEventException.FireEventSignatureException(eventType);
            }
        }
    }
    public void FireEvent<T, U, V>(int eventType, T arg1, U arg2, V arg3)
    {
        OnFireEvent(eventType);
        Delegate d;
        if (mEventTable.TryGetValue(eventType, out d))
        {
            Callback<T, U, V> callback = d as Callback<T, U, V>;
            if (callback != null)
            {
                callback(arg1, arg2, arg3);
            }
            else
            {
                throw FireEventException.FireEventSignatureException(eventType);
            }
        }
    }

    public bool IsListenerEvent(int eventType)
    {
        return mEventTable.ContainsKey(eventType);
    }
    public void PrintEventTable(){
        
    }
}
