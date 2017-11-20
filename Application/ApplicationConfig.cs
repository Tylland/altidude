using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using Altidude.Contracts;
using Altidude.Domain;
using Altidude.Domain.EventHandlers;
using Altidude.Infrastructure;
using Altidude.Infrastructure.SqlServerOrmLite;
using Altidude.Views;
using Microsoft.CSharp.RuntimeBinder;

namespace Altidude.Application
{

    public class ApplicationEventBus : IEventBus
    {
        private List<Delegate> _actions;

        public static IEventHandlerContainer Handlers { get; set; }

        //Registers a callback for the given domain event
        public void Register<T>(Action<T> callback) where T : IEvent
        {
            if (_actions == null)
                _actions = new List<Delegate>();
            _actions.Add(callback);
        }

        //Clears callbacks passed to Register on the current thread   
        public void ClearCallbacks()
        {
            _actions = null;
        }

        //Raises the given domain event   
        public void Raise<T>(T args) where T : IEvent
        {
            if (Handlers != null)
            {
                foreach (var handler in Handlers.ResolveAll<IHandleEvent<T>>())
                {
                    try
                    {
                        handler.Handle(args);
                    }
                    catch (Exception ex)
                    {
                        //TODO: Logging
                        Console.WriteLine(ex);
                    }
                }
            }

            if (_actions != null)
                foreach (var action in _actions)
                    if (action is Action<T>)
                        ((Action<T>) action)(args);
        }



        public ApplicationEventBus(IEventHandlerContainer eventHandlerContainer)
        {
            Handlers = eventHandlerContainer;
        }


        public void Raise(IEvent evt)
        {
            //Raise((dynamic) evt);

            if (Handlers != null)
            {
                foreach (var handler in Handlers)
                {
                    try
                    {
                        ((dynamic)handler).Handle((dynamic)evt);
                    }
                    catch (RuntimeBinderException ex)
                    {
                    }
                    catch (Exception ex)
                    {
                        //Log.Error(ex, "Replay event '{@evt}' in '{view}' failed", evt, view);
                        throw;
                    }
                }
            }
        }
    }
    
    public class EventHandlerContainer : IEventHandlerContainer
        {
            //private class HandlerEntry
            //{
            //    public object Handler { get; private set; }
            //    public bool Synchronized { get; private set; }

            //    public void Handle<T>(T args) where T : class
            //    {
            //        var handler = Handler as T;

            //        if (handler != null)
            //        {
            //            hand
            //        }
            //    }

            //    public HandlerEntry(object handler)
            //        : this(handler, true)
            //    {
            //    }
            //    public HandlerEntry(object handler, bool synchronized)
            //    {
            //        Handler = handler;
            //        Synchronized = synchronized;
            //    }
            //}

            private readonly List<object> _handlers = new List<object>();

            public T[] ResolveAll<T>() where T : class
            {
                var resolved = new List<T>();

                foreach (var obj in _handlers)
                {
                    var t = obj as T;

                    if (t != null)
                        resolved.Add(t);
                }

                return resolved.ToArray();
            }

            public IEventHandlerContainer Add(object obj)
            {
                _handlers.Add(obj);

                return this;
            }

            public IEventHandlerContainer Remove(object obj)
            {
                _handlers.Remove(obj);

                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerator<object> GetEnumerator()
            {
                return _handlers.GetEnumerator();
            }
        }
    
}