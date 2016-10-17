using UnityEngine;
using System.Collections;

public class Mirilla : MonoBehaviour {
    private SpriteRenderer mirillaRenderer;
    // Use this for initialization
    void Start()
    {
        mirillaRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 pos = Input.mousePosition;
        // pos.z = transform.position.z - Camera.main.transform.position.z;
        pos.z = 2.6f;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}

