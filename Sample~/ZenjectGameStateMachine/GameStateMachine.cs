using System;
using DanSim.UltimateStateMachine.Core;
using DanSim.UltimateStateMachine.ZenjectSupport;
using Zenject;

namespace DanSim.UltimateStateMachine.Sample.ZenjectGameStateMachine
{
    public class GameStateMachine : StateMachine, IInitializable, ITickable, IDisposable
    {
        public GameStateMachine(IStateFactory stateFactory) : base(stateFactory)
        {
            AddState<BootstrapState>();
            AddState<LoadSceneState>();
            AddState<MenuState>();
        }

        public void Initialize()
        {
            ChangeState<BootstrapState>();
        }

        public void Tick()
        {
            Update();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}