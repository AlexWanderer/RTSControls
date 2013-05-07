using UnityEngine;
using System.Collections;

/// <summary>
/// Vehicle.
/// This is a drivable character class.
/// The vehicle will orient itself to the terrain.
/// </summary>


//directive to enforce that our parent Game Object has a Character Controller
[RequireComponent(typeof(CharacterController))]

public class Vehicle : MonoBehaviour
{
	//The Character Controller attached to this gameObject
	CharacterController characterController;

	// The linear gravity factor. Made available in the Editor.
	public float gravity = 100.0f;
	
	// mass of vehicle
	public float mass = 1.0f;

	// The initial orientation.
	private Quaternion initialOrientation;

	// The cummulative rotation about the y-Axis.
	private float cummulativeRotation;

	// The rotation factor, this will control the speed we rotate at.
	public float rotationSensitvity = 500.0f;

	// The "scout" is used to mark the future position of the vehicle.
	// It is made visible as a debugging aid.
	// The point it is placed at is used in alignment
	// and in keeping the vehicle from leaving the terrain.
	public GameObject scout;

	//variables used to align the vehicle with the terrain surface 
	public float lookAheadDist = 8.0f; 	// How far ahead the scout is place
	private Vector3 hitNormal; 			// Normal to the terrain under the vehicle
	private float halfHeight; 			// half the height of the vehicle
	private Vector3 lookAtPt; 			// used to align the vehicle; marked by scout
	private Vector3 rayOrigin; 			// point from which ray is cast to locate scout
	private RaycastHit rayInfo; 		// struct to hold information returned by raycast
	private int layerMask = 1 << 8; 	//mask for a layer containg the terrain

	//movement variables - exposed in inspector panel
	public float maxSpeed = 50.0f; 		//maximum speed of vehicle
	public float maxForce = 15.0f; 		// maximimum force allowed
	public float friction = 0.997f; 	// multiplier decreases speed
	
	//movement variables - updated by this component
	private float speed = 0.0f;  		//current speed of vehicle
	private Vector3 steeringForce; 		// force that accelerates the vehicle
	private Vector3 velocity; 			//change in position per second


	// Use this for initialization
	void Start ()
	{
		// Use GetComponent to save a reference to another component . 
		// This generic method is avalable from the parent Game Object.  
		// The class in the angle brackets <  > is the type of the component we need a reference to.
		characterController = gameObject.GetComponent<CharacterController> ();
		
		//save the quaternion representing our initial orientation from the transform
		initialOrientation = transform.rotation;
		
		//set the cummulativeRotation to zero.
		cummulativeRotation = 0.0f;
		
		//half the height of vehicle bounding box
		halfHeight = renderer.bounds.extents.y;
	}

	// Update is called once per frame
	void Update ()
	{
		// We will get our orientation before we move: rotate before translation	
		// We are using the left or right movement of the Mouse to steer our vehicle. 
		SteerWithMouse ();
		
		// calculate steering forces that will change our velocity
		CalcForces ();

		// forces must not exceed maxForce
		ClampForces (); 
		
		CalcVelocity ();
		
		//orient vehicle transform toward velocity 
		if (velocity != Vector3.zero) {
			transform.forward = velocity;
			MoveAndAlign ();
		}
	}
	

	//-----------------------------------steer with mouse------------------------------------		
	// In mouse steering, we keep track of the cumulative rotation on the y-axis which we can combine
	// with our initial orientation to get our current heading. We are keeping our transform level so that
	// right and left turning remains predictable even if our vehicle banks and climbs.	
	void SteerWithMouse ()
	{
		//Get the left/right Input from the Mouse and use time along with a scaling factor 
		// to add a controlled amount to our cummulative rotation about the y-Axis.
		cummulativeRotation += Input.GetAxis ("Mouse X") * Time.deltaTime * rotationSensitvity;
		
		//Create a Quaternion representing our current cummulative rotation around the y-axis. 
		Quaternion currentRotation = Quaternion.Euler (0.0f, cummulativeRotation, 0.0f);
		
		// Use the quaternion to update the transform of the vehicle's Game Object based on 
		// initial orientation and the accumulated rotation since the original orientation. 
		transform.rotation = initialOrientation * currentRotation;
	}

	//--------------------Accelerate with Arrow or WASD keys-----------------------		
	// Move 'forward' based on player input
	// If the user is pressing the up-arrow or W key this function will return a force 
	// to accelerate the vehicle along its z-axis, which is to say in the foward direction.
	private Vector3 KeyboardAcceleration ()
	{	
		Vector3 force;
		
		//dv is desired velocity
		Vector3 dv = Vector3.zero;
		
		//forward is positive z 
		dv.z = Input.GetAxis ("Vertical");
		
		//Take the moveDirection from the vehicle's local space to world space 
		//using the transform of the Game Object this script is attached to.
		dv = transform.TransformDirection (dv);
		
		//calculate the force needed to correct our current velocity
		dv *= maxSpeed;
		force = dv - transform.forward * speed;
		return force;
	}
	
	// Calculate the forces that alter velocity
	private void CalcForces ()
	{
		steeringForce = Vector3.zero;
		steeringForce += KeyboardAcceleration ();
	}

	// if steering forces exceed maxForce they are set to maxForce
	private void ClampForces ()
	{
		if (steeringForce.magnitude > maxForce) {
			steeringForce.Normalize ();
			steeringForce *= maxForce;
		}
	}
	
	// acceleration and velocity are calculated
	void CalcVelocity ()
	{
		// move in forward direction
		Vector3 moveDirection = transform.forward;
		
		// speed is reduced to simulate friction
		speed *= friction;
		
		// movedirection is scaled to get velocity
		velocity = moveDirection * speed;
		
		// acceleration is force/mass
		Vector3 acceleration = steeringForce / mass;
		
		// add acceleration to velocity
		velocity += acceleration * Time.deltaTime;
		
		// speed is altered by acceleration	
		speed = velocity.magnitude;
			
		if (speed > maxSpeed) {
			// clamp speed & velocity to maxspeed
			speed = maxSpeed;
			velocity = moveDirection * speed;
		}
	}

	//----------------------MoveAndAlign------------------------		
	// Alignment permits our vehicle to tilt to climb hills and 
	// bank to follow the camber of the path.
	// It is done after we move and the transform is restored to its level state 
	// by the mouse steering code at the beginning of the function 
	// as we prepare to orient and move again. 
	void MoveAndAlign ()
	{
		rayOrigin = transform.position + transform.forward * lookAheadDist;
		rayOrigin.y += 100;
		// A ray is cast from a position lookAheadDist ahead of the vehicle on its current path 
		// and high above the terrain. If the ray misses the terrain, we are likely to fall off, so
		// no move will take place.
		if (Physics.Raycast (rayOrigin, Vector3.down, out rayInfo, Mathf.Infinity, layerMask)) {
			//Apply net movement to character controller which keeps us from penetrating colliders.
			// Velocity is scaled by deltaTime to give the correct movement for the time elapsed
			// since the last update. Gravity keeps us grounded.
			characterController.Move (velocity * Time.deltaTime + Vector3.down * gravity);
			
			// Use lookat function to align vehicle with terrain and position scout
			lookAtPt = rayInfo.point;
			lookAtPt.y += halfHeight;
			transform.LookAt (lookAtPt, hitNormal);
			scout.transform.position = lookAtPt;
		}
	}

	// The hitNormal will give us a normal to the terrain under our vehicle
	// which we can use to align the vehicle with the terrain. It will be
	// called repeatedly when the collider on the character controller
	// of our vehicle contacts the collider on the terrain
	void OnControllerColliderHit (ControllerColliderHit hit)
	{	
		hitNormal = hit.normal;
	}
}

