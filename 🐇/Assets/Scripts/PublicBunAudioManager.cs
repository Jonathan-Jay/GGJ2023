using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicBunAudioManager : MonoBehaviour
{
	static PublicBunAudioManager instance;
	[SerializeField] AudioQueue jumpQueue;
	[SerializeField] AudioQueue landQueue;
	[SerializeField] AudioQueue hitQueue;
	[SerializeField] AudioQueue jumpQueueQuiet;
	[SerializeField] AudioQueue landQueueQuiet;
	[SerializeField] AudioQueue hitQueueQuiet;
	private void Start() {
		if (instance != null) {
			Destroy(instance);
		}

		instance = this;
	}

	static public void PlayJump() {
		instance?.jumpQueue.Play();
	}

	static public void PlayLand() {
		instance?.landQueue.Play();
	}

	static public void PlayHit() {
		instance?.hitQueue.Play();
	}

	//out of 100
	static public void PlayJump(int chance) {
		if (Random.Range(0, 100) < chance)
			instance?.jumpQueueQuiet.Play();
	}

	//out of 100
	static public void PlayLand(int chance) {
		if (Random.Range(0, 100) < chance)
			instance?.landQueueQuiet.Play();
	}

	//out of 100
	static public void PlayHit(int chance) {
		if (Random.Range(0, 100) < chance)
			instance?.hitQueueQuiet.Play();
	}
}
