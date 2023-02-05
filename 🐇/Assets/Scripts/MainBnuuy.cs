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

	bool grounded = false;
	bool stunned = false;
	Vector2 input = Vector2.zero;
	Animator anim;
	SpriteRenderer sprite;
	private void Awake() {
		Camera.main.GetComponent<CameraController>().following.Add(transform);
		++Camera.main.GetComponent<CameraController>().playerCount;

		inputs = GetComponent<PlayerInput>();
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();

		if (inputs.playerIndex >= CameraController.colours.Count)
			sprite.color = CameraController.colours[0];
		else
			sprite.color = CameraController.colours[inputs.playerIndex];

		move = inputs.currentActionMap.asset.FindAction("Move");
		jump = inputs.currentActionMap.asset.FindAction("Jump");

		move.started += ctx => {
			StartCoroutine(Move());
		};

		move.performed += ctx => {
			input = ctx.ReadValue<Vector2>();
		};

		jump.started += ctx => {
			if (grounded && !stunned) {
				Jump(input.y * 0.5f + 1f);
			}
		};
	}

	private void OnDestroy() {
		if (Camera.main)
			--Camera.main.GetComponent<CameraController>().playerCount;
	}

	private void FixedUpdate() {
		if (rb.velocity.x > 0)	sprite.flipX = false;
		if (rb.velocity.x < 0)	sprite.flipX = true;
		if (rb.velocity.y > 0.25f)			anim.SetInteger("yvelo", 1);
		else if (rb.velocity.y < -0.25f)	anim.SetInteger("yvelo", -1);
		else								anim.SetInteger("yvelo", 0);
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (grounded) return;

		for (int i = other.contactCount - 1; i >= 0; --i) {
			if (other.GetContact(i).normal.y > 0.5f) {
				if (!grounded)
					PublicBunAudioManager.PlayLand();
				grounded = true;
				return;
			}
		}
	}

	private void OnCollisionExit2D(Collision2D other) {
		grounded = false;
	}

	void Jump(float scaler) {
		PublicBunAudioManager.PlayJump();
		rb.AddForce(Vector2.up * scaler * jumpStrength, ForceMode2D.Impulse);
	}

	public void Hit(Vector3 hitPoint, float stunTime) {
		//calculate direction
		Vector2 direction = transform.position - hitPoint;

		rb.velocity = direction.normalized * jumpStrength * 1.5f;
		if (!stunned)
			StartCoroutine(UnStun(stunTime));
	}

	IEnumerator UnStun(float waitTime) {
		PublicBunAudioManager.PlayHit();
		stunned = true;
		yield return new WaitForSeconds(waitTime);
		stunned = false;
	}

	static WaitForFixedUpdate wfu = new WaitForFixedUpdate();
	IEnumerator Move() {
		Vector2 velo;
		while (this != null && move.inProgress) {
			if (!stunned) {
				if (!grounded) {
					velo = rb.velocity;
					velo.x = input.x * speed;
					rb.velocity = velo;
				}
				else {
					if (Mathf.Abs(input.y) < 0.5f)
						Jump(moveJumpScaler);
				}
			}
			yield return wfu;
		}
	}
}
