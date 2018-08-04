using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class BulletHitScanTest {
    /// <summary>
    ///  A simple test with:
    ///   - TestRig1
    ///     - A HitScanBullet, with damage of 5
    ///     - A TeamPlayerCube, with health of less-than 5.
    ///  The test checks that the HitScan bullet damages the thing in front of
    ///  it (& destroys it).
    /// </summary>
    /// <returns>Enumerator for co-routines</returns>
    [UnityTest]
    public IEnumerator TestBulletHitScanDestroysObject() {
        SceneManager.LoadScene("BulletHitScan");

        yield return null;

        // FRAME 0:
        // Scene just loaded,
        // Unity hasn't cleaned up the objects yet

        GameObject testRig1 = GameObject.Find("TestRig1");
        Assert.IsNotNull(testRig1);

        GameObject teamPlayerCube = testRig1.transform.Find("TeamPlayerCube").gameObject;
        Assert.IsNotNull(teamPlayerCube);

        yield return null;

        // FRAME 1:
        // The bullet has destroyed the object & Unity has cleaned it up
        // (since the TeamPlayer component would have had less-than 0 health).

        Transform teamPlayerCubeTransform = testRig1.transform.Find("TeamPlayerCube");

        Assert.IsNull(teamPlayerCubeTransform);
    }
}
