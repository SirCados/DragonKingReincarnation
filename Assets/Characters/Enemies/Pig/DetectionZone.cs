using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public string tagTarget = "Player";

    public List<Collider2D> DetectedObjects = new List<Collider2D>();

    public Collision2D col;
    


    // Update is called once per frame
    void OntriggerEnter2D(Collider2D Collider) { 
        if(Collider.gameObject.tag == tagTarget) {
            DetectedObjects.Add(Collider);
        }
    
        
    }

    private void OnTriggerEnter2D(Collider2D collider){


        DetectedObjects.Remove(collider);
    }
}
