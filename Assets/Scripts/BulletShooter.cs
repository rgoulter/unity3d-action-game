using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour {
    float shootDelay = 3.0f;
    float shootTimer = 0;
    GameObject spwnObj;
    string shootAxis = "Fire1";
    PlayerTargetter targetter;

    public int ammoCount = 1000;

    int currentSpawnChild = 0;

    void Start () {
        // Targetting laser will belong to a 'sibling' object
        // of the targetting laser, for the mech.

        targetter = transform.parent.GetComponentInChildren<PlayerTargetter>();

        if (targetter == null) { // TIDYME: 2018-07-07: Use assertion
            Debug.Log("Could not find a targetter for the projectile shooter.");
            Destroy(gameObject);
        }
    }

    void Update () {
        if (shootTimer < shootDelay) {
            shootTimer += Time.deltaTime;
        }

        if (Input.GetAxis(shootAxis) > 0 && shootTimer >= shootDelay && ammoCount > 0) {
            // Point to spawn the bullet at.
            // How we do this: We assume the shooter object has certain points
            // which are its children; we then pick the first child and shoot from that.
            Transform spawnTransform = transform.GetChild(currentSpawnChild);
            currentSpawnChild = (currentSpawnChild + 1) % transform.childCount; // move to next child
            Vector3 spawnPt = spawnTransform.position;


            // Rotation to spawn the bullet towards.
            // If the targetter has a target, aim at that.
            // Otherwise, just aim 'forward'.
            Quaternion quaternion;

            if (targetter.currentTarget != null) {
                Vector3 targetPt = targetter.currentTarget.transform.position;
                targetPt.y += 1;

                Quaternion oldRotation = spawnTransform.rotation;
                spawnTransform.LookAt(targetPt);
                quaternion = spawnTransform.rotation;
                spawnTransform.rotation = oldRotation;
            } else {
                quaternion = spawnTransform.rotation;
            }

            Instantiate(spwnObj, spawnPt, quaternion);
            shootTimer -= shootDelay;
            ammoCount -= 1;

            // Try for muzzle flash
            // TIDYME: 2018-07-07: Use Property, rather than GetComponentInChildren
            MuzzleFlash mflash = spawnTransform.gameObject.GetComponentInChildren<MuzzleFlash>();
            if (mflash != null) {
                mflash.Flash();
            }
        }
    }
}
