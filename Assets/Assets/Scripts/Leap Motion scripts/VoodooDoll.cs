using UnityEngine;

public class VoodooDoll : MonoBehaviour {

	public const float pinchThresholdFactor = 2.0f;

	protected VoodooHand leftHand;
	protected VoodooHand rightHand;
	protected GameObject doll;
	protected GameObject controlled;
	protected Vector3 offset;
	protected bool isGrabbing;

	protected void Awake() {
		leftHand = null;
		rightHand = null;
		doll = null;
		controlled = null;
		offset = Vector3.zero;
		isGrabbing = false;
	}

	protected void Update() {
		if(leftHand != null && rightHand != null) {
			if(! isGrabbing && leftHand.Hand.Pinch && rightHand.Hand.Pointed != null) {
				Grab(rightHand.Hand.Pointed);
			}
			else if(isGrabbing && ! leftHand.Hand.Pinch) {
				Release();
			}
		}
		else if(isGrabbing) {
			Release();
		}

		if(isGrabbing && doll != null && controlled != null) {
			Move();
		}
	}

	public void SetHand(VoodooHand hand) {
		if(gameObject.activeInHierarchy && enabled) {
			switch(hand.type) {
				case VoodooHand.Type.LEFT:
					if(leftHand == null) {
						leftHand = hand;

						leftHand.Hand.Trigger.radius *= pinchThresholdFactor;
						leftHand.Pinch.enabled = false;
					}

					break;
				case VoodooHand.Type.RIGHT:
					if(rightHand == null) {
						rightHand = hand;

						rightHand.Hand.showRay = true;
						rightHand.Pinch.layer = Layer.VOODOO_DOLL;
					}

					break;
			}
		}
	}

	public void RemoveHand(VoodooHand hand) {
		if(gameObject.activeInHierarchy && enabled) {
			switch(hand.type) {
				case VoodooHand.Type.LEFT:
					if(leftHand == hand) {
						leftHand.Pinch.enabled = true;
						leftHand.Hand.Trigger.radius /= pinchThresholdFactor;

						leftHand = null;
					}

					break;
				case VoodooHand.Type.RIGHT:
					if(rightHand == hand) {
						rightHand.Pinch.layer = Layer.OBJECT;
						rightHand.Hand.showRay = false;

						rightHand = null;
					}

					break;
			}
		}
	}

	protected void Grab(GameObject pointed) {
		//TODO

		rightHand.Hand.showRay = false;

		isGrabbing = true;
	}

	protected void Release() {
		Vector3 p = controlled.transform.position;
		controlled.transform.position = new Vector3(
			Mathf.Clamp(p.x, -0.35f, 0.35f),
			Mathf.Clamp(p.y, 0.0f, 0.45f),
			Mathf.Clamp(p.z, -0.2f, 0.35f)
		);

		//TODO

		if(rightHand != null) {
			rightHand.Hand.showRay = true;
		}

		isGrabbing = false;
	}

	protected void Move() {
		//TODO
	}
}
