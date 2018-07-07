using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXBulletSpark : MonoBehaviour {
    float speed = 500f;
    float drx = 0f;
    float dry = 0f;
    float drz = 0f;
    float lifetime = 0.3f;

    void Update () {
        lifetime -= Time.deltaTime;

        transform.Rotate(drx * Time.deltaTime, dry * Time.deltaTime, drx * Time.deltaTime);
        transform.Translate(transform.forward * speed * Time.deltaTime);

        if(lifetime < 0){
            Destroy(gameObject);
        }
    }
}
