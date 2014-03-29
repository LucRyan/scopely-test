using UnityEngine;
using System.Collections;

public class FireExplosion : MonoBehaviour {
	
	private float _start;
	private float _delay;
	private ParticleSystem _particle;

	public Light light;
	public AudioClip explosionSound;
	
	#region Unity Lifecycle
	void Start () {
		_particle = this.GetComponent<ParticleSystem>();
		_start = _particle.startLifetime;
		_delay = _particle.startLifetime;	
		AudioSource.PlayClipAtPoint(explosionSound, this.transform.position);
	}
	void Update () {
		
		// Destroy self after the effect is done. Animate light intensity
		_delay -= Time.deltaTime;
		if (_delay < 0.0f) {
			GameObject.Destroy(this.gameObject);
			return;
		}
		float ratio = _delay / _start;
		light.intensity = ratio;
	}
	#endregion
}
