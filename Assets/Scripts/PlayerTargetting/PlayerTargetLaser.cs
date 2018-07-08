using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetLaser : MonoBehaviour {
    public PlayerTargetter targetter;

    void Start () {
        // Targetting laser will belong to a 'sibling' object
        // of the targetting laser, for the mech.

        targetter = transform.parent.GetComponentInChildren<PlayerTargetter>();

        if (targetter == null) {
            Debug.Log("Could not find a targetter for the reticule");
            Destroy(gameObject);
        }
    }

    void Update () {
        GameObject target = targetter.currentTarget;

        GameObject laserCylinder = transform.GetChild(0).gameObject; // TIDYME 2018-07-07 MAGIC; should use property

        if (target != null) {
            laserCylinder.GetComponent<Renderer>().enabled = true; // make ourselves visible

            // Now we want to 'point to' the target,
            Vector3 targetPt = target.GetComponent<TeamPlayer>().GetAimAtPoint();

            transform.LookAt(targetPt);

            float dist = Vector3.Distance(transform.position, targetPt);
            Vector3 s = transform.localScale;
            s.z = dist / 2;
            transform.localScale = s;
        } else {
            laserCylinder.GetComponent<Renderer>().enabled = false;
        }
    }
}
