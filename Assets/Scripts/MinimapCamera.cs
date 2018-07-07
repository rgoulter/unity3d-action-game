using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {
    Transform followPt;
    int height;

    void Start () {
        // We need to ensure a minimap icon is created for the objects which
        // are in the scene already.
        // For objects which are created afterwards, they will worry about this themselves.
        TowerLive[] towers = UnityEngine.Object.FindObjectsOfType(typeof(TowerLive)) as TowerLive[];
        foreach (TowerLive tower in towers) {
            tower.AddMinimapIcon();
        }
    }

    public static GameObject AddMinimapIconTo (Transform trans, Texture2D texture) {
        Vector3 position = trans.position;
        position.y = 5; // MAGIC

        Quaternion rot = UnityEngine.Quaternion.identity;

        GameObject mmIcon = Instantiate(Resources.Load("MinimapIcon"), // TIDYME: 2018-07-07: avoid using Resources.Load
            position,
            rot) as GameObject;
        mmIcon.transform.parent = trans; // Attach it to its parent

        // Now update the material/texture..
        mmIcon.GetComponent<Renderer>().material.mainTexture = texture;

        return mmIcon;
    }

    void Update () {
        // Set the x & z components of the minimap camera to be above followPt
        Camera cam = GetComponent<Camera>();
        Transform t = cam.transform;
        Vector3 p = t.position;
        p.x = followPt.position.x;
        p.z = followPt.position.z;
        p.y = height;
        cam.transform.position = p;
    }

    void OnGUI () {
        GUI.depth = 12;
        GUI.Box(new Rect(GetComponent<Camera>().pixelRect.x - 1, (Screen.height - GetComponent<Camera>().pixelRect.yMax - 1), GetComponent<Camera>().pixelWidth + 2, GetComponent<Camera>().pixelHeight + 2), "");
    }
}
