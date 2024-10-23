using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CustomizableCharacter : MonoBehaviour {
    const string PLAYER_CUSTOMIZATION_FILE = "PlayerCustomization";

    // Types of body parts, basically of items
    [Serializable]
    public enum BodyPart{
        Head = 0, 
        UpperBody = 1, 
        LowerBody = 2, 
        Shoes = 3
    }

    // Positions for items GOs
    public Transform headTransform, upperBodyTransform, lowerBodyTransform, shoesTransform;
    
    // Dictionary that maintains the relation between each bodypart and its item with its GO
    
    public Dictionary<BodyPart, CustomizableItem> customization = new(){
        {BodyPart.Head, null},
        {BodyPart.UpperBody, null},
        {BodyPart.LowerBody, null},
        {BodyPart.Shoes, null},
    };

    /////////////////////////////////////////////////////////////////////////////////////////////
    // Start is called before the first frame update
    void Start()
    {
        //Reset();
        // Load customization from memory
        LoadCustomization();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
        PlayerPrefs.DeleteAll();
        LoadCustomization();
    }

    /// <summary>
    /// Given an item updates its correspondant body part
    /// </summary>
    public void RecognizeBodyPart (CustomizableItem newItem){
        switch(newItem.bodyPart){
            case BodyPart.Head:
                UpdateBodyPart(newItem, headTransform);
                break;
            case BodyPart.UpperBody:
                UpdateBodyPart(newItem, upperBodyTransform);
                break;
            case BodyPart.LowerBody:
                UpdateBodyPart(newItem, lowerBodyTransform);
                break;
            case BodyPart.Shoes:
               UpdateBodyPart(newItem, shoesTransform);
                break;
            default:
                break;
        }

        // Save customization in memory
        SaveCustomization();
    }

    /// <summary>
    /// Handles creation and destruction of items 
    /// </summary>
    void UpdateBodyPart(CustomizableItem newItem, Transform bodyPartTransform){
        
        CustomizableItem currentItem = customization[newItem.bodyPart];
        
        // Current item not null
        if (currentItem != null){
            // Different from the given
            if(currentItem != newItem){
                // Not chosen anymore
                currentItem.chosen = false;

                // Destroys the current item gameobject
                Destroy(currentItem.GO);

                InstantiateItem(newItem, bodyPartTransform);
            
            // Same as given
            }else{
                // Current is chosen
                if (currentItem.chosen)
                    InstantiateItem(newItem, bodyPartTransform);

                // Current is not chosen
                else
                    // Destroys the current item gameobject 
                    Destroy(newItem.GO);
            } 
        // Current item is null
        }else
            InstantiateItem(newItem, bodyPartTransform);
    }

    /// <summary>
    /// Instantiates a copy of the item gameobject at the correspondant body transform as a child
    /// </summary>
    void InstantiateItem(CustomizableItem item, Transform bodyPartTransform){
        GameObject GOcopy = Instantiate(item.gameObject,bodyPartTransform.position, bodyPartTransform.rotation, bodyPartTransform);
        GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale
        item.GO = GOcopy;
        customization[item.bodyPart] = item;
    }

    /// <summary>
    /// Save player customization in nmemory as a json
    /// </summary>
    void SaveCustomization(){
        PlayerCustomization playerCustomization = new ();

        // Loop through the dictionary and create a serializable version
        foreach (var kvp in customization) {
            if (kvp.Value != null) {
                CustomizationData data = new () {
                    bodyPart = kvp.Key,
                    prefabName = kvp.Value.prefab.name,
                    isChosen = kvp.Value.chosen,
                };
                playerCustomization.customizationItems.Add(data);
            }
        }

        // Serialize the list to JSON
        string json = JsonUtility.ToJson(playerCustomization);
        Debug.Log(json);
        PlayerPrefs.SetString(PLAYER_CUSTOMIZATION_FILE, json);
    }

    /// <summary>
    /// Reads a json file with the player customization and loads the data
    /// </summary>
    void LoadCustomization(){
        string json = PlayerPrefs.GetString(PLAYER_CUSTOMIZATION_FILE);

        Debug.Log(json);

        if (string.IsNullOrEmpty(json)) {
            Debug.LogWarning("No saved customization found.");
            return;
        }

        PlayerCustomization playerCustomization = JsonUtility.FromJson<PlayerCustomization>(json);

        // Loop through the saved customization data and instantiate items
        foreach (var loadedItem in playerCustomization.customizationItems) {
            // Load the prefab using Addressables
            Addressables.LoadAssetAsync<GameObject>(loadedItem.prefabName).Completed += handle => {
                if (handle.Status == AsyncOperationStatus.Succeeded) {
                    GameObject prefab = handle.Result;
                    CustomizableItem newItem = prefab.GetComponent<CustomizableItem>();
                    newItem.bodyPart = loadedItem.bodyPart;
                    Transform bodyPartTransform = GetBodyPartTransform(newItem.bodyPart);
                    newItem.chosen = loadedItem.isChosen;
                    if (newItem.chosen) InstantiateItem(newItem, bodyPartTransform);
                }
            };
        }
    }

    Transform GetBodyPartTransform(BodyPart bodyPart){
        return bodyPart switch
        {
            BodyPart.Head => headTransform,
            BodyPart.UpperBody => upperBodyTransform,
            BodyPart.LowerBody => lowerBodyTransform,
            BodyPart.Shoes => shoesTransform,
            _ => null,
        };
    }
}

[Serializable]
public class CustomizationData {
    public CustomizableCharacter.BodyPart bodyPart;
    public string prefabName;
    public bool isChosen;
}

[Serializable]
public class PlayerCustomization {
    public List<CustomizationData> customizationItems = new ();
}