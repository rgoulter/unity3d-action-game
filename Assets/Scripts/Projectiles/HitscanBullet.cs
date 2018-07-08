using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanBullet : MonoBehaviour {
    // An alternative to MG Bullet, but instead,
    // to do damage, this will just hitscan to the
    // target.

    public float maxRange = 60f; // Roughly what the mgBullet has by default.
    public float damageAmount = 5f;

    void Start () {
        // Raycast for our target

        // Bit shift the index of the layer (2) to get a bit mask
        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, maxRange, layerMask)) {
            GameObject hitObject = hitInfo.collider.gameObject;

            if (hitObject.GetComponent<TeamPlayer>()) {
                hitObject.GetComponent<TeamPlayer>().health -= damageAmount;
            } else {
                Debug.Log("Hit non TP object, " + hitObject);
            }

            // TODO: Maybe create a 'sparkle' sfx at the hitpoint??
            Vector3 pt = hitInfo.point;
        }

        Destroy(gameObject);
    }
}
