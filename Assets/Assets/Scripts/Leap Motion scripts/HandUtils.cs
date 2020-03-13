using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HandUtils : MonoBehaviour {

	public const float maxSelectionDistance = 1000.0f;

	public GameObject pinchWith;

	public bool showRay = false;

	protected SphereCollider trigger;
	protected GameObject ray;
	protected GameObject pointed;
	protected bool pinch;

	public SphereCollider Trigger {
		get {
			if(trigger == null) {
				trigger = GetComponent<SphereCollider>();
			}

			return trigger;
		}
	}

	public GameObject Pointed {
		get {
			return pointed;
		}
	}

	public bool Pinch {
		get {
			return pinch;
		}
	}

	protected void Awake() {
		pinch = false;

		trigger = GetComponent<SphereCollider>();

		trigger.isTrigger = true;

		ray = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

		ray.name = "Ray";
		ray.transform.parent = transform;
		ray.transform.localPosition = Vector3.forward;
		ray.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);
		ray.transform.localScale = new Vector3(0.002f, 1.0f, 0.002f);

		Destroy(ray.GetComponent<Collider>());
	}

	protected void Update() {
		ray.SetActive(showRay);

		// Detect pinch between index and thumb
		//TODO

		pinch = false;

		RaycastHit hit;
        float dist = (transform.position - pinchWith.transform.position).magnitude;
        Debug.Log(dist);
		if(Physics.Raycast(new Ray(transform.position, transform.forward), out hit, maxSelectionDistance, (1 << (int) Layer.OBJECT))) {
			SetHighLight(pointed, false);

			pointed = hit.collider.gameObject;

			if(showRay) {
				SetHighLight(pointed, true);
			}
		}
		else {
			SetHighLight(pointed, false);

			pointed = null;
		}
	}

	protected static void SetHighLight(GameObject go, bool set) {
		if(go != null) {
			Highlighter highlight = go.GetComponent<Highlighter>();

			if(highlight != null) {
				if(set) {
					highlight.disableAfterDuration = false;
					highlight.highlight = true;
				}
				else {
					highlight.highlight = false;
				}
			}
		}
	}
}
