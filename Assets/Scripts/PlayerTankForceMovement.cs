using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankForceMovement : MonoBehaviour {
    float speed = 6.0f;
    float rotateSpeed = Mathf.PI;

    string forwardAxis = "Vertical";
    string turnAxis = "Horizontal";
    string strafeAxis = "Strafe";

    void Start () {
        GetComponent<TeamPlayer>().pointsValue = PlayerScores.killMechBonus;
    }

    void Update () {
        // Rotate around y - axis
        // TODO: rotate speed needs to shift to degrees/second, not per frame.
        transform.Rotate(0, Input.GetAxis(turnAxis) * rotateSpeed * Time.deltaTime * Mathf.Rad2Deg, 0);

        // Move forward / backward, and/or strafe
        Vector3 forwardVec = Input.GetAxis(forwardAxis) * Vector3.forward;
        Vector3 strafeVec = Input.GetAxis(strafeAxis) * Vector3.right;

        Vector3 moveVec = forwardVec + strafeVec;
        moveVec.Normalize();


        transform.Translate(moveVec * speed * Time.deltaTime);
    }


    void OnCollisionEnter (Collision collision) {
        //Debug.Log("Player got collision from:" + collision.collider.name);

        // What is this doing here?? The code for this kindof thing should
        // be in mgBullet only..
        // Alas, mgBullet collides with a trigger. So, we kinda have to have this code.
        if (collision.gameObject.GetComponent<Bullet>() != null) {
            // This seems to work now?
            Debug.Log("Mech take damage from mgBullet");
            GetComponent<TeamPlayer>().health -= collision.gameObject.GetComponent<Bullet>().damageAmount;

            Destroy(collision.gameObject);
        }
    }

    void OnDestroy () {
        // We got blown up or something.

        // Add/Remove points as necessary.

        // Now we need to "respawn".
        // Respawn policy: No delay (atm) TODO
        // Respawn policy: At an arbitrary(?) team-base's mech spawn.
        // Respawn policy: Weapons and ammo are kept...(?)
        // Respawn policy: Health will be 100

        // 2018-07-07: PlayerRespawner takes care of this; the code at the end was already commented out.
        //TeamBase baseToRespawnAt;
        //TeamBase[] bases = FindObjectsOfType(typeof(TeamBase)) as TeamBase[];
        //foreach (TeamBase teamBase in bases) {
        //    // Some base of the same team.
        //    if (teamBase.team == gameObject.GetComponent<TeamPlayer>().team) {
        //        baseToRespawnAt = teamBase;
        //        break;
        //    }
        //}

        /*

        var newPlayer : GameObject;
        // Can't just clone ourselves, since gameObj is null by now.
        newPlayer = Instantiate(Resources.Load("PlayerMech"),
                                baseToRespawnAt.mechSpawnPoint.position,
                                baseToRespawnAt.mechSpawnPoint.rotation);

    // Set the health to 100
    //newPlayer.GetComponent(TeamPlayer).health = 100;

    // ... and, re-associate the camera with the new player.
        var cameras : Camera[] = Camera.allCameras;
        for (var c : Camera in cameras) {
            if (c.GetComponent(CameraFollowPlayer) != null &&
               c.GetComponent(CameraFollowPlayer).followObj == null) {

                c.GetComponent(CameraFollowPlayer).followObj = newPlayer;
            }
        }
        */
    }
}
