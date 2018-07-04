#pragma strict

var sparkObj : GameObject;
var numSparks : int = 3;

function Start () {
    var i = 0;

    var position : Vector3;
    var rotation : Quaternion;
    var sp : SFXBulletSpark;  // 20180701 STYLE: unused variable!

    for (i = 0; i < numSparks; i++) {
        // Create a spark
        position = transform.position;
        rotation = Quaternion.FromToRotation(transform.forward, new Vector3(Random.Range(-0.05, 0.05), Random.Range(-0.05, 0.05), -1));

        Instantiate(sparkObj, position, rotation);
    }

    Destroy(gameObject);
}
