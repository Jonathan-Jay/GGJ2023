using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
	public Vector2 waitTime = new Vector2(10f, 20f);
	public Animator anim;

	int numOfPlayers = 0;
	float timer = 0f;
	private void Awake() {
		timer = Random.Range(waitTime.x, waitTime.y);
		enabled = false;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		enabled = true;
		if (++numOfPlayers == 1 && timer < waitTime.x)
			timer = Random.Range(waitTime.x, waitTime.y);

	}

	private void OnTriggerExit2D(Collider2D other) {
		if (--numOfPlayers == 0)
			enabled = false;
	}

	private void Update() {
		timer -= Time.deltaTime;
		if (timer <= 0f) {
			anim.SetTrigger("Stab");
			timer = Random.Range(waitTime.x, waitTime.y);
		}
	}
}
