using UnityEngine;

namespace PlayerStates
{
    public class FlyingState : BaseState
    {
        public BaseState FallingState { get; private set; }

        private RaycastHit _hitInfo;
        private bool _isTouchGroundThisFrame;

        public FlyingState(FPController context) : base(context)
        {
            FallingState = new FallingState(context);
        }

        public override void EnterState()
        {
            SwitchSubState(FallingState);
        }

        protected override void UpdateState()
        {
            context.currentVelocity.y += context.gravity * Time.deltaTime;
            if (_isTouchGroundThisFrame == true)
            {
                float angle = Vector3.Angle(Vector3.up, _hitInfo.normal);
                if (angle > context.CharacterControllerComponent.slopeLimit)
                {
                    Vector3 project = Vector3.ProjectOnPlane(context.currentVelocity, _hitInfo.normal);
                    context.currentVelocity = project;
                }
            }
        }

        protected override void ExitState()
        { }

        protected override bool CheckSwitchState()
        {
            _isTouchGroundThisFrame =
                context.IsGrounded(context.transform, context.CharacterControllerComponent, out _hitInfo);
            if (_isTouchGroundThisFrame == true)
            {
                float angle = Vector3.Angle(Vector3.up, _hitInfo.normal);
                if (angle > context.CharacterControllerComponent.slopeLimit)
                    return false;
                SwitchState(context.GroundedState);
                return true;
            }

            return false;
        }
    }
}
