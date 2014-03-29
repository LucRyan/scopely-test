using UnityEngine;
using System.Collections;

public class BloodSpray : MonoBehaviour {

	private float _delay;
	private ParticleSystem _particle;

	public AudioClip splatSound;
	
	#region Unity Lifecycle
	void Start () {
		_particle = this.GetComponent<ParticleSystem>();
		_delay = _particle.startLifetime;	
		AudioSource.PlayClipAtPoint(splatSound, this.transform.position);
	}
	void Update () {
		
		// Destroy self after the effect is done.
		_delay -= Time.deltaTime;
		if (_delay < 0.0f) {
			GameObject.Destroy(this.gameObject);
			return;
		}
	}
	#endregion
}
