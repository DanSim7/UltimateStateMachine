using Zenject;

namespace DanSim.UltimateStateMachine.Sample.ZenjectGameStateMachine
{
    public class GameStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStateFactory();
            BindStates();
            BindStateMachine();
        }

        private void BindStateFactory()
        {
            Container.BindInterfacesTo<ZenjectStateFactory>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadSceneState>().AsSingle();
            Container.Bind<MenuState>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
        }
    }
}