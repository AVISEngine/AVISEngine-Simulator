using System;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

[Serializable]
public class CarPhysicsParameters
{
    public float VehicleMass;
    public float VehicleDrag;
    public float VehicleAngularDrag;
    public WheelFrictionParameters FRWheelFriction;
    public WheelFrictionParameters FLWheelFriction;
    public WheelFrictionParameters RRWheelFriction;
    public WheelFrictionParameters RLWheelFriction;

    [Serializable]
    public class WheelFrictionParameters
    {
        public float ExtremumSlip;
        public float ExtremumValue;
        public float AsymptoteSlip;
        public float AsymptoteValue;
        public float Stiffness;
    }

    public void ApplyToCarController(CarController controller)
    {
        Rigidbody carRigidbody = controller.GetComponent<Rigidbody>();

        // Apply vehicle parameters
        carRigidbody.mass = VehicleMass;
        carRigidbody.drag = VehicleDrag;
        carRigidbody.angularDrag = VehicleAngularDrag;

        // Assume the controller has an array or list of WheelColliders
        ApplyWheelFrictionParameters(controller.m_WheelColliders[0], FRWheelFriction); // Front Right
        ApplyWheelFrictionParameters(controller.m_WheelColliders[1], FLWheelFriction); // Front Left
        ApplyWheelFrictionParameters(controller.m_WheelColliders[2], RRWheelFriction); // Rear Right
        ApplyWheelFrictionParameters(controller.m_WheelColliders[3], RLWheelFriction); // Rear Left
    }

    private void ApplyWheelFrictionParameters(WheelCollider wheelCollider, WheelFrictionParameters frictionParameters)
    {
        WheelFrictionCurve frictionCurve = wheelCollider.sidewaysFriction;

        frictionCurve.extremumSlip = frictionParameters.ExtremumSlip;
        frictionCurve.extremumValue = frictionParameters.ExtremumValue;
        frictionCurve.asymptoteSlip = frictionParameters.AsymptoteSlip;
        frictionCurve.asymptoteValue = frictionParameters.AsymptoteValue;
        frictionCurve.stiffness = frictionParameters.Stiffness;

        wheelCollider.sidewaysFriction = frictionCurve;
        // Repeat for forwardFriction if desired/required
        wheelCollider.forwardFriction = frictionCurve;
    }
}