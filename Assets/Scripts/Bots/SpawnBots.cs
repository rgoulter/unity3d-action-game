using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBots : MonoBehaviour {
    Transform bluetank;
    float currXPos;
    float currZPos;
    int maxRange = 5;
    int minRange = -5;
    float spawnXPos;
    float spawnZPos;

    void Update () {
        // if (Input.GetKeyDown("c")) {
        //     spawnXPos = this.transform.localPosition.x;
        //     spawnZPos = this.transform.localPosition.z;

        //     while (spawnXPos <= this.transform.localPosition.x + 2 && spawnXPos> = this.transform.localPosition.x - 2) {
        //         spawnXPos = this.transform.localPosition.x + Random.Range(minRange, maxRange);
        //     }
        //
        //     while (spawnZPos <= this.transform.localPosition.z + 2 && spawnZPos> = this.transform.localPosition.z - 2) {
        //         spawnZPos = this.transform.localPosition.z+Random.Range(minRange, maxRange);
        //     }

        //     Vector3 position = Vector3(4, 0, 10);

        //     Instantiate(bluetank, position, Quaternion.identity);


        // }
    }
}
