using System;

namespace UltimateStateMachine.Code.Core
{
    public interface IState
    {
        Type GetStateType();
        void AddTransition(ITransition transition);
        void RemoveTransition(ITransition transition);
        bool TryTransit(IChangeStateStateMachine stateMachine);
    }

    public interface IEnterState : IState
    {
        void OnEnter();
    }

    public interface IEnterStateWithPayload<TPayload> : IState
        where TPayload : class
    {
        void OnEnter(TPayload payload);
    }

    public interface IExitState
    {
        void OnExit();
    }

    public interface IUpdateState
    {
        void OnUpdate();
    }

    public interface IPostUpdateState
    {
        void OnPostUpdate();
    }
}