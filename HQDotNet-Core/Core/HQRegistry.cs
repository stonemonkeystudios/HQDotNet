﻿using System;
using System.Collections.Generic;
using System.Linq;
using HQDotNet.Model;

/*
 * Maps to build
 * 
 * 
 */

namespace HQDotNet {
    public sealed class HQRegistry : HQController{

        public Dictionary<Type, HQController> Controllers { get; private set; }
        public Dictionary<Type, HQService> Services { get; private set; }
        public Dictionary<Type, List<HQView>> Views { get; private set; }

        private Dictionary<Type, DispatchListenerCollection<IDispatchListener>> _dispatcherBinding;

        public HQRegistry() : base() {

            Controllers = new Dictionary<Type, HQController>();
            Services = new Dictionary<Type, HQService>();
            Views = new Dictionary<Type, List<HQView>>();
            _dispatcherBinding = new Dictionary<Type, DispatchListenerCollection<IDispatchListener>>();
        }

        /// <summary>
        /// Register a singleton behavior for a type of controller 
        /// </summary>
        /// <param name="controller"></param>
        public bool RegisterController<TBehavior>(TBehavior controller)
            where TBehavior : HQController, new(){

            if (Controllers.ContainsKey(controller.GetType()))
                return false;

            Controllers.Add(controller.GetType(), controller);
            return true;
        }

        /// <summary>
        /// Register a singleton behavior for a type of service 
        /// </summary>
        /// <param name="controller"></param>
        public bool RegisterService<TBehavior>(TBehavior service)
            where TBehavior : HQService, new(){

            if (Services.ContainsKey(service.GetType()))
                return false;
            //if (_behaviorModelBinding.ContainsKey(typeof(TModel)))
            //    return false;
            Services.Add(service.GetType(), service);
            return true;
        }

        public bool RegisterView<TBehavior>(TBehavior view)
            where TBehavior : HQView, new(){

            if (!Views.ContainsKey(view.GetType()))
                Views.Add(view.GetType(), new List<HQView>());

            if (Views[view.GetType()].Contains(view))
                return false;

            Views[view.GetType()].Add(view);
            return true;
        }

        public void BindListener <TListenerBehavior>(TListenerBehavior behavior) where TListenerBehavior : IDispatchListener {
            Type listenerType = typeof(TListenerBehavior);

            if (!_dispatcherBinding.ContainsKey(listenerType)) {
                _dispatcherBinding.Add(listenerType, new DispatchListenerCollection<IDispatchListener>());
            }

            _dispatcherBinding[listenerType].Add(behavior);
        }

        public static BehaviorCategory GetBehaviorCategory(Type behaviorType) {
            if ((typeof(HQSession)).IsAssignableFrom(behaviorType))
                return BehaviorCategory.HQ;

            if ((typeof(HQController).IsAssignableFrom(behaviorType)))
                return BehaviorCategory.Controller;

            if ((typeof(HQService).IsAssignableFrom(behaviorType)))
                return BehaviorCategory.Service;

            if (typeof(HQView).IsAssignableFrom(behaviorType)) {
                return BehaviorCategory.View;
            }

            return BehaviorCategory.Invalid;
        }

        #region Dispatcher Bindings

        public IDispatchListener GetDispatchListenerForType<TDispatchListener>()
            where TDispatchListener : IDispatchListener{

            Type listenerType = typeof(TDispatchListener);

            if (_dispatcherBinding.ContainsKey(listenerType)) {
                return _dispatcherBinding[listenerType];
            }
            return new DispatchListenerCollection<TDispatchListener>();
        }

        #endregion;

        #region Private Binding Classes
        private sealed class HQDispatcherBinding<TDispatcherListener> : Dictionary<Type, TDispatcherListener> where TDispatcherListener : IDispatchListener { }

        #endregion

        #region HQBehavior Overrides

        public override bool Startup() {
            //Remap any behaviors that were bound during HQSession initialization
            //Remap();
            return base.Startup();
        }


        #endregion
    }
}
