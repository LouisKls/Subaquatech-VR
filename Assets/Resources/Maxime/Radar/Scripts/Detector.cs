using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Detector : MonoBehaviour
{
    void OnTriggerEnter (Collider col) {
        if (col.tag == "HiddenObject") {           
            col.GetComponent<Renderer>().enabled = true;
        }
    }

    void OnTriggerExit (Collider col) {
        if(col.tag == "HiddenObject") {
            col.GetComponent<Renderer>().enabled = false;
        }
    }
    
}
