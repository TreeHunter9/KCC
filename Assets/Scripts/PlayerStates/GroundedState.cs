using UnityEngine;

namespace PlayerStates
{
    public class GroundedState : BaseState
    {
        public BaseState WalkState { get; private set; }
        public BaseState SprintState { get; private set; }

        private float _currentJumpPenalty = 1f;
        private RaycastHit _hitInfo;
        private bool _isTouchGroundThisFrame;

        public GroundedState(FPController context) : base(context)
        {
            WalkState = new WalkState(context);
            SprintState = new SprintState(context);
        }

        public override void EnterState()
        {
            SwitchSubState(WalkState);
        }

        protected override void UpdateState()
        {
            Vector3 project =
                Vector3.ProjectOnPlane(new Vector3(context.currentVelocity.x, 0, context.currentVelocity.z),
                    _hitInfo.normal);
            float xDiff = 0f;
            float zDiff = 0f;
            if (Vector3.Angle(Vector3.up, _hitInfo.normal) <= context.CharacterControllerComponent.slopeLimit)
            {
                xDiff = context.currentVelocity.x / (project.x == 0 ? 1 : project.x);
                zDiff = context.currentVelocity.z / (project.z == 0 ? 1 : project.z);
            }
            Debug.Log(project.y);
            context.currentVelocity.y = project.y * Mathf.Max(xDiff, zDiff, 1f) - 1f;

            ChangeJumpPenalty();
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
                    SwitchState(context.FlayingState);
            }
            else
            {
                SwitchState(context.FlayingState);
                return true;
            }

            if (context.FpInputs.JumpButtonPressed == true)
            {
                context.currentVelocity *= context.jumpMultiplierAcceleration;
                context.currentVelocity.y = context.jumpHeight * _currentJumpPenalty;
                SwitchState(context.FlayingState);
                _currentJumpPenalty = context.maxJumpPenalty;
                return true;
            }

            return false;
        }

        protected override bool CheckSwitchSubState()
        {
            if (context.isSprinting == true)
            {
                if (currentSubState != SprintState)
                {
                    SwitchSubState(SprintState);
                    return true;
                }
            }
            else
            {
                if (currentSubState != WalkState)
                {
                    SwitchSubState(WalkState);
                    return true;
                }
            }

            return false;
        }

        private void ChangeJumpPenalty()
        {
            if (_currentJumpPenalty < 1)
            {
                _currentJumpPenalty = Mathf.MoveTowards(_currentJumpPenalty, 1f,
                    Time.deltaTime / context.timeToJumpRecovery);
            }
        }
    }
}
