using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    // protected GameObject SpeechBubble;

    // Private variables
    [SerializeField] private ContactFilter2D Filter;
    private List<Collider2D> CollidedObjects = new List<Collider2D>(1);
    private Collider2D Collider;

    // --------------- Functions --------------- //

    protected virtual void Start()
    {
        Collider = GetComponentInChildren<Collider2D>(); // Get collider from game object.
        // Filter.SetLayerMask(LayerMask.GetMask("NPCs")); // Only detect objects in the "NPCs" layer.

        // Find the speech bubble component with a tag.
        //SpeechBubble = transform.Find("SpeechBubble").gameObject;
    }

    protected virtual void Update()
    {
        // Call overlap collider.
        Collider.OverlapCollider(Filter, CollidedObjects);

        foreach(var o in CollidedObjects)
        {

          OnCollided(o.gameObject);
            
        }
    }

    protected virtual void OnCollided(GameObject _CollidedObject)
    {
        Debug.Log("Collided with " + _CollidedObject.name);
    }
}
