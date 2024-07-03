using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;

	[SerializeField, Range(0f, 10f)]
	float jumpHeight = 2f;

	[SerializeField, Range(0, 5)]
	int maxAirJumps = 0;

    Vector3 velocity, desiredVelocity;

    Rigidbody body;

	bool desiredJump;
	bool onGround;
	int jumpPhase;

	void Awake () {
		body = GetComponent<Rigidbody>();
	}
    void Update () {
		Vector2 playerInput;
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
		desiredJump |= Input.GetButtonDown("Jump");
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);
		

		//Vector3 desiredVelocity =
		desiredVelocity =
			new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
	}
    void FixedUpdate () {
		UpdateState();
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
		float maxSpeedChange = acceleration * Time.deltaTime;

        velocity.x =
			Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
		velocity.z =
			Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

		if (desiredJump) {
			desiredJump = false;
			Jump();
		}
        body.velocity = velocity;
		onGround = false;
    }
	void Jump() {
		if (onGround || jumpPhase < maxAirJumps) {
			jumpPhase += 1;
			float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
			if (velocity.y > 0f) {
				jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
			}
			velocity.y += jumpSpeed;
		}
	}
	void OnCollisionEnter (Collision collision) {
	
		EvaluateCollision(collision);
	}

	void OnCollisionStay (Collision collision) {
	
		EvaluateCollision(collision);
	}
	
	void EvaluateCollision (Collision collision) {
		for (int i = 0; i < collision.contactCount; i++) {
			Vector3 normal = collision.GetContact(i).normal;
			onGround |= normal.y >= 0.9f;
		}
	}
	void UpdateState () {
		velocity = body.velocity;
		if (onGround) {
			jumpPhase = 0;
		}
	}
}
