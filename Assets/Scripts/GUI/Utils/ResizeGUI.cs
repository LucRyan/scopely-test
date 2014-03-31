using UnityEngine;
using System.Collections;

public class ResizeGUI : MonoBehaviour {

	int lastWidth;
    int lastHeight;
    bool stay = true;
	
	#region Unity Lifecycle
    void Start(){
        CalculateRects();
        StartCoroutine( CheckForResize() );
    }
	
	//
    void OnDestroy(){
        stay = false;
    }
	#endregion
	
	#region Resize
    IEnumerator CheckForResize(){
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        while( stay ){
            if( lastWidth != Screen.width || lastHeight != Screen.height ){
                CalculateRects();
                lastWidth = Screen.width;
                lastHeight = Screen.height;
            }
            yield return new WaitForSeconds(0.3f);
        }

    }

	void CalculateRects()
	{
		float scale = (float)Screen.width / 1400;
	//	Debug.Log(scale);
		this.transform.localScale = new Vector3(scale, scale, 0);
		
	}
	#endregion
}
