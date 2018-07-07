using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    float lifetime = 3.0f;

    float shootSpeed = 20f;
    public float damageAmount = 5f;

    Texture2D rocketTex;

    void Start () {
        GetComponent<Rigidbody>().velocity = transform.forward * shootSpeed;

        GameObject mmIcon = MinimapCamera.AddMinimapIconTo(transform, rocketTex);
        Quaternion r = mmIcon.transform.localRotation;
        r.y = 180;
        mmIcon.transform.localRotation = r;
    }

    void Update () {
        if (lifetime > 0) {
            lifetime -= Time.deltaTime;
        } else {
            GameObject.Destroy(gameObject);
            return;
        }
    }

    void OnCollisionEnter (Collision collision) {
        Debug.Log("Bullet Collided with:" + collision.collider.name + ", gObj:" + collision.gameObject.name);

        // Generally, ignore triggers.
        if (collision.collider.isTrigger) {
            return;
        }

        // Damage any "Team Player" thing which we hit. (e.g. mech, tower, tank...).
        if (collision.gameObject.GetComponent<TeamPlayer>() != null) {
            TeamPlayer objectHit = collision.gameObject.GetComponent<TeamPlayer>();

            // Apply the damage.
            objectHit.health -= damageAmount;
        } else {
            Debug.Log("Hit non TP object, " + collision.gameObject);
        }

        // Since we hit something, destroy the bullet.
        Destroy(gameObject);
    }
}
