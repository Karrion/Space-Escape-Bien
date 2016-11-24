using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]

public class Vertex : MonoBehaviour {

    public int id;
    public List<Edge> neighbours;
    [HideInInspector]
    public Vertex prev;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
