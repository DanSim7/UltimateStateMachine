using System;

namespace UltimateStateMachine.Code.Core
{
    public interface ITransition
    {
        bool TryTransit(IChangeStateStateMachine stateMachine);
    }

    public class Transition<TState> : ITransition 
        where TState : class, IEnterState
    {
        private readonly Func<bool> _condition;

        public Transition(Func<bool> condition)
        {
            _condition = condition;
        }

        public bool TryTransit(IChangeStateStateMachine stateMachine)
        {
            if (_condition?.Invoke() != true)
                return false;
            
            stateMachine.ChangeState<TState>();
            return true;
        }
    }

    public class TransitionWithPayload<TState, TPayload> : ITransition 
        where TState : class, IEnterStateWithPayload<TPayload>
        where TPayload : class
    {
        private readonly Func<bool> _condition;
        private readonly Func<TPayload> _payload;

        public TransitionWithPayload(Func<bool> condition, Func<TPayload> payload)
        {
            _condition = condition;
            _payload = payload;
        }

        public bool TryTransit(IChangeStateStateMachine stateMachine)
        {
            if (_condition?.Invoke() != true)
                return false;
            
            stateMachine.ChangeState<TState, TPayload>(_payload?.Invoke());
            return true;
        }
    }
}