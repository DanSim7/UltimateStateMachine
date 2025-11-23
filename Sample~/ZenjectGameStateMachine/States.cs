using System;
using DanSim.UltimateStateMachine.Core;

namespace DanSim.UltimateStateMachine.Sample.ZenjectGameStateMachine
{
    public interface IExampleBootService
    {
        void Load(Action onLoaded);
    }

    public class BootstrapState : BaseState<BootstrapState>, IEnterState
    {
        private readonly IExampleBootService _bootService;
        private bool _isLoadComplete;

        private const string MENU_SCENE_NAME = "Example Menu Scene";

        public BootstrapState(IExampleBootService bootService)
        {
            _bootService = bootService;
            
            AddTransition(
                new TransitionWithPayload<LoadSceneState, string>(
                    condition:() => _isLoadComplete == true,
                    payload: () => MENU_SCENE_NAME
                    )
                );
        }

        public void OnEnter()
        {
            _isLoadComplete = false;
            _bootService.Load(() => _isLoadComplete = true);
        }
    }
    
    public class LoadSceneState : BaseState<LoadSceneState>, IEnterStateWithPayload<string>
    {
        public void OnEnter(string payload)
        {
            // load scene with name = payload
        }
    }

    public class MenuState : BaseState<MenuState>, IEnterState, IPreUpdateState, IUpdateState, IExitState
    {
        public MenuState()
        {
            // get dependencies
        }
        
        public void OnEnter()
        {
            // initialize something
        }

        public void OnPreUpdate()
        {
            // update something before check transitions
        }

        public void OnUpdate()
        {
            // update something after check transitions
        }

        public void OnExit()
        {
            // cleanup something
        }
    }
}