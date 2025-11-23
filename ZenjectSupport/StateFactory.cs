#if ZENJECT
using DanSim.UltimateStateMachine.Core;
using Zenject;

namespace DanSim.UltimateStateMachine.ZenjectSupport
{
    public interface IStateFactory
    {
        T CreateState<T>() where T : IState;
    }
    
    public class StateFactory : IStateFactory
    {
        private readonly DiContainer _container;

        public StateFactory(DiContainer container) =>
            _container = container;

        public T CreateState<T>() where T : IState =>
            _container.Resolve<T>();
    }
}
#endif