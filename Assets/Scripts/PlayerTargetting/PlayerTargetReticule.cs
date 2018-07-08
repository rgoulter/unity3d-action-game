using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetReticule : MonoBehaviour {
    // This is to show some kind of target/reticule on the
    // thing which the player is currently targetting.

    public PlayerTargetter targetter;
    public Camera forCamera;

    void Start () {
        // The targetter must be set externally.
        if (targetter == null) {
            Debug.Log("Could not find a targetter for the reticule");
            Destroy(gameObject);
        }
    }

    void Update () {
        GameObject target = targetter.currentTarget;

        Vector3 p;

        if (target != null) {
            // The following, commented-out code is appropriate for some kind of icon
            // is to float above a target's head. This could be useful. KIV.
            //// // Move to its position.
            //// p = target.transform.position;
            //// p.y = p.y + 1;
            //// gameObject.transform.position = p;

            //
            // Have our plane so that it's positioned facing the camera.
            //
            // The idea is: from the point of the target, move towards the camera point,
            //  then look at the camera.
            Vector3 targetPt = target.GetComponent<TeamPlayer>().GetAimAtPoint();
            Vector3 cameraPt = forCamera.transform.position;

            p = Vector3.MoveTowards(targetPt, cameraPt, 2); // 2 is MAGIC

            transform.position = p;
            transform.LookAt(cameraPt);
        } else {
            p = new Vector3(0, -10, 0);

            gameObject.transform.position = p;
        }
    }
}
