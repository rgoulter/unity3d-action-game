using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXBulletSparks : MonoBehaviour {
    GameObject sparkObj;
    int numSparks = 3;

    void Start () {
        for (int i = 0; i < numSparks; i++) {
            // Create a spark
            Vector3 position = transform.position;
            Quaternion rotation = Quaternion.FromToRotation(transform.forward, new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), -1));

            Instantiate(sparkObj, position, rotation);
        }

        Destroy(gameObject);
    }
}
