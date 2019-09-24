using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTheLine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // An object has collided with the portal
        Debug.Log("collision");
        this.transform.parent.gameObject.BroadcastMessage("PortalHit", other.gameObject);
    }

    //public void OnTriggerStay2D(Collider2D other)
    //{
    //    Debug.Log("trigger");
    //    this.transform.parent.gameObject.BroadcastMessage("PortalHit", other.gameObject);
    //}
}
