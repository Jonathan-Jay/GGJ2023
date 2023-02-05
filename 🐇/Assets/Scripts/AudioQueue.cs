using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioQueue : MonoBehaviour
{
	public List<AudioSource> sounds = new List<AudioSource>();

	private void Awake() {
		if (sounds.Count == 0) {
			foreach (AudioSource audio in GetComponents<AudioSource>())
				sounds.Add(audio);
		}
	}

	int lastPlayed = -1;

	public void Play() {
		//play random sound, try to avoid Previously played
		int index = Random.Range(0, sounds.Count);
		if (sounds.Count > 1) {
			while (index == lastPlayed)
				index = Random.Range(0, sounds.Count);
		}
		
		sounds[index].Play();
		lastPlayed = index;
	}
}
