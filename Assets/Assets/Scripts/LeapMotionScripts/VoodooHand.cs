using UnityEngine;

[RequireComponent(typeof(HandUtils))]
[RequireComponent(typeof(Pinch))]
public class VoodooHand : MonoBehaviour {

	public enum Type {
		LEFT,
		RIGHT
	}

	public Type type;

	protected HandUtils hand;
	protected Pinch pinch;

	public HandUtils Hand {
		get {
			return hand;
		}
	}

	public Pinch Pinch {
		get {
			return pinch;
		}
	}

	protected void Awake() {
		hand = GetComponent<HandUtils>();
		pinch = GetComponent<Pinch>();
	}

	protected void OnEnable() {
		VoodooDoll manager = FindObjectOfType<VoodooDoll>();

		if(manager != null) {
			manager.SetHand(this);
		}
	}

	protected void OnDisable() {
		VoodooDoll manager = FindObjectOfType<VoodooDoll>();

		if(manager != null) {
			manager.RemoveHand(this);
		}
	}
}
