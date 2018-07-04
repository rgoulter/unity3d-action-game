#pragma strict

var bluetank : Transform;
var currXPos : float;
var currZPos : float;
var maxRange : int = 5;
var minRange : int = -5;
var spawnXPos : float;
var spawnZPos : float;

function Update () {
    // if (Input.GetKeyDown("c")) {
    //     spawnXPos=this.transform.localPosition.x;
    //     spawnZPos=this.transform.localPosition.z;

    //     while (spawnXPos <= this.transform.localPosition.x + 2 && spawnXPos> = this.transform.localPosition.x - 2) {
    //         spawnXPos = this.transform.localPosition.x + Random.Range(minRange, maxRange);
    //     }
    //
    //     while (spawnZPos <= this.transform.localPosition.z + 2 && spawnZPos> = this.transform.localPosition.z - 2) {
    //         spawnZPos = this.transform.localPosition.z+Random.Range(minRange, maxRange);
    //     }

    //     var position : Vector3 = Vector3(4, 0, 10);

    //     Instantiate(bluetank, position, Quaternion.identity);


    // }
}
