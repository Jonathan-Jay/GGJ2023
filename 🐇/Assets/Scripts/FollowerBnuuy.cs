using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerBnuuy : MonoBehaviour
{
	public MainBnuuy bnuuy;
	public float jumpStrength = 5f;
	public float boredJumpStrength = 5f;
	public Vector2 boredomTime = new Vector2(5f, 2f);
	public Vector2 closeDistance = new Vector2(2f, 5f);
	public Vector2 randjumpScale = new Vector2(0.75f, 1.25f);

	float boredomTimer = -1f;
	bool grounded = false;
	bool dead = false;
	Rigidbody2D rb;

	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	private void Start() {
		if (bnuuy == null) {
			Die();
			return;
		}
		GetComponent<SpriteRenderer>().color = bnuuy.GetComponent<SpriteRenderer>().color;
	}

	private void FixedUpdate() {
		if (dead) return;

		float dist = Vector2.Distance(transform.position, bnuuy.transform.position);
		if (dist < closeDistance.x) {
			//use boredom timer, and set if not started
			if (boredomTimer <= -1f) {
				boredomTimer = Random.Range(boredomTime.x, boredomTime.y);
			}

			boredomTimer -= Time.deltaTime;
			if (boredomTimer <= 0) {
				boredomTimer = Random.Range(boredomTime.x, boredomTime.y);

				Jump(boredJumpStrength, dist);
			}
			return;
		}
		boredomTimer = -1;

		if (grounded) {
			//hop towards player
			Jump(Mathf.Lerp(boredJumpStrength, jumpStrength, (dist - closeDistance.x) / (closeDistance.y - closeDistance.x))
				* Random.Range(randjumpScale.x, randjumpScale.y), dist);
		}
	}

	private void OnCollisionStay2D(Collision2D other) {
		grounded = grounded || other.gameObject.layer != 8;
	}

	private void OnCollisionExit2D(Collision2D other) {
		grounded = false;
	}

	public void Die() {
		dead = true;
		Destroy(gameObject);
	}

	void AutoJump(float jump) {
		float val = jump * 0.707106781f;

		if (transform.position.x < bnuuy.transform.position.x) {
			rb.velocity = Vector2.one * val;
		}
		else {
			rb.velocity = (Vector2.left + Vector2.up) * val;
		}
	}

	public void Jump(float jump, float distance) {
		//math the jump angle
		float gravity = Physics2D.gravity.y * rb.gravityScale;
		float height = transform.position.y - bnuuy.transform.position.y;
		float angle = 0f;

		if (height < 5f && height > -5f) {
			float test = -gravity * distance / (jump * jump);

			if (test < -1 || test > 1) {
				AutoJump(jump);
				return;
			}

			angle = Mathf.Sin(test) * 0.5f;
		}
		else {
			//equation:			[  (  gx^2		)  ]
			//					[ -( ------ - h )  ]		 (  x  )
			// angle = [  cos-1	[  (   v^2		)  ] + tan^-1( --- )  ] / 2
			//					[ ---------------- ]		 ( -h  )
			//					[ (h^2 + x^2)^1/2  ]

			//	thank you Michel van Biezen for this video that helped find this equation: https://www.youtube.com/watch?v=bqYtNrhdDAY

			float test = (-(gravity * distance * distance / (jump * jump) - height) /		//	-(gx^2/v^2 - h) /
				Mathf.Sqrt(distance * distance + height * height)									//	sqrt(h^2 + x^2)
				);

			if (test < -1f || test > 1f) {
				AutoJump(jump);
				return;
			}

			angle = ( Mathf.Cos(test) + Mathf.Tan(distance / -height) ) * 0.5f;		//	( cos^-1(test) + tan^-1(x/-h) ) / 2
			if (angle < 0f)
				angle += Mathf.PI *0.5f;
		}

		if (angle < Mathf.PI * 0.16666f) {
			angle = Mathf.PI * 0.5f - angle;
		}

		if (transform.position.x < bnuuy.transform.position.x) {
			rb.velocity = new Vector2(jump * Mathf.Cos(angle), jump * Mathf.Sin(angle));
		}
		else {
			rb.velocity = new Vector2(-jump * Mathf.Cos(angle), jump * Mathf.Sin(angle));
		}
	}
}
