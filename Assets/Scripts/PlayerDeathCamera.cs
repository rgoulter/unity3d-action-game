using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathCamera : MonoBehaviour {
    public PlayerRespawner respawner;

    void Start () {
        // TIDYME: 2018-07-08: Use property, rather than GetComponent
        respawner = GetComponent<CameraFollowPlayer>().followObj.GetComponent<PlayerRespawner>();

        if (respawner == null) {
            Debug.Log("Respawner is null in the object the camera is following!!");
        }
    }

    void OnEnable () {
        Debug.Log("Death cam enabled.");
        GetComponent<CameraFollowPlayer>().enabled = false;
    }

    void Update () {
        if (respawner.respawnTime < 0) {
            enabled = false;
            GetComponent<CameraFollowPlayer>().enabled = true;
        }
    }
}
