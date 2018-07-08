using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamPlayer : MonoBehaviour {
    // 0 == blue,
    // 1 == red
    public int team = 0;
    public Vector3 relAimAtPoint = new Vector3(0, 1, 0);

    public float health = 100;
    public int pointsValue;

    void Update () {
        if (health < 0) {
            // Add the # points to the *other* team
            PlayerScores.AddTeamScore(1 - team, pointsValue);

            if (GetComponent<PlayerTankForceMovement>() != null) {
                // Treat the player specially... since things can get a bit awkward otherwise

                // Any respawn / reassignment logic would need to happen here;
                GetComponent<PlayerRespawner>().enabled = true;

                health = 100; // So that the player doesn't keep adding score.

                CameraFollowPlayer cam = CameraFollowPlayer.GetCameraForPlayer(GetComponent<PlayerTankForceMovement>());
                if (cam == null) { Debug.Log("Couldn't find camera for the player... awkward."); } // TIDYME: 2018-07-07 Use assertion
                cam.GetComponent<PlayerDeathCamera>().enabled = true;
            } else {
                // // added explosion effect
                // if (this.tag == "Tank") {
                //     var explosion : GameObject = Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation);
                // }
                // var explosion : GameObject = Instantiate(Resources.Load("Explosion"), transform.position, transform.rotation) as GameObject;
                Destroy(gameObject);
            }
        }
    }

    public bool SameTeam (TeamPlayer other) {
        return other.team == team;
    }

    public Vector3 GetAimAtPoint (){
        return transform.position + relAimAtPoint;
    }
}
