using System;

namespace _Project.Scripts.Infrastructure.SM
{
    public abstract class BaseTransition<TState> : ITransition
        where TState : BaseState
    {
        public Type ToStateType { get; }
        public Func<bool> Condition { get; private set; }

        protected BaseTransition(Func<bool> condition)
        {
            ToStateType = typeof(TState);
            Condition = condition;
        }

        public void ChangeCondition(Func<bool> newCondition)
        {
            Condition = newCondition;
        }

        public abstract bool Check(StateMachine stateMachine);
    }
}