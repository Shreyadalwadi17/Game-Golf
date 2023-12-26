using Ubiq.XR;
using UnityEngine;

public class ClubController : GraspBehaviour
{
    public Transform visual;
    public BallController myBall;
    public Rigidbody club;
    public float batDelta;
    public Vector3 prevAngle;
    public float batPower;

    public void FixedUpdate()
    {
        batDelta = (prevAngle - club.GetComponent<Rigidbody>().position).magnitude;
        prevAngle = club.GetComponent<Rigidbody>().position;
    }
    internal override void Awake()
    {
        base.Awake();
    }

    public override void Grasp(Hand controller)
    {
        base.Grasp(controller);
        Vector3 relativePosition = transform.position - controller.transform.position;
        visual.position += relativePosition;
    }

    public override void Release(Hand controller)
     {
        base.Release(controller);
     }


    internal override void Update()
    {
        base.UpdateOwnership(myBall);
        base.Update();
    }
   


}
