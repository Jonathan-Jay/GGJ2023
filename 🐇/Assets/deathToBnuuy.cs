using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class deathToBnuuy : MonoBehaviour
{
	public InputAction mouse;

	private void Awake() {
		mouse.performed += ctx => {
			Vector3 pos = ctx.ReadValue<Vector2>();
			pos.z = -Camera.main.transform.position.z;
			transform.position = Camera.main.ScreenToWorldPoint(pos);
		};

		mouse.Enable();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		FollowerBnuuy bnuuy = other.GetComponent<FollowerBnuuy>();
		if (bnuuy) {
			bnuuy.Die();
			return;
		}

		MainBnuuy Bnuuy = other.GetComponent<MainBnuuy>();
		if (Bnuuy) {
			Bnuuy.Hit(transform.position, 0.5f);
		}
	}
}
