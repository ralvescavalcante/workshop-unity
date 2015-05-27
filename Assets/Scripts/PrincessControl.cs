﻿using UnityEngine;
using System.Collections;

public class PrincessControl : MonoBehaviour {
	private Animator animator;
	private GameObject playerFeet;
	private Rigidbody2D rigidbody2d;
	private readonly float JUMP_SPEED = 20f;
	private readonly float JUMP_TIME_MAX = 0.1f;
	private float jumpTime;
	private readonly float WALK_SPEED = 500f;
	private bool grounded;

	void OnGroundedChange(bool g) {
		grounded = g;
	}

	void Awake () {
		this.animator = GetComponent<Animator> ();
		this.rigidbody2d = this.GetComponent<Rigidbody2D> ();
	}
	void Start() {
		playerFeet = (GameObject) this.transform.FindChild ("PlayerFeet").gameObject;
	}
	
	void FixedUpdate() {
		//controla o pe do personagem
		playerFeet.SetActive(rigidbody2d.velocity.y <= 0);

		bool grounded = this.IsGrounded ();

		 
		Vector2 vel = this.rigidbody2d.velocity;
		vel.x = Input.GetAxisRaw ("Horizontal") * WALK_SPEED * Time.fixedDeltaTime;
		this.rigidbody2d.velocity = vel;

		//atualizar a animacao - walking x idle
		if (vel.x != 0) {
			this.animator.SetBool ("Moving", true);
		} else {
			this.animator.SetBool ("Moving", false);
		}
		//controla o lado
		if (vel.x < 0 && this.transform.localScale.x > 0) {
			//flip
			this.transform.localScale = new Vector3 (-1, 1, 1);
		} else if (vel.x > 0  && this.transform.localScale.x < 0) {
			//flip
			this.transform.localScale = new Vector3 (1, 1, 1);
		}

		if (jumpTime < JUMP_TIME_MAX) {
			jumpTime += Time.fixedDeltaTime;
			if(Input.GetButton("Jump")) {
				Vector2 velj = this.rigidbody2d.velocity;
				velj.y = JUMP_SPEED * (0.5f + 0.5f * jumpTime/JUMP_TIME_MAX);
				this.rigidbody2d.velocity = velj;
				this.animator.SetTrigger("Jump");
				GetComponent<AudioSource>().Play();
			} else {
				jumpTime = JUMP_TIME_MAX;
			}
		}
	}

	void Update() {
		bool grounded = this.IsGrounded ();
		this.animator.SetBool ("Grounded", grounded);

		if (grounded) {
			if(Input.GetButtonDown("Jump")) {
				Vector2 vel = this.rigidbody2d.velocity;
				vel.y = JUMP_SPEED * 0.5f;
				this.rigidbody2d.velocity = vel;
				this.animator.SetTrigger("Jump");
				jumpTime = 0;
				OnGroundedChange(false);
			}
		}

	}

	bool IsGrounded() {
		return this.grounded;
	}


}
