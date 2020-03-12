using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HandUtils))]
public class Pinch : MonoBehaviour {

	public Layer layer = Layer.OBJECT;

	protected HandUtils hand;
	protected HashSet<GameObject> contacts;
	protected Dictionary<GameObject, Transform> grabbed;
	protected bool isGrabbing;
    protected GameObject oldParent;

    protected void Awake() {
		contacts = new HashSet<GameObject>();
		grabbed = new Dictionary<GameObject, Transform>();

		hand = GetComponent<HandUtils>();

		isGrabbing = false;
	}

	protected void Update() {
		if(! isGrabbing && hand.Pinch) {
			Grab();
		}
		else if(isGrabbing && ! hand.Pinch) {
			Release();
		}
        Debug.Log(hand.Pinch);
	}

	protected void OnTriggerEnter(Collider other) {
		if(other.gameObject.layer == (int) layer && ! contacts.Contains(other.gameObject)) {
			contacts.Add(other.gameObject);
		}
	}

	protected void OnTriggerExit(Collider other) {
		if(contacts.Contains(other.gameObject)) {
			contacts.Remove(other.gameObject);
		}
	}

	protected void Grab() {
		foreach(GameObject contact in contacts)
        {
			if(contact != null)
            {
                if (contact.GetComponent<Collider>().transform.parent.name == "Flasks")
                {
                    //TODO - à vérifier 
                    grabbed.Add(contact, contact.transform);
                    oldParent = contact.GetComponent<Collider>().transform.parent.gameObject;
                    contact.GetComponent<Collider>().transform.parent = gameObject.transform;
                    Rigidbody R = contact.GetComponent<Collider>().attachedRigidbody;
                    R.isKinematic = true;
                }
            }
			else
            {
				grabbed.Remove(contact);
			}
		}

		isGrabbing = true;
	}

	protected void Release() {
		foreach(var pair in grabbed)
        {
			if(pair.Key != null)
            {
                //TODO
                pair.Key.transform.parent = oldParent.transform;
                pair.Key.GetComponent<Rigidbody>().isKinematic = false;
            }
		}

		grabbed.Clear();

		isGrabbing = false;
	}
}
