using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    public bool release = false; // Un booléen pour relacher l'objet
    protected Transform oldParent; // L'ancien parent
    protected Collider objectSticked; // Pour sauvegarder les objets attachés
    protected bool freeHand;
    
    private void Start()
    {
        freeHand = true;
        oldParent = null;
        objectSticked = null;
    }

    protected void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.parent.name == "Flasks" && freeHand) // Si on touche une flasque
        {
            objectSticked = collision;
            oldParent = collision.transform.parent; // On conserve l'ancien parent
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = true; // On rend l'objet "kinematic" pour qu'il ne soit plus affecté par la gravité et les autre collision si j'ai bien compris
            collision.gameObject.transform.SetParent(gameObject.transform); // On change de parent pour qu'il suive le doigt

            freeHand = false;

        }
    }

    private void Update()
    {
        if(release && objectSticked != null) // Si on ne veut plus que l'objet soit sticky
        {
            objectSticked.transform.SetParent(oldParent);
            objectSticked.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            freeHand = true;
        }
    }
}
