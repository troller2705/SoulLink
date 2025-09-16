using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class PlaceSigils : MonoBehaviour
{
    public GameObject sigilPrefab;
    public ARPlaneManager planeManager;
    public Camera mainCamera;

    [Range(1, 20)]
    public int sigilsPerPlane = 5; // how many sigils to spawn

    void Start()
    {
        if (!planeManager) planeManager = FindFirstObjectByType<ARPlaneManager>();
        if (!mainCamera) mainCamera = Camera.main;
    }

    // -------------------------------
    // Option 1: Scatter on a random plane but keep planes updating
    // -------------------------------
    public void SpawnSigilsOnRandomPlane()
    {
        if (planeManager.trackables.count == 0)
        {
            Debug.LogWarning("No planes detected yet!");
            return;
        }

        // Pick a random detected plane
        int randomIndex = Random.Range(0, planeManager.trackables.count);
        int i = 0;
        ARPlane chosenPlane = null;
        foreach (var plane in planeManager.trackables)
        {
            if (i == randomIndex)
            {
                chosenPlane = plane;
                break;
            }
            i++;
        }

        if (chosenPlane != null)
        {
            PlaceMultipleSigils(chosenPlane, sigilsPerPlane);
        }
    }

    // -------------------------------
    // Option 2: Scatter on all planes but keep planes updating
    // -------------------------------
    public void ScatterSigilsOnAllPlanes()
    {
        if (planeManager.trackables.count == 0)
        {
            Debug.LogWarning("No planes detected yet!");
            return;
        }

        foreach (var plane in planeManager.trackables)
        {
            PlaceMultipleSigils(plane, sigilsPerPlane);
        }
    }

    // -------------------------------
    // Option 3: Scatter and then freeze planes
    // -------------------------------
    public void ScatterSigilsAndFreezePlanes()
    {
        if (planeManager.trackables.count == 0)
        {
            Debug.LogWarning("No planes detected yet!");
            return;
        }

        foreach (var plane in planeManager.trackables)
        {
            PlaceMultipleSigils(plane, sigilsPerPlane);
        }

        // Disable further plane detection & hide existing planes
        planeManager.enabled = false;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }

    // -------------------------------
    // Helper: Place multiple sigils on a single plane
    // -------------------------------
    private void PlaceMultipleSigils(ARPlane plane, int count)
    {
        if (plane.boundary == null || plane.boundary.Length < 3)
        {
            Debug.LogWarning("Plane boundary not ready");
            return;
        }

        Vector3 center = plane.center;

        for (int i = 0; i < count; i++)
        {
            // pick a random boundary vertex
            Vector2 randomBoundaryPoint = plane.boundary[Random.Range(0, plane.boundary.Length)];

            // interpolate between center and that boundary point
            Vector2 randomPoint2D = Vector2.Lerp(center, randomBoundaryPoint, Random.value);

            // convert local plane coords → world space
            Vector3 randomPoint3D = plane.transform.TransformPoint(new Vector3(randomPoint2D.x, 0, randomPoint2D.y));

            GameObject tempSigil = Instantiate(sigilPrefab, randomPoint3D, Quaternion.identity);

            Debug.Log($"{tempSigil.transform.position}, {tempSigil.transform.rotation}, {tempSigil.transform.localScale}");
        }
    }
}
