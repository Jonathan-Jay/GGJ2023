using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnHandler : MonoBehaviour
{
	PlayerInputManager manager;
	int counter = 0;
	int maxCounter = 0;
	private void Awake() {
		manager = GetComponent<PlayerInputManager>();
	}

	private void OnTriggerExit2D(Collider2D other) {
		//if a player leaves, update counter
		if (other.gameObject.CompareTag("Bnuuy"))
			--counter;

		if (counter == 0) {
			manager.DisableJoining();
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Bnuuy"))
			maxCounter = Mathf.Max(++counter, maxCounter);
		
		if (counter == maxCounter) {
			manager.EnableJoining();
		}
	}
}
