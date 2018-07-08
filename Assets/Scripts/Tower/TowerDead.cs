using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDead : MonoBehaviour {
    /*
      Time to wait before spawning a new tower int this position.
      */
    public float spawnDelay = 5.0f;

    void Start () {
        spawnDelay = 5.0f;
    }

    void Update () {
        spawnDelay -= Time.deltaTime;

        if (spawnDelay < 0) {
            // Destroy me, and make an inactive tower here.
            Destroy(gameObject);
            GameObject blankTower = Instantiate(Resources.Load("TowerBlank"), // TIDYME: Avoid Resources.Load
                gameObject.transform.position,
                gameObject.transform.rotation) as GameObject;
            blankTower.transform.parent = transform.parent;

            // If this was on a captured base, set the team to the base team.
            if (blankTower.transform.parent != null &&
                blankTower.transform.parent.GetComponent<TeamBase>() != null) {
                // Get the team, and replace with live tower..
                int team = blankTower.transform.parent.GetComponent<TeamBase>().team;
                blankTower.GetComponent<TowerInactive>().ReplaceWithLiveTower(team);
            }
        }
    }
}
