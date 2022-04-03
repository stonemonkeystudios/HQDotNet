﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using HQ.Contracts;

namespace HQ {
    public class BehaviorListenerCollection<TBehaviorListener> : DispatchListenerCollection<TBehaviorListener>, IBehaviorListener where TBehaviorListener : IBehaviorListener{

        void IBehaviorListener.PhaseUpdated(HQPhase phase) {
            this.ForEach((listener) => listener.PhaseUpdated(phase);
        }
    }
}
