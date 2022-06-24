using UnityEngine;

namespace PlayerStates
{
    public class FlyingState : BaseState
    {
        public BaseState FallingState { get; private set; }

        public FlyingState(FPController context) : base(context)
        {
            FallingState = new FallingState(context);
        }

        public override void EnterState()
        {
            Debug.Log($"Enter {this.GetType().Name}");
            SwitchSubState(FallingState);
        }

        protected override void UpdateState()
        {
            context.currentVelocity.y += context.gravity * Time.deltaTime;
        }

        protected override void ExitState()
        { }

        protected override bool CheckSwitchState()
        {
            if (context.IsGrounded(context.transform, context.CharacterControllerComponent, out var hit) == true)
            {
                float angle = Vector3.Angle(Vector3.up, hit.normal);
                if (angle > context.CharacterControllerComponent.slopeLimit)
                    return false;
                SwitchState(context.GroundedState);
                return true;
            }

            return false;
        }
    }
}
