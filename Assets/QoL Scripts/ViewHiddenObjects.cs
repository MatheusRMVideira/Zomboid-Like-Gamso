using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewHiddenObjects : MonoBehaviour
{
    void Start()
    {
        //Disable mesh renderer for this object
        GetComponent<MeshRenderer>().enabled = false;
        
    }
}
