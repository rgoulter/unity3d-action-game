using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {
    public GameObject followObj;
    public float cameraDistance = 3;

    private float currentRotation = 0;

    void Start () {
        /*
          Note on angles:
          Heck, don't care, but this seems to work okay as it is.
          I need to sit down and think to clean it up to something nicer.
          */
        currentRotation = (90 + followObj.transform.rotation.eulerAngles.y);
    }

    void Update () {
        // The idea here is that the camera is always a certain radius
        // from the game object, but we want to have a smooth transition
        // for when the player turns.. so we 'orbit' behind the player
        // to achieve a special effect.

        if (followObj == null) { return; }

        Transform tr = followObj.transform;
        Vector3 followPt = followObj.transform.position;

        // Get the direction followObj is facing. (i.e. its y axis)
        float goalRotation = (90 + followObj.transform.rotation.eulerAngles.y);
        currentRotation = Mathf.MoveTowardsAngle(currentRotation, goalRotation, (0.86f * 3.0f));

        // Now, update our rotation to be *closer* to the rotation of the player.
        // -- Player rotate speed is about 3.0 (degrees/tick).

        // Calculate the position this camera needs to be at
        float cx = cameraDistance * Mathf.Cos(currentRotation * Mathf.Deg2Rad);
        float cz = cameraDistance * -Mathf.Sin(currentRotation * Mathf.Deg2Rad);

        float cy = cameraDistance * Mathf.Tan(3 * Mathf.PI / 12);

        transform.position = followObj.transform.position + new Vector3(cx, cy, cz);

        // Now we focus on a certain point. Make this 'above' the player, to be fancy.
        Vector3 lookatPt = followObj.transform.position;
        lookatPt.y = 3; // MAGIC
        transform.LookAt(lookatPt);
    }

    public static CameraFollowPlayer GetCameraForPlayer (PlayerTankForceMovement p) {
        CameraFollowPlayer[] cams = UnityEngine.Object.FindObjectsOfType<CameraFollowPlayer>() as CameraFollowPlayer[];

        foreach (CameraFollowPlayer cam in cams) { // TIDYME: 2018-07-07: use foreach loop
            // This more/less assumes that followObj has the playerTankForceMovement script...
            if (cam.followObj.GetComponent<PlayerTankForceMovement>() != null &&
                cam.followObj.GetComponent<PlayerTankForceMovement>().Equals(p)){
                return cam;
            }
        }

        Debug.Log("Couldn't find cam for " + p);

        return null;
    }
}
