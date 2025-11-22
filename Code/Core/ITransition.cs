using System;

namespace _Project.Scripts.Infrastructure.SM
{
    public interface ITransition
    {
        Type ToStateType { get; }
        Func<bool> Condition { get; }

        void ChangeCondition(Func<bool> newCondition);

        bool Check(StateMachine stateMachine);
    }
}