using Zenject;

namespace _Project.Scripts.Infrastructure.SM
{
    public class StateFactory
    {
        private readonly DiContainer _container;

        public StateFactory(DiContainer container) =>
            _container = container;

        public T CreateState<T>() where T : BaseState =>
            _container.Resolve<T>();
    }
}