using System;
using System.Collections.Generic;

namespace DanSim.UltimateStateMachine.Core
{
    public abstract class BaseState<TState>: IState
        where TState : class, IState
    {
        private readonly List<ITransition> _transitions = new();
        
        public Type GetStateType() => typeof(TState);
        
        public void AddTransition(ITransition transition) => _transitions.Add(transition);
        public void RemoveTransition(ITransition transition) => _transitions.Remove(transition);

        public bool TryTransit(IChangeStateStateMachine stateMachine)
        {
            for (var i = 0; i < _transitions.Count; i++)
            {
                if (_transitions[i].TryTransit(stateMachine))
                    return true;
            }

            return false;
        }
    }
}