namespace _Project.Scripts.Infrastructure.SM
{
    public interface IParameterEnterableState<TParameter>
    {
        void Enter(in TParameter parameter);
    }
}