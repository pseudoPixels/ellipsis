using UnityEngine;
using System.Collections;

public class SteeringBehavioursAI : MonoBehaviour {


	//-----------------------------------------------------------
	//					Agent Attributes
	//-----------------------------------------------------------
	public GameObject target;
	public float maxForce = 50.0f;
	public float maxSpeed = 50.0f;






	//------------------------------------------------------------
	//						Arrive Variables
	//------------------------------------------------------------
	public enum Deceleration{slow =3, normal=2, fast=1}; // it defines how the player decelerates during arrive








	//---------------------------------------------------------------------
	//				Variable for wader steering behaviour
	//---------------------------------------------------------------------
	/*private Vector2 wanderSteeringDirection = new Vector2(0,0);
	public float wanderCircleRadius = 2.0f;
	public float wanderCircleOffset = 5.0f;
	public float wanderMaxJitter = 0.3f;*/




	//---------------------------------------------------------------------
	//				Variables for Obstacle Avoidance
	//---------------------------------------------------------------------
	/*public Transform leftRayPosition;
	public Transform rightRayPosition;
	public Transform centerRayPosition;*/












	void Start(){
		//-------------------------------------------------------------------
		// 					Wander Behaviour Initialization
		//-------------------------------------------------------------------
		//wanderSteeringDirection = new Vector2 (Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));


		target = GameObject.Find ("player");







	}

	void Update () {
		if (target != null) {
			Vector2 acc = (pursuit (target.rigidbody2D)) / rigidbody2D.mass;
			rigidbody2D.velocity += acc * Time.deltaTime;
			rigidbody2D.velocity = Vector3.ClampMagnitude (rigidbody2D.velocity, maxForce);

			rotateTowardsVelocity ();
		}

		
	}




//-------------------------------------------------------------------------
//                    	Obstacle Avoidance
//-------------------------------------------------------------------------
	/*public Vector2 obstacleAvoidance(){
		float avoidanceDistance = 1.50f;
		RaycastHit2D hit = Physics2D.Raycast (centerRayPosition.position, centerRayPosition.up.normalized, avoidanceDistance);
		Debug.DrawRay (centerRayPosition.position, centerRayPosition.up.normalized, Color.red);
		Vector2 avoidance = new Vector2 (0,0);
		bool detectedOnceAlready = false;
		if (hit != null && hit.collider != null && hit.transform != transform && detectedOnceAlready == false) {
			Debug.DrawRay(transform.position, transform.up, Color.red);
			var relativePos = transform.InverseTransformPoint(hit.collider.gameObject.transform.position);
			float avoidancLateralForceMultiplyer = 1.0f + (float)( (avoidanceDistance - relativePos.x) / avoidanceDistance);
			//Debug.Log (avoidancLateralForceMultiplyer);

			avoidance.y = ( hit.collider.transform.GetComponent<CircleCollider2D>().radius - relativePos.y ) * avoidancLateralForceMultiplyer;
			avoidance.x = ( hit.collider.transform.GetComponent<CircleCollider2D>().radius - relativePos.x) * 0.2f;
			detectedOnceAlready = true;

			//Debug.Log(avoidance);
			//avoidance = hit.normal * 20;


		}

		RaycastHit2D hitLeft = Physics2D.Raycast (leftRayPosition.position, leftRayPosition.up.normalized, avoidanceDistance);
		Debug.DrawRay (leftRayPosition.position, leftRayPosition.up.normalized, Color.red);
		if (hitLeft != null && hitLeft.collider != null && hitLeft.transform != transform && detectedOnceAlready == false) {
			var relativePos = transform.InverseTransformPoint(hitLeft.collider.gameObject.transform.position);
			float avoidancLateralForceMultiplyer = 0.50f + (float)( (avoidanceDistance - relativePos.x) / avoidanceDistance);
			//Debug.Log (avoidancLateralForceMultiplyer);
			
			avoidance.y += ( hitLeft.collider.transform.GetComponent<CircleCollider2D>().radius - relativePos.y ) * avoidancLateralForceMultiplyer;
			avoidance.x += ( hitLeft.collider.transform.GetComponent<CircleCollider2D>().radius - relativePos.x) * 0.2f;
			detectedOnceAlready = true;

			//avoidance += hitLeft.normal * 10;

		}


		RaycastHit2D hitRight = Physics2D.Raycast (rightRayPosition.position, rightRayPosition.up.normalized, avoidanceDistance);
		Debug.DrawRay (rightRayPosition.position, rightRayPosition.up.normalized, Color.red);
		if (hitRight != null && hitRight.collider != null && hitRight.transform != transform && detectedOnceAlready == false) {
			var relativePos = transform.InverseTransformPoint(hitRight.collider.gameObject.transform.position);
			float avoidancLateralForceMultiplyer = 0.50f + (float)( (avoidanceDistance - relativePos.x) / avoidanceDistance);
			//Debug.Log (avoidancLateralForceMultiplyer);
			
			avoidance.y += ( hitRight.collider.transform.GetComponent<CircleCollider2D>().radius - relativePos.y ) * avoidancLateralForceMultiplyer;
			avoidance.x += ( hitRight.collider.transform.GetComponent<CircleCollider2D>().radius - relativePos.x) * 0.2f;

			//avoidance += hitRight.normal * 10;
		}


		return avoidance;
	}








*/






//-------------------------------------------------------------------------
//                    Pursuit Behaviour
//If the agent facing already towards the target... then just seek to that 
//position. Otherwise predict the new position of the target and seek there.
//-------------------------------------------------------------------------
	public Vector2 pursuit(Rigidbody2D pursuitTarget){
		Vector2 toTarget =  (Vector2)pursuitTarget.position - (Vector2)rigidbody2D.transform.position;
		float relativeHeading = Vector2.Dot ((Vector2)pursuitTarget.position.normalized, (Vector2)rigidbody2D.velocity.normalized);

		//if the agent is 'facing' the target... then just seek towards it 
		if (Vector2.Dot ((Vector2)rigidbody2D.velocity.normalized, toTarget) > 0 && relativeHeading < -0.95) {
			return seek(pursuitTarget.position);
		}


		//In this case Agent is not facing towards the target.
		//So first of we predict the new position of the target and seek to 
		//the predicted position
		float lookAheadTime = toTarget.magnitude / (maxSpeed + pursuitTarget.velocity.magnitude);
		return seek ((Vector3) pursuitTarget.transform.position + (Vector3)pursuitTarget.velocity*lookAheadTime);
	}








//-------------------------------------------------------------------------
//                    Evade Behaviour
//Just the opposite behaviour to pursuit
//But here calculating the facing direction is meaningless
//-------------------------------------------------------------------------
	public Vector2 evade(Rigidbody2D evadeTarget){
		Vector2 toTarget =  (Vector2)evadeTarget.position - (Vector2)rigidbody2D.transform.position;

		float lookAheadTime = toTarget.magnitude / (maxSpeed + evadeTarget.velocity.magnitude);
		return flee((Vector3) evadeTarget.transform.position + (Vector3)evadeTarget.velocity*lookAheadTime);
	}







//-------------------------------------------------------------------------
//                    Wander Behaviour
//-------------------------------------------------------------------------
	/*public Vector2 wander(){

		Vector2 targetPosition = wanderSteeringDirection - (Vector2)transform.forward;
		targetPosition = new Vector2 (targetPosition.x + Random.Range(-wanderMaxJitter, wanderMaxJitter)*Time.deltaTime, targetPosition.y + Random.Range(-wanderMaxJitter, wanderMaxJitter)*Time.deltaTime);
		targetPosition = targetPosition.normalized;
		targetPosition *= wanderCircleRadius;

		wanderSteeringDirection = targetPosition + (Vector2)transform.forward * wanderCircleOffset;

		return wanderSteeringDirection;
	}*/















//-------------------------------------------------------------------------
//                    Arrive Behaviour
//-------------------------------------------------------------------------
	public Vector2 arrive(Vector3 arriveTargetPosition, Deceleration decelerationRate){
		Vector2 toTarget = arriveTargetPosition - rigidbody2D.transform.position;

		//calculate the remaining distance to the arrive target
		float distanceToCover = toTarget.magnitude;

		if (distanceToCover > 0) {

			//Because the Deceleration is int based enum
			//This value is used to tweak the deceleration as required.
			float deceleratoinTweaker = 0.3f;

			//calculate the required speed with the given deceleration
			float requiredSpeed = distanceToCover / (float)((float)decelerationRate * deceleratoinTweaker);


			//make sure the requiredSpeed calculated does not exceed the max spped
			requiredSpeed = Mathf.Min(maxSpeed, requiredSpeed);


			//Desired velocity is calculated just like the seek.
			//here we are not normalizing... cause we already calculated the distance to cover.
			//just divide by it to normalize... to lower the number of calculation.
			Vector2 desiredVelocity  =  toTarget * requiredSpeed / distanceToCover;

			return desiredVelocity - rigidbody2D.velocity;

			
		}



		//If distance is almost zero... then we have arrived the destination
		return new Vector2 (0,0);
	}









//-------------------------------------------------------------------------
//                    Flee Behaviour
//-------------------------------------------------------------------------
	public Vector2 flee(Vector3 fleeTargetPosition){
		
		Vector2 diff =  transform.position - fleeTargetPosition; 
		diff = diff.normalized;
		diff *= maxSpeed; 
		
		
		Vector2 desiredVelocity =(Vector2)diff  - (Vector2)rigidbody2D.velocity ;
		return desiredVelocity;
	}








//-------------------------------------------------------------------------
//                    Seek Behaviour
//-------------------------------------------------------------------------
	public Vector2 seek(Vector3 seekTargetPosition){

		Vector2 diff =  seekTargetPosition - transform.position; 
		diff = diff.normalized;
		diff *= maxSpeed; 


		Vector2 desiredVelocity =(Vector2)diff  - (Vector2)rigidbody2D.velocity ;
		return desiredVelocity;
	}









//-------------------------------------------------------------------------
//	This method make sure that
//	the Agent facing as the direction it is moving
//	Calculation is done based on the velocity direction of the Agent.
//-------------------------------------------------------------------------
	public void rotateTowardsVelocity(){
		Vector3 dir = rigidbody2D.velocity;
		float angle2 = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis (angle2 - 90, Vector3.forward);
	}





}
