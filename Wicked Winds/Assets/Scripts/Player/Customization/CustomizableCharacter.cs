using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CustomizableCharacter : MonoBehaviour {
    const string PLAYER_CUSTOMIZATION_FILE = "PlayerCustomization";
    const string PLAYER_PURCHASED_ITEMS_FILE = "PlayerPurchasedItems";

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
    public Dictionary<BodyPart, CustomizableItem> currentCustomization = new(){
        {BodyPart.Head, null},
        {BodyPart.UpperBody, null},
        {BodyPart.LowerBody, null},
        {BodyPart.Shoes, null},
    };

    // List of purchased items
    public List<CustomizableItem> purchasedItems = new();

    /////////////////////////////////////////////////////////////////////////////////////////////
    void Awake()
    {
        // Load customization
        LoadCustomization();
    }

    /// <summary>
    /// Change body part according to item received, creationg or destroying it
    /// </summary>
    public void UpdateBodyPart(CustomizableItem newItem){
        
        CustomizableItem currentItem = currentCustomization[newItem.bodyPart];
        
        // Current item not null
        if (currentItem != null){
            // Destroys it
            Destroy(currentItem.instance);

            // Is the same one wearing
            if(currentItem == newItem)
                // Unwears it
                currentCustomization[newItem.bodyPart] = null;
            // Is different
            else
                // Instantiates the new and updates dictionary
                InstantiateItem(newItem);
        // Current item is null
        }else
            // Instantiates the new and updates dictionary
            InstantiateItem(newItem);

        // Adds item to purchased list if new
        if (!newItem.isPurchased){
            purchasedItems.Add(newItem);
            newItem.isPurchased = true;
        }

        // Save customization after updating
        SaveCustomization();
    }

    /// <summary>
    /// Instantiates a copy of the item gameobject at the correspondant body transform as a child.
    /// Updates the dictionary with the new item
    /// </summary>
    void InstantiateItem(CustomizableItem item){
        // Gets transform
        Transform bodyPartTransform = GetBodyPartTransform(item.bodyPart);

        // Instantiates a copy of the prefab in that transform as a child of it
        GameObject prefabCopy = Instantiate(item.prefab,bodyPartTransform.position, bodyPartTransform.rotation, bodyPartTransform);
        prefabCopy.transform.localScale = new Vector3(1, 1, 1); // Ensure normal scale
        
        // Defines item reference to the copy
        item.instance = prefabCopy;
        
        // Update dictionary
        currentCustomization[item.bodyPart] = item;
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

    ///////////////////////////////////////////////////////////////////////////////////
    #region SERIALIZATION
    /// <summary>
    /// Save customization to PlayerPrefs as JSON.
    /// </summary>
    public void SaveCustomization()
    {
        List<CustomizationData> dataList = new();

        // Convert dictionary to a serializable list
        foreach (var kvp in currentCustomization)
        {
            if (kvp.Value != null)
            {
                CustomizationData data = new ()
                {
                    bodyPart = kvp.Key,
                    prefabName = kvp.Value.prefab.name
                };
                dataList.Add(data);
            }
        }

        // Serialize the list to JSON
        string json = JsonUtility.ToJson(new CustomizationList(dataList));
        PlayerPrefs.SetString(PLAYER_CUSTOMIZATION_FILE, json);
        Debug.Log("Saved Customization: " + json);
    }

    /// <summary>
    /// Load customization from PlayerPrefs and apply it.
    /// </summary>
    public void LoadCustomization()
    {
        string json = PlayerPrefs.GetString(PLAYER_CUSTOMIZATION_FILE, "");

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogWarning("No saved customization found.");
            return;
        }

        CustomizationList customizationList = JsonUtility.FromJson<CustomizationList>(json);
        
        foreach (var data in customizationList.customizationItems)
        {
            // Use Addressables to load the prefab by name/label
            Addressables.LoadAssetAsync<GameObject>(data.prefabName).Completed += handle => 
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = handle.Result;

                    // Get the CustomizableItem component attached to the prefab
                    CustomizableItem newItem = prefab.GetComponent<CustomizableItem>();
                    
                    // Assign data to the new item
                    newItem.bodyPart = data.bodyPart;
                    newItem.prefab = prefab;

                    // Instantiate the item
                    InstantiateItem(newItem);
                }
                else
                {
                    Debug.LogError("Failed to load prefab: " + data.prefabName);
                }
            };
        }
        Debug.Log("Loaded Customization: " + json);
    }
    #endregion
}

/// <summary>
/// Customization data class for serialization
/// </summary>
[Serializable]
public class CustomizationData {
    public CustomizableCharacter.BodyPart bodyPart;
    public string prefabName; // Label for addressables
}

/// <summary>
/// Wrapper class to hold list of CustomizationData (required for Unity serialization)
/// </summary>
[Serializable]
public class CustomizationList {
    public List<CustomizationData> customizationItems = new ();

    public CustomizationList(List<CustomizationData> customizationItems){
        this.customizationItems = customizationItems;
    }
}