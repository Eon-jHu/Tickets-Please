using System.Collections.Generic;
using UnityEngine;

public class CollidableObject : MonoBehaviour
{
    // Private serialized class variables.
    [SerializeField] private ContactFilter2D Filter;
    [SerializeField] private List<Collider2D> CollidedObjects = new List<Collider2D>(1);

    // Private class variables.
    private Collider2D Collider;

    // --------------- Functions --------------- //

    protected virtual void Start()
    {
        Collider = GetComponent<Collider2D>(); // Get collider from game object.
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
