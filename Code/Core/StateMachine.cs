using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.SM
{
    public abstract class StateMachine : IInitializable, ITickable
    {
        public event Action<BaseState, BaseState> OnStateChanged;
        public BaseState CurrentState { get; private set; }
        
        private Dictionary<Type, BaseState> _states;
        private BaseState _previousState;

        public virtual void Initialize()
        {
            _states = InitStates();
        }

        protected abstract Dictionary<Type, BaseState> InitStates();

        public void Enter<TState>() where TState : BaseState
        {
            var state = ChangeState<TState>();
            if (state is IEnterableState enterableState)
                enterableState.Enter();
            
            OnStateChanged?.Invoke(state, _previousState);
        }

        public void Enter<TState, TParameter>(TParameter parameter) where TState : BaseState, IParameterEnterableState<TParameter>
        {
            var state = ChangeState<TState>();
            state.Enter(parameter);
            
            OnStateChanged?.Invoke(state, _previousState);
        }
        
        public void Stop()
        {
            if (CurrentState is IExitableState exitableState)
                exitableState.Exit();

            CurrentState = null;
        }

        public void Tick()
        {
            var currentState = CurrentState;
            if (currentState == null)
                return;
            
            var hasTransition = currentState.CheckTransitions();
            
            if (!hasTransition && CurrentState is IUpdateableState updateableState) 
                updateableState.Update();
        }

        private TState ChangeState<TState>() where TState : BaseState
        {
            if (CurrentState is IExitableState exitableState)
                exitableState.Exit();
      
            var state = GetState<TState>();
            _previousState = CurrentState;
            
            CurrentState = state;
      
            return state;
        }

        public TState GetState<TState>() where TState : BaseState => 
            _states[typeof(TState)] as TState;
    }
}