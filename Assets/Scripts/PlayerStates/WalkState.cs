using UnityEngine;

namespace PlayerStates
{
    public class WalkState : BaseState
    {
        private Vector2 smoothInput;
        private Vector2 smoothDampVelocity;
        
        public WalkState(FPController context) : base(context)
        { }

        public override void EnterState()
        {
            smoothDampVelocity = Vector2.zero;
        }

        protected override void UpdateState()
        {
            context.smoothInput = Vector2.SmoothDamp(context.smoothInput, context.FpInputs.MoveInput, ref smoothDampVelocity,
                context.accelerationTime);
            context.currentVelocity = Vector3.SmoothDamp(context.currentVelocity,
                context.transform.localRotation *
                new Vector3(context.FpInputs.MoveInput.x, context.currentVelocity.y, context.FpInputs.MoveInput.y) * 
                context.speed, ref context.smoothDampVelocity, context.accelerationTime);
            context.CharacterControllerComponent.Move(context.currentVelocity * Time.deltaTime);
        }

        protected override void ExitState()
        { }
    }
}
