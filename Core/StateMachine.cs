using System;
using System.Collections.Generic;

namespace DanSim.UltimateStateMachine.Core
{
    public interface IChangeStateStateMachine
    {
        void ChangeState<TState>()
            where TState : class, IEnterState;
        void ChangeState<TState, TPayload>(TPayload payload)
            where TState : class, IEnterStateWithPayload<TPayload> 
            where TPayload : class;
    }

    public interface IStateMachine : IChangeStateStateMachine
    {
        event Action<IState, IState> OnStateChanged;
        
        IState CurrentState { get; }

        TState GetState<TState>() where TState : class, IState;
        void Update();
        void Stop();
    }

    public abstract class StateMachine : IStateMachine
    {
        public event Action<IState, IState> OnStateChanged;

        private readonly IStateFactory _stateFactory;
        private readonly Dictionary<Type, IState> _states = new();
        private IState _previousState;

        protected StateMachine(IStateFactory stateFactory)
        {
            _stateFactory = stateFactory;
        }

        public IState CurrentState { get; private set; }

        public void ChangeState<TState>()
            where TState : class, IEnterState
        {
            var state = SetCurrentState<TState>();
            state.OnEnter();
            
            OnStateChanged?.Invoke(state, _previousState);
        }

        public void ChangeState<TState, TPayload>(TPayload payload)
            where TState : class, IEnterStateWithPayload<TPayload> 
            where TPayload : class
        {
            var state = SetCurrentState<TState>();
            state.OnEnter(payload);
            
            OnStateChanged?.Invoke(state, _previousState);
        }

        public TState GetState<TState>() where TState : class, IState => 
            _states[typeof(TState)] as TState;

        public void Update()
        {
            var currentState = CurrentState;
            if (currentState == null)
                return;
            
            if (currentState is IPreUpdateState updateState)
                updateState.OnPreUpdate();
            
            var hasTransition = currentState.TryTransit(this);
            
            if (!hasTransition && CurrentState is IUpdateState postUpdateState) 
                postUpdateState.OnUpdate();
        }

        public void Stop()
        {
            if (CurrentState is IExitState exitState)
                exitState.OnExit();

            CurrentState = null;
        }

        protected void AddState<TState>()
            where TState : class, IState
        {
            _states.Add(typeof(TState), _stateFactory.CreateState<TState>());
        }

        private TState SetCurrentState<TState>() 
            where TState : class, IState
        {
            if (CurrentState is IExitState exitState)
                exitState.OnExit();
      
            var state = GetState<TState>();
            _previousState = CurrentState;
            
            CurrentState = state;
      
            return state;
        }
    }
}