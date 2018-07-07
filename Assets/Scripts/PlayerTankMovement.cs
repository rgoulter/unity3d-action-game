using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerTankMovement : MonoBehaviour {
    float speed = 3.0f;
    float rotateSpeed = 3.0f;

    void Update () {
        CharacterController controller = GetComponent<CharacterController>();

        // Rotate around y - axis
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotateSpeed, 0); // TIDYME: 2018-07-07: MAGIC constant

        // Move forward / backward
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Input.GetAxis("Vertical"); // TIDYME: 2018-07-07: MAGIC constant
        controller.SimpleMove(forward * curSpeed);
    }
}
