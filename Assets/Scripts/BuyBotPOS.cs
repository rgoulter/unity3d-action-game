using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyBotPOS : MonoBehaviour {
    void Start () {
        int cost = PlayerScores.botTankCost;

        TextMesh textMesh = GetComponentInChildren<TextMesh>() as TextMesh;

        if (textMesh != null) {
            textMesh.text = "" + cost;
        }
    }
}
