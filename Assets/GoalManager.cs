using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalManager : MonoBehaviour {

    Text victoria;
	// Use this for initialization
	void Start () {
        victoria = GameObject.FindGameObjectWithTag("Win").GetComponent<Text>();
        victoria.enabled = false;
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider altre) {
	
        if (altre.tag == "Player")
        {
            victoria.enabled = true;
            Time.timeScale = 0;
        }

	}
}
