using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyHealthPOS : MonoBehaviour {
    void Start () {
        // Set text of mesh to be our cost
        int cost = PlayerScores.buyHealthCost;

        TextMesh textMesh = GetComponentInChildren<TextMesh>() as TextMesh;

        if (textMesh != null) {
            textMesh.text = "" + cost;
        }
    }
}
