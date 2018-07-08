using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetter : MonoBehaviour {
    // Use this for managing how a player aims at things.
    // -- apply to a 'trigger'.

    public GameObject currentTarget;

    public float maxRange = 8.0f;

    void Start () {
        currentTarget = null;
    }

    void Update () {
        // parent here must be playermech. Should be fine as long as this targetter used only for players
        if (currentTarget != null &&
            maxRange < Vector3.Distance(transform.parent.position, currentTarget.transform.position)) {
            currentTarget = null;
        }
    }

    void OnTriggerEnter (Collider other) {
        GameObject otherObj = other.gameObject;

        if (otherObj.GetComponent<TeamPlayer>() != null) {
            //Debug.Log("entered team target: " + other.gameObject.name);

            if (currentTarget == null) {
                currentTarget = otherObj;
            }
        }
    }

    void OnTriggerExit (Collider other) {
        GameObject otherObj = other.gameObject;

        if (otherObj.Equals(currentTarget)) {
            //Debug.Log("Lost focus on target");

            currentTarget = null;
        }
    }
}
