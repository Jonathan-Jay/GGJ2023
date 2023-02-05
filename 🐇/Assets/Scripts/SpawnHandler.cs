using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnHandler : MonoBehaviour
{
	PlayerInputManager manager;
	SpriteRenderer sprite;
	int counter = 0;
	int maxCounter = 0;
	private void Awake() {
		manager = GetComponent<PlayerInputManager>();
		sprite = GetComponent<SpriteRenderer>();
	}

	private void OnTriggerExit2D(Collider2D other) {
		//if a player leaves, update counter
		if (other.gameObject.CompareTag("Bnuuy"))
			--counter;

		if (counter == 0) {
			manager.DisableJoining();
			sprite.enabled = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("Bnuuy"))
			maxCounter = Mathf.Max(++counter, maxCounter);
		
		if (counter == maxCounter) {
			manager.EnableJoining();
			sprite.enabled = true;
		}
	}

	public IEnumerator Reactivate() {
		yield return new WaitForSeconds(1f);
		manager.EnableJoining();
		sprite.enabled = true;
	}
}
