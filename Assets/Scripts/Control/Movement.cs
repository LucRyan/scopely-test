using UnityEngine;
using System.Collections;

public class Movement {
	
	public const int TERRAIN_MASK = (1 << 8);
	public const float LARGE = 16192.0f;

	private static float GetSpeedScale(Vector3 startPosition, Vector3 direction, int layerMask){

		// Get Raycast parameters
		Vector3 originalPos = startPosition;
		Vector3 moveDir = new Vector3 (direction.x, 0.0f, direction.z);
		if (moveDir.x == 0.0f && moveDir.z == 0.0f){
			return 0.0f;
		}
		moveDir.Normalize ();
		Vector3 checkPos = originalPos + (Vector3.up * LARGE);
		
		// Do Raycast and find normal of surface we are moving across
		RaycastHit hit;
		bool collided = Physics.Raycast (checkPos, Vector3.down, out hit, Mathf.Infinity, layerMask);
		if (collided) {
			return Mathf.Clamp01(Vector3.Dot(moveDir, hit.normal) + 1);
		} else {
			return 0.0f;
		}
	}

	public static bool Move(GameObject gameObject, Vector3 direction, float speed){

		// What 2D direction are we moving in?
		Vector3 moveDir = new Vector3 (direction.x, 0.0f, direction.z);
		moveDir.Normalize ();

		// Get Raycast layer parameter
		int layerMask = LayerMask.NameToLayer("Terrain");
		if (layerMask <= 0){
			Debug.LogError("Movement: Move: could not find custom 'Terrain' layer");
			return false;
		}
		layerMask = (1 << layerMask);
		
		// Adjust speed based on deltaTime
		float adjustedSpeed = speed * Time.deltaTime;

		// If there is no RigidBody, adjust speed when moving uphill
		if (gameObject.rigidbody == null){
			adjustedSpeed *= GetSpeedScale(gameObject.transform.position, direction, layerMask);
		}

		// Determine next position
		Vector3 estimatedNewPos = gameObject.transform.position + (moveDir * adjustedSpeed);

		// Do terrain Raycast and update object position
		RaycastHit hit;
		bool collided = Physics.Raycast (estimatedNewPos + Vector3.up * LARGE, Vector3.down, out hit, Mathf.Infinity, layerMask);
		if (!collided) {
			Debug.LogError("Movement: Move: no terrain below startPosition");
			return false;
		}

		// Set our new position
		if (gameObject.rigidbody == null){
			gameObject.transform.position = hit.point;
		}else{
			// If there is a RigidBody attached to this object, offset by height
			Vector3 newPos = hit.point;
			float height = gameObject.collider.bounds.max.y - gameObject.collider.bounds.min.y;
			newPos += new Vector3(0f, height * 0.55f, 0f);
			gameObject.transform.position = newPos;
		}

		return true;
	}
}
