using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField] 
    private float forceMultiplier = 3500;

    [SerializeField]
    private GameObject aimingPoint;

    public void PlayBall()
    {
        var camTrans = CameraCache.Main.transform;
        var camPos = camTrans.position;

        // Spawning point 0.5m before the camera
        var forceVector = camTrans.forward * 0.5f;
#if UNITY_EDITOR
        // When running in editor, check if there's a fixed aiming point
        // so the effects of just changing the parameters of the physical
        // material (in stead of also moving and rotating the camera) can be observed.
        if (aimingPoint != null)
        {
            // Spawn 0.5m from the camera in direction of fixed aiming point
            forceVector = (aimingPoint.transform.position - camPos).normalized * 0.5f;
        }
#endif
        var ballLocation = camPos + forceVector;
        Instantiate(ballPrefab, ballLocation, Quaternion.identity);
        if (Physics.SphereCast(camPos, 0.05f, forceVector, out var hitInfo))
        {
            if (hitInfo.rigidbody != null)
            {
                hitInfo.rigidbody.AddForceAtPosition(
                    forceVector * forceMultiplier,
                    hitInfo.transform.position);
            }
        }
    }
}
