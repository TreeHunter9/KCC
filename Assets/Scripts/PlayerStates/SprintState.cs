using UnityEngine;

namespace PlayerStates
{
    public class SprintState : BaseState
    {
        private Vector2 smoothInput;
        private Vector2 smoothDampVelocity;
        
        public SprintState(FPController context) : base(context)
        { }

        public override void EnterState()
        {
            smoothDampVelocity = Vector2.zero;
        }

        protected override void UpdateState()
        {
            context.smoothInput = Vector2.SmoothDamp(context.smoothInput, context.FpInputs.MoveInput,
                ref smoothDampVelocity,
                context.sprintAccelerationTime);
            context.currentVelocity = Vector3.SmoothDamp(context.currentVelocity,
                context.transform.localRotation *
                new Vector3(context.smoothInput.x, context.currentVelocity.y, context.smoothInput.y) * 
                context.sprintSpeed, ref context.smoothDampVelocity, context.accelerationTime);
            context.CharacterControllerComponent.Move(context.currentVelocity * Time.deltaTime);
        }

        protected override void ExitState()
        { }
    }
}
