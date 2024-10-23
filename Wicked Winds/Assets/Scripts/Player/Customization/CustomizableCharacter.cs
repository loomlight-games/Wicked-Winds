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
        // Load customization from memory
            // Instantiate the chosen items

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset(){
        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// Handles creation and destruction of items 
    /// </summary>
    public void UpdateBodyPart(CustomizableItem newItem){
        
        CustomizableItem currentItem = customization[newItem.bodyPart];
        
        // Current item not null
        if (currentItem != null){
            // Destroys it
            Destroy(currentItem.GO);

            // Is the same one wearing
            if(currentItem == newItem)
                // Unwears it
                customization[newItem.bodyPart] = null;
            // Is different
            else
                // Instantiates the new and updates dictionary
                InstantiateItem(newItem);
        // Current item is null
        }else
            // Instantiates the new and updates dictionary
            InstantiateItem(newItem);
    }

    /// <summary>
    /// Instantiates a copy of the item gameobject at the correspondant body transform as a child
    /// Updates the dictionary with the new item
    /// </summary>
    void InstantiateItem(CustomizableItem item){
        // Gets transform
        Transform bodyPartTransform = GetBodyPartTransform(item.bodyPart);

        // Instantiates a copy in that transform as a child of it
        GameObject GOcopy = Instantiate(item.gameObject,bodyPartTransform.position, bodyPartTransform.rotation, bodyPartTransform);
        GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale
        
        item.GO = GOcopy;
        //item.chosen = true;
        
        // Update dictionary
        customization[item.bodyPart] = item;
    }

    /// <summary>
    /// Instantiates the item of a body part stored in dictionary 
    /// </summary>
    void InstantiateBodyPart(BodyPart bodyPart){
        Transform bodyPartTransform = GetBodyPartTransform(bodyPart);
        CustomizableItem item = customization[bodyPart];

        GameObject GOcopy = Instantiate(item.gameObject,bodyPartTransform.position, bodyPartTransform.rotation, bodyPartTransform);
        GOcopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale
    }

    /// <summary>
    /// Save player customization in memory as a json
    /// </summary>
    void SaveCustomization(){

        // PlayerCustomization playerCustomization = new ();

        // // Loop through the dictionary and create a serializable version
        // foreach (var kvp in customization) {
        //     if (kvp.Value != null) {
        //         CustomizationData data = new () {
        //             bodyPart = kvp.Key,
        //             prefabName = kvp.Value.prefab.name,
        //             isChosen = kvp.Value.chosen,
        //         };
        //         playerCustomization.customizationItems.Add(data);
        //     }
        // }

        // // Serialize the list to JSON
        // string json = JsonUtility.ToJson(playerCustomization);
        // Debug.Log(json);
        // PlayerPrefs.SetString(PLAYER_CUSTOMIZATION_FILE, json);
    }

    /// <summary>
    /// Reads a json file with the player customization and loads the data
    /// </summary>
    void LoadCustomization(){

        // string json = PlayerPrefs.GetString(PLAYER_CUSTOMIZATION_FILE);

        // Debug.Log(json);

        // if (string.IsNullOrEmpty(json)) {
        //     Debug.LogWarning("No saved customization found.");
        //     return;
        // }

        // PlayerCustomization playerCustomization = JsonUtility.FromJson<PlayerCustomization>(json);

        // // Loop through the saved customization data and instantiate items
        // foreach (var loadedItem in playerCustomization.customizationItems) {
        //     // Load the prefab using Addressables
        //     Addressables.LoadAssetAsync<GameObject>(loadedItem.prefabName).Completed += handle => {
        //         if (handle.Status == AsyncOperationStatus.Succeeded) {
        //             GameObject prefab = handle.Result;
        //             CustomizableItem newItem = prefab.GetComponent<CustomizableItem>();
        //             newItem.bodyPart = loadedItem.bodyPart;
        //             Transform bodyPartTransform = GetBodyPartTransform(newItem.bodyPart);
        //             newItem.chosen = loadedItem.isChosen;
        //             if (newItem.chosen) InstantiateItem(newItem, bodyPartTransform);
        //         }
        //     };
        // }
    }

    /// <returns>Corresponding transform to body part</returns>
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