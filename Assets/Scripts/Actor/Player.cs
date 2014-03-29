using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour, IDamageable {
	
	// Rendering Constants
	public const float NEAR_Z = 0.3f;
	public const float DEFAULT_VIEW_DISTANCE = 1000.0f;
	public const float FOG_DEPTH = 800.0f;
	
	// Input Constants
	public const float SENSITIVITY_X = 4.0f;
	public const float SENSITIVITY_Y = 4.0f;
	public const float MAX_VERTICAL_LOOK_DEGREES = 40.0f;
	
	// Player Constants
	public const float MOVE_SPEED = 75.0f;
	public const float PLAYER_HEIGHT = 16.0f;
	public const float CALM_COOLDOWN = 0.3f;
	
	private Camera _cam;
	private float _rotX;
	private float _rotY;
	private Quaternion _initialCameraRot;
	
	// Player Attributes
	public GameObject playerbloodSpray;
	private int _health = 100;
	public int Health  // read-write instance property
    {
        get{return _health;}
        set{_health = value;}
    }
	private bool _dead = false;
	private float _calmCooldown;
	
	
	#region Unity Lifecycle
	void Start () {

		// Get required components and default camera orientation
		_cam = Camera.main;
		if (_cam == null){
			Debug.LogError("Player: Start: _cam is null");
			return;
		}

		_initialCameraRot = _cam.transform.rotation;

		// Capture the mouse
		Screen.showCursor = false;
		Screen.lockCursor = true;

		// Set default render settings
		DefaultFog();
		DefaultClipping();

		// Fall to ground if Camera was placed too high
		Move(Vector3.up, 0.0f);
	}

	void Update () {
		
		_calmCooldown -= Time.deltaTime;
		
		// Let the player look, shoot, and move
		UpdateLookRotation ();
		UpdatePosition ();
		UpdateCrosshairAccuracyByMoving();
	}
	#endregion

	#region Render Settings
	private void DefaultFog(){
		
		// Make fog respect the player's view distance
		RenderSettings.fog = true;
		RenderSettings.fogColor = new Color (0.25f, 0.3f, 0.35f);
		RenderSettings.fogMode = FogMode.Linear;
		RenderSettings.fogEndDistance = DEFAULT_VIEW_DISTANCE;
		RenderSettings.fogStartDistance = DEFAULT_VIEW_DISTANCE - FOG_DEPTH;
	}
	private void DefaultClipping(){
		
		// Adjust the clipping to match the player's view distance
		_cam.nearClipPlane = NEAR_Z;
		_cam.farClipPlane = DEFAULT_VIEW_DISTANCE;
	}
	#endregion

	#region Move Helpers
	private void Move(Vector3 direction, float speed){
		if (Movement.Move(this.gameObject, direction, speed)){
			this.transform.position += new Vector3(0, PLAYER_HEIGHT, 0);
		}
	}
	private void UpdatePosition(){
		
		// Check keyboard input
		Vector3 moveDirection = new Vector3();
		if (Input.GetKey (KeyCode.W)) {
			CrosshairMgr.ChangeCrosshairAccuracy("less");
			moveDirection += _cam.transform.forward;
		}
		if (Input.GetKey (KeyCode.S)) {
			moveDirection -= _cam.transform.forward;
		}
		if (Input.GetKey (KeyCode.D)) {
			moveDirection += _cam.transform.right;
		}
		if (Input.GetKey (KeyCode.A)) {
			moveDirection -= _cam.transform.right;
		}
		if (moveDirection.magnitude > 0.0f){
			moveDirection.Normalize();
			Move(moveDirection, MOVE_SPEED);
			CrosshairMgr.ChangeCrosshairAccuracy("less");
		}
	}
	private void UpdateLookRotation(){
		
		// Determine rotation based on mouse movement
		_rotX = (_rotX + (Input.GetAxis("Mouse X") * SENSITIVITY_X)) % 360;
		_rotY = Mathf.Clamp(_rotY + (Input.GetAxis("Mouse Y") * SENSITIVITY_Y), -MAX_VERTICAL_LOOK_DEGREES, MAX_VERTICAL_LOOK_DEGREES);
		
		// Update camera orientation
		Quaternion xQuaternion = Quaternion.AngleAxis(_rotX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis(_rotY, Vector3.left);
		_cam.transform.localRotation = _initialCameraRot * xQuaternion * yQuaternion;
	}
	private void UpdateCrosshairAccuracyByMoving()
	{
		// Early return if we are cooling down
		if (_calmCooldown < 0.0f) {
			_calmCooldown = CALM_COOLDOWN;
		} else {
			return;
		}
		
		CrosshairMgr.ChangeCrosshairAccuracy("more");
	}
	#endregion
	
	#region Damage
	public void DamageTaken(int amount, Vector3 damageDirection, Vector3 damagePosition)
	{
		Health -= amount;
		Debug.Log (_health);
		for(int i = 0; i < amount/5; i++)
		{
			GameGUIMgr.HeartHit();
		}
		if (Health <= 0)
		{
			_dead = true;	
		}
		EffectCreator.Instance.Effect(damagePosition, playerbloodSpray);
	}	
	#endregion
}
