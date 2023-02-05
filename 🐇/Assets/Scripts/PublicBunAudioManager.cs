using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicBunAudioManager : MonoBehaviour
{
	static PublicBunAudioManager instance;
	[SerializeField] AudioQueue jumpQueue;
	[SerializeField] AudioQueue landQueue;
	[SerializeField] AudioQueue hitQueue;
	private void Start() {
		if (instance != null) {
			Destroy(instance);
		}

		instance = this;
	}

	static public void PlayJump() {
		instance.jumpQueue.Play();
	}

	static public void PlayLand() {
		instance.landQueue.Play();
	}

	static public void PlayHit() {
		instance.hitQueue.Play();
	}
}
