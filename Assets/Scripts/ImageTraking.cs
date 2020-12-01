using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTraking : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnePrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackableImageManager;

    private void Awake()
    {
        trackableImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach (GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnePrefabs.Add(prefab.name, newPrefab);
        }
    }


    private void OnEnable()
    {
        trackableImageManager.trackedImagesChanged += ImageChange;
    }

    private void OnDisable()
    {
        trackableImageManager.trackedImagesChanged -= ImageChange;
    }

    private void ImageChange(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnePrefabs[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.name;
        Vector3 position = trackedImage.transform.position;
        GameObject prefab = spawnePrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);
        foreach (GameObject item in spawnePrefabs.Values)
        {
            if (item.name != name)
            {
                item.SetActive(false);
            }
        }
    }
}
