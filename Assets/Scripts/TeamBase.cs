using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBase : MonoBehaviour {
    public Transform mechSpawnPoint;
    public Transform tankSpawnPoint;
    public Transform jetSpawnPoint;

    public int team;

    // TODO: Until I find a better way to do this...
    public Texture2D tankRedMMIcon;
    public Texture2D tankBlueMMIcon;

    public void SpawnBotTank () {
        Debug.Log("Spawn Tank");

        Vector3 pt = tankSpawnPoint.position;
        Quaternion r = tankSpawnPoint.rotation;

        GameObject newTank =
            Instantiate(Resources.Load("BotTank2"), pt, r) as GameObject; // TIDYME: 2018-07-07: Avoid Resources.Load
        // newTank.GetComponent<TeamPlayer>().team = team;
        // TODO: Update team texture...

        // Add a minimap icon for the tank.
        Texture2D mmIconTex = (team == 0) ? tankBlueMMIcon : tankRedMMIcon;
        MinimapCamera.AddMinimapIconTo(newTank.transform, mmIconTex);
    }
}
