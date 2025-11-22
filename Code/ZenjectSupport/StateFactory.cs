using UltimateStateMachine.Code.Core;
using Zenject;

namespace UltimateStateMachine.Code.ZenjectSupport
{
    public class StateFactory
    {
        private readonly DiContainer _container;

        public StateFactory(DiContainer container) =>
            _container = container;

        public T CreateState<T>() where T : IState =>
            _container.Resolve<T>();
    }
}