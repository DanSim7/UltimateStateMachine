#if ZENJECT
using Zenject;
#endif

namespace DanSim.UltimateStateMachine.Core
{
    public interface IStateFactory
    {
        T CreateState<T>() where T : IState;
    }
    
#if ZENJECT
    public class ZenjectStateFactory : IStateFactory
    {
        private readonly DiContainer _container;

        public ZenjectStateFactory(DiContainer container) =>
            _container = container;

        public T CreateState<T>() where T : IState =>
            _container.Resolve<T>();
    }
#endif
}