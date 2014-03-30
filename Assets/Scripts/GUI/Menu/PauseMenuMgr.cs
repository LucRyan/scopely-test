using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMenuMgr : MonoBehaviour {
	
	//For ResumeBtn Flag
	public static bool resumeFlag = false;
	//
	public static GameObject menuBG;
	public static GameObject helpBG;
	//
	private bool _isPaused = false;
	private float _savedTimeScale;
	//
	private GameObject _player;
	private List<GameObject> _buttons;
	private GameObject[] _panels;
	//
	private MouseLook _pml;
	private MouseLook _cml;
	
	#region Unity Lifecycle
	// Use this for initialization
	void Awake()
	{
		InitialPauseSystem();
	}
	// Update is called once per frame
	void Update ()
	{
		TogglePauseGame();
	}
	#endregion
	
	#region Menu Helper
	private void TogglePauseGame()
	{
		if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) || resumeFlag)
		{
			if(!_isPaused)
			{
				StartCoroutine(Pause());

			}
			else
			{
				StartCoroutine(UnPause());
				resumeFlag = false;
				
			}
		}
	}
	
	void InitialPauseSystem()
	{
		
		menuBG = this.transform.FindChild("MenuBG").gameObject;
		helpBG = this.transform.FindChild("HelpBG").gameObject;
		_player = GameObject.Find("Player");
		//
		_buttons = new List<GameObject>();
		_buttons.Add(GameObject.Find("ResumeBtn"));
		_buttons.Add(GameObject.Find("HelpBtn"));
		_buttons.Add(GameObject.Find("QuitBtn"));
		_panels = GameObject.FindGameObjectsWithTag("Panel");
		//
		_pml = _player.GetComponent<MouseLook>();
		_cml = _player.transform.FindChild("Main Camera").gameObject.GetComponent<MouseLook>();
		
		//
		helpBG.SetActive(false);
		menuBG.SetActive(false);
	}
	
	IEnumerator Pause()
	{
		//Pause Game
		_savedTimeScale = Time.timeScale;
		Time.timeScale = 0.0f;
		//
		menuBG.SetActive(true);
		_pml.enabled = false;
		_cml.enabled = false;
		Screen.showCursor = true;
		Screen.lockCursor = false;
		AudioListener.pause = true;
		//Animation
		AnimateMenuFly(menuBG, _buttons, true);
		DisableAllGuiButSelf();
		//
		_isPaused = true;
		yield return null;
	}
	
	IEnumerator UnPause()
	{
		//Resume Game
		Time.timeScale = _savedTimeScale;
		AnimateMenuFly(menuBG, _buttons, false);
		yield return new WaitForSeconds(0.9f);
		
		//
		_pml.enabled = true;
		_cml.enabled = true;
		Screen.showCursor = false; 	
		Screen.lockCursor = true;
		AudioListener.pause = false;
		//
		menuBG.SetActive(false);
		EnableAllGui();
		_isPaused = false;
	}
	
	void DisableAllGuiButSelf()
	{
		foreach(GameObject go in _panels)
		{
			go.SetActive(false);
		}
		this.gameObject.SetActive(true);
	}
	
	void EnableAllGui()
	{
		foreach(GameObject go in _panels)
		{
			go.SetActive(true);
		}
	}
	#endregion
	
	#region Animation
	void AnimateMenuFly(GameObject bg, List<GameObject> btns, bool isFlyIn)
	{
		if(isFlyIn)
		{	
			float posOffset = 0f;
			float delayOffset = 0.3f;
			
			TweenPosition bgTp = TweenPosition.Begin<TweenPosition>(bg, 0.3f);
			bgTp.from = new Vector3(0f, 500f, 0f);
			bgTp.to = Vector3.zero;
			bgTp.delay = 0f;
			bgTp.Play();
			
			//Debug.Log(btns.Count);
			for(int i = 0; i < btns.Count; i++)
			{
				GameObject go = btns[i];
				
				//Debug.Log(go.name);
				TweenPosition btnTp =  TweenPosition.Begin<TweenPosition>(go, 0.2f);
				btnTp.from = new Vector3(1000f, 55f - posOffset, 0f);
				btnTp.to = new Vector3(0f, 55f - posOffset, 0f);
				btnTp.delay = delayOffset;
				btnTp.Play();
				
				posOffset += 110f;
				delayOffset += 0.2f;
			}
		}
		else
		{
			float posOffset = 0f;
			float delayOffset = 0.0f;
			
			for(int i = 0; i < btns.Count; i++)
			{
				GameObject go = btns[i];
				
				TweenPosition btnTp =  TweenPosition.Begin<TweenPosition>(go, 0.2f);
				btnTp.from = new Vector3(0f, 55f - posOffset, 0f);
				btnTp.to = new Vector3(1000f, 55f - posOffset, 0f);
				btnTp.delay = delayOffset;
				btnTp.Play();
				
				posOffset += 110f;
				delayOffset += 0.2f;
			}
			
			TweenPosition bgTp = TweenPosition.Begin<TweenPosition>(bg, 0.3f);
			bgTp.from = Vector3.zero;
			bgTp.to = new Vector3(0f, 500f, 0f);
			bgTp.delay = delayOffset;
			bgTp.Play();
		}
	
	}
	#endregion
}
