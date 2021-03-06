﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drop : MonoBehaviour
{
    private List<GameObject> collectedObjects;
    [SerializeField] private Text collectedText;

    // Start is called before the first frame update
    void Start()
    {
        collectedObjects = new List<GameObject>();
    }

    private void SetCountText()
    {
        collectedText.text = collectedObjects.Count.ToString();
    }

    // The pickupables colliders are disables when held.
    // If object is dropped when jn trigger this will fire!
    private void OnTriggerEnter(Collider other) 
    {   
        if(other.gameObject.tag == "Pickupable")
        {
            Debug.Log("COLLECTED: " + other.gameObject.name);
            other.gameObject.active = false;
            SetCountText();
        }
    }
 
}
