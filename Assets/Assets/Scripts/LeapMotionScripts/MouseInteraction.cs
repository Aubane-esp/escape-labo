using UnityEngine;

public class MouseInteraction : MonoBehaviour {

	protected GameObject pivot;
	protected GameObject selected;
	protected Transform startParent;
	protected Vector3 startPosition;
	protected Vector3 previousMousePosition;
	protected float startDistance;

    protected GameObject oldParent;

    //public Camera camera;


    public float maxDistance = 10f;
	public float rotationSpeed = 0.001f; // In pixels per second
	public float scrollSpeed = 15f; // In degrees per scroll count

	protected void Start() {
		pivot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		pivot.transform.localScale = 0.005f * Vector3.one;
		pivot.name = "Pivot";
		pivot.SetActive(false);

        Camera.main.GetComponent<Camera>();

    }

	protected void Update () {
		// Get ray under mouse position
		//TODO - récupérer rayon sous la souris
		Ray ray = new Ray();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        int layerMask = 1 << 9;

        // Detect collision between ray and objects
        if (Physics.Raycast(ray, out hit, maxDistance, layerMask)) { //TODO
			// Select object
			if(selected == null && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Mouse1))) {
				Grab(hit);
			}
		}

		// Release object 
		if(selected != null && (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1))) {
			Release();
		}

		// Interact with selected object
		if(selected != null) {
			Move(ray);
		}

		// Save the current mouse position for next fame
		previousMousePosition = Input.mousePosition;
	}

	protected void Grab(RaycastHit hit) {
        //TODO
        selected = hit.collider.gameObject;
        // Highlight selected object
        Highlighter highlighter = hit.transform.GetComponent<Highlighter>(); 

		if(highlighter != null) {
			highlighter.disableAfterDuration = false;
			highlighter.highlight = true;
		}

        //Rigidbody rb = selected.AddComponent<Rigidbody>() as Rigidbody;
        selected.GetComponent<Rigidbody>().isKinematic = true;

        pivot.SetActive(true);

        pivot.transform.position = hit.point;

        oldParent = selected.transform.parent.gameObject;

        selected.transform.SetParent(pivot.transform);

    }

    protected void Release() {
        if (selected != null) {
			// Disable highlight for released object
			Highlighter highlighter = selected.GetComponent<Highlighter>();

			if(highlighter != null) {
				highlighter.highlight = false;
			}

            //TODO
            highlighter = null;
            selected.GetComponent<Rigidbody>().isKinematic = false;
            pivot.SetActive(false);

            selected.transform.SetParent(oldParent.transform);
        }

		// Clear selected variable
		selected = null;
	}

	protected void Move(Ray ray) {
        // Move object
        float wallDistance = maxDistance;

        if (Input.GetKey(KeyCode.Mouse0)) {
            // Get pivot distance on the ray in order to keep the object above the ground and inside the room
            //TODO
            startDistance = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - pivot.transform.position).magnitude;
            startDistance += 0.02f * Input.mouseScrollDelta.y;

            //Gestion des collisions avec les murs
            RaycastHit hit;
            int layerMask = 1 << 8;
            // Detect collision between ray and objects
            if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
            {
                wallDistance = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - hit.point).magnitude;
            }
            startDistance = Mathf.Min(startDistance, wallDistance);
            // Set pivot position
            pivot.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + ray.direction * startDistance;
            

        }
		// Rotate object
		else if(Input.GetKey(KeyCode.Mouse1)) {
            // Get distance traveled by the mouse since last frame
            //TODO
            //startPosition = Input.mousePosition - previousMousePosition;
            //selected.transform.Rotate(startPosition.x, startPosition.y, startPosition.z);
            // Rotate selected object around pivot point

            selected.transform.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed, Input.GetAxis("Mouse X") * rotationSpeed, 0);
        }
	}
}
