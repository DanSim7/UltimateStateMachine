using System;

namespace _Project.Scripts.Infrastructure.SM
{
    public class ParameterTransition<TState, TParameter> : BaseTransition<TState>
        where TState : BaseState, IParameterEnterableState<TParameter>
    {
        private readonly Func<TParameter> _getParameter;
        
        public ParameterTransition(Func<bool> condition, Func<TParameter> getParameter) : base(condition)
        {
            _getParameter = getParameter;
        }

        public override bool Check(StateMachine stateMachine)
        {
            if (!Condition())
                return false;
            
            var parameter = _getParameter == null
                ? default
                : _getParameter.Invoke();
            
            stateMachine.Enter<TState, TParameter>(parameter);
            
            return true;
        }
    }
}