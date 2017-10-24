using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine_1911 : MonoBehaviour {

    private int maxCapacity = 8;
    public int capacity;
    private MeshRenderer renderedBullet;

    public void removeRound()
    {
        if (capacity > 0)
        {
            capacity -= 1;
        }
        if(capacity == 0)
        {
            renderedBullet.enabled = false;
        }
    }
    public void addRound()
    {
        if (capacity < maxCapacity)
        {
            capacity++;
        }
        if(renderedBullet.enabled == false)
        {
            renderedBullet.enabled = true;
        }

    }
	// Use this for initialization
	void Start () {
        renderedBullet = this.transform.GetChild(0).gameObject.GetComponent(typeof(MeshRenderer)) as MeshRenderer;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
