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
	public float doubleJumpTime = 1.5f;

	float boredomTimer = -1f;
	float doubleJumpTimer = 5f;
	bool grounded = false;
	bool touchBun = false;
	bool dead = false;
	Rigidbody2D rb;
	Animator anim;
	SpriteRenderer sprite;
	private void Awake() {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer>();
	}

	private void Start() {
		if (bnuuy == null) {
			Die();
			return;
		}
		GetComponent<SpriteRenderer>().color = bnuuy.GetComponent<SpriteRenderer>().color;
	}

	private void FixedUpdate() {
		if (rb.velocity.x > 0)	sprite.flipX = false;
		if (rb.velocity.x < 0)	sprite.flipX = true;
		if (rb.velocity.y > 0.25f)			anim.SetInteger("yvelo", 1);
		else if (rb.velocity.y < -0.25f)	anim.SetInteger("yvelo", -1);
		else								anim.SetInteger("yvelo", 0);

		if (doubleJumpTimer > 0f) {
			doubleJumpTimer -= Time.deltaTime;
		}

		if (dead) return;

		float dist = Vector2.Distance(transform.position, bnuuy.transform.position);
		if (dist < closeDistance.x) {
			//use boredom timer, and set if not started
			if (boredomTimer <= -5f) {
				boredomTimer = Random.Range(boredomTime.x, boredomTime.y);
			}

			boredomTimer -= Time.deltaTime;
			if (boredomTimer <= 0f) {

				if (touchBun) {
					if (doubleJumpTimer <= 0f) {
						Jump(boredJumpStrength, dist);
						doubleJumpTimer = doubleJumpTime;
						boredomTimer = Random.Range(boredomTime.x, boredomTime.y);
					}
				}
				else {
					Jump(boredJumpStrength, dist);
					boredomTimer = Random.Range(boredomTime.x, boredomTime.y);
				}
			}
			return;
		}
		boredomTimer = -5f;

		if (grounded) {
			if (touchBun) {
				if (doubleJumpTimer <= 0f) {
					//hop towards player
					Jump(Mathf.Lerp(boredJumpStrength, jumpStrength, (dist - closeDistance.x) / (closeDistance.y - closeDistance.x))
						* Random.Range(randjumpScale.x, randjumpScale.y), dist);
					doubleJumpTimer = doubleJumpTime;
				}
			}
			else {
				Jump(Mathf.Lerp(boredJumpStrength, jumpStrength, (dist - closeDistance.x) / (closeDistance.y - closeDistance.x))
					* Random.Range(randjumpScale.x, randjumpScale.y), dist);
			}
		}
	}

	private void OnCollisionStay2D(Collision2D other) {
		if (!grounded)
			PublicBunAudioManager.PlayLand(25);
		grounded = true;
		touchBun = other.gameObject.layer == 8;
	}

	private void OnCollisionExit2D(Collision2D other) {
		grounded = false;
	}

	public void Die() {
		if (dead)	return;

		PublicBunAudioManager.PlayHit(50);

		dead = true;
		//set yourself to background
		gameObject.layer = 10;
		transform.GetChild(0).gameObject.layer = 10;

		//do some funny stuff
		rb.gravityScale = 1f;
		rb.velocity = Vector2.up * jumpStrength * 2f + Vector2.right * Random.Range(-boredJumpStrength, boredJumpStrength);

		Destroy(gameObject, 5f);
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
		PublicBunAudioManager.PlayJump(50);

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
