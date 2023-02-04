using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainBnuuy : MonoBehaviour
{
	[SerializeField] float speed = 5f;
	[SerializeField] float jumpStrength = 5f;
	[SerializeField] float moveJumpScaler = 0.25f;

	PlayerInput inputs;
	Rigidbody2D rb;
	InputAction move;
	InputAction jump;
	InputAction call;

	Vector2 input = Vector2.zero;
	bool grounded = false;
	private void Awake() {
		inputs = GetComponent<PlayerInput>();
		rb = GetComponent<Rigidbody2D>();

		move = inputs.currentActionMap.asset.FindAction("Move");
		jump = inputs.currentActionMap.asset.FindAction("Jump");
		call = inputs.currentActionMap.asset.FindAction("Call");

		move.started += ctx => {
			StartCoroutine(Move());
		};
		move.performed += ctx => {
			input = ctx.ReadValue<Vector2>();
		};

		jump.started += ctx => {
			if (grounded) {
				Jump(input.y * 0.25f + 0.75f);
			}
		};
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (grounded) return;

		for (int i = other.contactCount - 1; i >= 0; --i) {
			if (other.GetContact(i).normal.y > 0.5f) {
				grounded = true;
				return;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D other) {
		grounded = false;
	}

	void Jump(float scaler) {
		rb.AddForce(Vector2.up * scaler * jumpStrength, ForceMode2D.Impulse);
	}

	static WaitForFixedUpdate wfu = new WaitForFixedUpdate();
	IEnumerator Move() {
		Vector2 velo;
		while (move.inProgress) {
			if (grounded) {
				if (input.y == 0f)
					Jump(moveJumpScaler);
			}
			else {
				velo = rb.velocity;
				velo.x = input.x * speed;
				rb.velocity = velo;
			}
			yield return wfu;
		}
	}
}
