using UnityEngine;

public class Validate : MonoBehaviour {

	protected void OnTriggerEnter(Collider collider) {
		if(collider.gameObject.layer == (int) Layer.OBJECT) {
			Rigidbody rigidbody = collider.GetComponent<Rigidbody>();

			if(rigidbody != null) {
				rigidbody.isKinematic = true;
			}
		}
	}
}
