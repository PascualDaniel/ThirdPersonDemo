using UnityEngine;

public class GravityBox : GravitySource {
	
	[SerializeField]
	float gravity = 9.81f;
	
	[SerializeField]
	Vector3 boundaryDistance = Vector3.one;

	void Awake () {
		OnValidate();
	}

	void OnValidate () {
		boundaryDistance = Vector3.Max(boundaryDistance, Vector3.zero);
	}

	void OnDrawGizmos () {
		Gizmos.matrix =
			Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(Vector3.zero, 2f * boundaryDistance);
	}
}