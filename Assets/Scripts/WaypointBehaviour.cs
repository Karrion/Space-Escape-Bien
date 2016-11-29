using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointBehaviour : MonoBehaviour {
    
    public List<WaypointBehaviour> neighbours;
    public List<float> values = new List<float>();

    // Use this for initialization
    void Start () {
        
    }
	void Awake()
    {
        int index = 0;
        foreach(WaypointBehaviour neighbour in neighbours)
        {
            Vector3 direction = Vector3.zero;
            Vector3 origin = transform.position;
            Vector3 target = Vector3.zero;
            target = neighbour.transform.position;
            direction = target - origin;
            float distance = direction.magnitude;
            values.Add(distance);
        }
    }
	// Update is called once per frame
	void Update () {
        
	}
}
