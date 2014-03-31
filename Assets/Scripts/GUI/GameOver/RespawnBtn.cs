using UnityEngine;
using System.Collections;

public class RespawnBtn : MonoBehaviour {
	
	private const float WAIT_TIME = 3f;
	
	public GameObject LoadingScreen;
	private AsyncOperation _async;
	private bool _loaded;
	
	private float _waitTime;

	void OnClick()
	{
		_waitTime = WAIT_TIME;
		_loaded = false;
		
		StartCoroutine(LoadLevelAsync());
	}
	
	
	IEnumerator LoadLevelAsync()
	{
		
		Debug.Log("Scene Loading");
		
        _async = Application.LoadLevelAsync(Application.loadedLevel);
       	LoadingScreen.SetActive(true);
		
        while( !_async.isDone && _async.progress < 0.9f )
        {
            yield return 0;
        }
	}
	
}
