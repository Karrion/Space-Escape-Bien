using UnityEngine;
using System.Collections;

public class Cover : MonoBehaviour {

    [HideInInspector] public bool occupied;
    GameObject[] enemies;
	
	void Start () {
        occupied = false;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}
	
    void OnCollisionEnter(Collision other)
    {
        foreach(GameObject enemy in enemies)
        {
            if(other.gameObject == enemy)
            {
                if (enemy.GetComponent<IA>().getRunToCover())
                {
                    occupied = true;
                }
            } 
        }
    }
	
    void Update()
    {
        Debug.Log(occupied);
    }

}
