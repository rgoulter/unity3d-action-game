using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour {
    public float respawnTime = 3.0f;

    void OnEnable () {
        respawnTime = 3.0f;

        // Disable the components so that the player appears to be gone.
        GetComponent<Collider>().enabled = false;
        GetComponent<PlayerTankForceMovement>().enabled = false;
        Vector3 p = transform.position;
        p.y = -100;
        transform.position = p;

        // Disable mg shooters, so we can't shoot when we are dead.
        BulletShooter[] comps = transform.GetComponentsInChildren<BulletShooter>(true);
        foreach (BulletShooter comp in comps) {
            comp.enabled = false;
        }
    }

    void OnDisable () {
        // Re-enable shooters because we disabled them, as a cheap hack.
        BulletShooter[] comps = transform.GetComponentsInChildren<BulletShooter>(true);
        foreach (BulletShooter comp in comps) {
            comp.enabled = true;
        }
    }

    void Update () {
        respawnTime -= Time.deltaTime;

        if (respawnTime <= 0) {
            // Need to disable the respawn component,
            enabled = false;

            // and re-enable the components which we disabled before.
            GetComponent<Collider>().enabled = true;
            GetComponent<PlayerTankForceMovement>().enabled = true;


            // and "Respawn"
            // it's easier to just retain the object as it was.
            float bestDist = Mathf.Infinity;
            TeamBase baseToRespawnAt = null;
            TeamBase[] bases = FindObjectsOfType(typeof(TeamBase)) as TeamBase[];

            foreach (TeamBase teamBase in bases) {
                float distanceTo = Vector3.Distance(transform.position, teamBase.transform.position);

                // The closest base of the same team.
                if (teamBase.team == gameObject.GetComponent<TeamPlayer>().team &&
                    distanceTo < bestDist) {
                    baseToRespawnAt = teamBase;
                    bestDist = distanceTo;
                }
            }

            if (baseToRespawnAt == null) {
                // Game over...
            }

            GetComponent<TeamPlayer>().health = 100; // MAGIC: 2018-07-07: Assumes player has 100 initial health
            // FIXME: 2018-07-07: 
            transform.position = baseToRespawnAt.mechSpawnPoint.position;
            transform.rotation = baseToRespawnAt.mechSpawnPoint.rotation;

            // Get the player UI, so we can re-show the objective.
            GetComponent<PlayerUI>().objectiveOpacity = 0.5f + 1.0f; // MAGIC.
        }
    }
}
