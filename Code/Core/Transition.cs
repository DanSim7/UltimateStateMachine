using System;

namespace _Project.Scripts.Infrastructure.SM
{
    public class Transition<TState> : BaseTransition<TState>
        where TState : BaseState
    {
        public Transition(Func<bool> condition) : base(condition)
        {
        }

        public override bool Check(StateMachine stateMachine)
        {
            if (!Condition())
                return false;
            
            stateMachine.Enter<TState>();
            
            return true;
        }
    }
}