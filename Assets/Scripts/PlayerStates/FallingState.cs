using UnityEngine;

namespace PlayerStates
{
    public class FallingState : BaseState
    {
        private Quaternion _velocityLocalRotation;
        public FallingState(FPController context) : base(context)
        { }

        public override void EnterState()
        {
            _velocityLocalRotation = context.transform.localRotation;
        }

        protected override void UpdateState()
        {
            ApplyAirDrag();
            Vector3 rawVelocity = context.GetRawVelocity(_velocityLocalRotation);
            float angleBetweenCameraAndVelocity = Vector3.SignedAngle(
                new Vector3(context.currentVelocity.x, 0, context.currentVelocity.z), context.transform.forward,
                Vector3.up);
            if (context.FpInputs.MoveInput.x < 0)
            {
                if (angleBetweenCameraAndVelocity < 0)
                {
                    _velocityLocalRotation = Quaternion.RotateTowards(_velocityLocalRotation,
                        context.transform.localRotation,
                        context.rotationSpeed * Time.deltaTime *
                        Mathf.Abs(context.FpInputs.MoveInput.x / (1 + Mathf.Abs(context.FpInputs.MoveInput.y))));
                }
            }
            else if (context.FpInputs.MoveInput.x > 0)
            {
                if (angleBetweenCameraAndVelocity > 0)
                {
                    _velocityLocalRotation = Quaternion.RotateTowards(_velocityLocalRotation,
                        context.transform.localRotation,
                        context.rotationSpeed * Time.deltaTime *
                        Mathf.Abs(context.FpInputs.MoveInput.x / (1 + Mathf.Abs(context.FpInputs.MoveInput.y))));
                }
            }
            context.currentVelocity = _velocityLocalRotation * rawVelocity;

            Vector3 directionalInput = context.transform.localRotation * context.FpInputs.MoveInput.ToVector3X0Y();
            float dotProduct = Vector3.Dot(directionalInput.normalized,
                new Vector3(context.currentVelocity.x, 0, context.currentVelocity.z).normalized);
            if (dotProduct < -0.7f)
            {
                context.currentVelocity = Vector3.MoveTowards(context.currentVelocity,
                    new Vector3(0, context.currentVelocity.y, 0), 10f * -1 * dotProduct * Time.deltaTime);
            }
            
            context.CharacterControllerComponent.Move(context.currentVelocity * Time.deltaTime);
        }

        protected override void ExitState()
        { }

        private void ApplyAirDrag()
        {
            context.currentVelocity = Vector3.MoveTowards(context.currentVelocity,
                new Vector3(0, context.currentVelocity.y, 0), context.airDrag * Time.deltaTime);
        }
    }
}
