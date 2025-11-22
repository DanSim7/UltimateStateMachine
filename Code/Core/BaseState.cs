using System;
using System.Collections.Generic;

namespace _Project.Scripts.Infrastructure.SM
{
    public abstract class BaseState
    {
        private readonly StateMachine _stateMachine;
        protected readonly List<ITransition> _transitions = new();

        protected BaseState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public bool CheckTransitions()
        {
            for (var i = 0; i < _transitions.Count; i++)
            {
                var hasTransition = _transitions[i].Check(_stateMachine);
                if (hasTransition)
                    return true;
            }

            return false;
        }

        public void ChangeTransitionCondition(Type toStateType, Func<bool> newCondition)
        {
            for (var i = 0; i < _transitions.Count; i++)
            {
                var transition = _transitions[i];
                if (transition.ToStateType != toStateType)
                    continue;
                
                transition.ChangeCondition(newCondition);
            }
        }
    }
}