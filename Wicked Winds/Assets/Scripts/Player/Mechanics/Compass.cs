using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Compass
{
    // Target
    //public Transform target; // Lista de targets del jugador
    // Rotation speed
    //public float speed = 2f; // Velocidad de rotación del jugador
    // LookCoroutine
    Transform compass, target;
    GameObject compassPrefab, prefabInstance;

    bool isIstanciated = false;
    
    // Start is called before the first frame update
    public void Start()
    {
        compass = PlayerManager.Instance.compassTransform;


        // Use Addressables to load the prefab by name/label
        Addressables.LoadAssetAsync<GameObject>("Compass").Completed += handle => 
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
                    compassPrefab = handle.Result;
                else
                    Debug.LogError("Failed to load prefab compass");
                
        };
    }

    // Update is called once per frame
    public void Update()
    {
        // Player has mission
        if (PlayerManager.Instance.hasActiveMission){
            // Not instantiated compass
            if (!isIstanciated){
                // Instantiates a copy of the prefab in that transform as a child of it
                prefabInstance = GameManager.Instance.InstantiateGO(compassPrefab, compass.position, compass.rotation, compass);
                compass = prefabInstance.transform;
                isIstanciated = true;
            }
            prefabInstance.SetActive(true);

            // Target is first target of list
            if (PlayerManager.Instance.currentTargets[0] != null)
                target = PlayerManager.Instance.currentTargets[0].transform;
            
            // Rotation to object
            Quaternion lookRotation = Quaternion.LookRotation(target.position - compass.position);
            // Transition rotation
            compass.rotation = Quaternion.Slerp(compass.rotation, lookRotation, PlayerManager.Instance.rotationSpeed * Time.deltaTime);
        }
        // No mission
        else {
            // Instantiated compass
            if (isIstanciated){
                // Destroys it
                //GameManager.Instance.DestroyGO(prefabInstance);
                prefabInstance.SetActive(false);
            }
        }
    }
}
