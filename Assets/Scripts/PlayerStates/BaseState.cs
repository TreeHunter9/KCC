using UnityEngine;

namespace PlayerStates
{
    public abstract class BaseState
    {
        protected FPController context;
        
        protected BaseState currentSubState;
        protected BaseState currentParentState;

        public BaseState(FPController context)
        {
            this.context = context;
        }

        public abstract void EnterState();
        protected abstract void UpdateState();
        protected abstract void ExitState();
        protected virtual bool CheckSwitchState() => false;
        protected virtual bool CheckSwitchSubState() => false;

        protected void SwitchState(BaseState newState)
        {
            ExitState();
            context.currentState = newState;
            //Debug.Log($"Enter {newState.GetType().Name}");
            context.currentState.EnterState();
        }

        protected void SwitchSubState(BaseState newSubState)
        {
            currentSubState?.ExitState();
            currentSubState = newSubState;
            currentSubState.EnterState();
        }

        public void UpdateStates()
        {
            if (CheckSwitchState() == false)
                UpdateState();

            CheckSwitchSubState();
            currentSubState?.UpdateState();
        }
    }
}
