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
	
	// Player Attributes
	public GameObject playerbloodSpray;
	public AudioClip walkingSound;
	
	private AudioSource _walkingAS;
	private Camera _cam;
	private bool _dead = false;
	private float _calmCooldown;
	private EndGameMgr _gameoverMgr;
	
	private int _health = 100;
	public int Health  // read-write instance property
    {
        get{return _health;}
        set{_health = value;}
    }

	
	#region Unity Lifecycle
	void Start () {

		// Get required components and default camera orientation
		_cam = Camera.main;
		if (_cam == null){
			Debug.LogError("Player: Start: _cam is null");
			return;
		}

		// Capture the mouse
		Screen.showCursor = false;
		Screen.lockCursor = true;

		// Set default render settings
		DefaultFog();
		DefaultClipping();
		
		//Set Gameover Manager
		_gameoverMgr = (EndGameMgr)GameObject.FindObjectOfType(typeof(EndGameMgr));
		if (_gameoverMgr == null){
			Debug.LogError("Player: Start: cant find _gameoverMgr");
		}
		
		//Set audio walking
		_walkingAS = this.gameObject.AddComponent<AudioSource>();
		_walkingAS.clip = walkingSound;
	}

	void Update () {
		
		_calmCooldown -= Time.deltaTime;
		
		UpdateCrosshairAccuracyByMoving();
		//playWalkingSound();
		
		if(_dead)
		{
			_gameoverMgr.GameOver();
		}
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
	private bool isMoving()
	{
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || 
		   Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
		   Input.GetKey(KeyCode.Space))
		{
			return true;	
		}
		else
		{
			return false;	
		}	
	}

	
	private void UpdateCrosshairAccuracyByMoving()
	{
		if(isMoving())
		{
			CrosshairMgr.ChangeCrosshairAccuracy("less");
		}
		
		// Early return if we are cooling down
		if (_calmCooldown < 0.0f) {
			_calmCooldown = CALM_COOLDOWN;
		} else {
			return;
		}
		
		CrosshairMgr.ChangeCrosshairAccuracy("more");
	}
	
		
	private bool isWalking()
	{
		if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || 
		   Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) )
		{
			return true;	
		}
		else
		{
			return false;	
		}	
	}
	
	private void playWalkingSound()
	{
		if(isWalking() && !_walkingAS.isPlaying)
		{
			_walkingAS.Play();
		}
		else
		{
			_walkingAS.Pause();
		}
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
