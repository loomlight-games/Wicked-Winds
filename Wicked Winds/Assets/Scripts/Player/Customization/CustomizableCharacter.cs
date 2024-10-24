using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CustomizableCharacter : MonoBehaviour {
    const string PLAYER_CUSTOMIZATION_FILE = "PlayerCustomization";
    const string PLAYER_PURCHASED_ITEMS_FILE = "PlayerPurchasedItems";
    const string PLAYER_COINS_FILE = "PlayerCoins";

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

    public int coins;

    /////////////////////////////////////////////////////////////////////////////////////////////
    void Awake()
    {
        // Load customization and purchased items
        Load();
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

        // Save customization and purchased items after updating
        Save();
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

    internal void UpdateCoins(int coins)
    {
        this.coins = coins;
        SaveCoins();
    }

    ///////////////////////////////////////////////////////////////////////////////////
    #region SERIALIZATION
    public void Save(){
        SavePurchasedItems();
        SaveCustomization();
    }

    public void Load(){
        LoadPurchasedItems();
        LoadCustomization();
        LoadCoins();
    }
    
    /// <summary>
    /// Saves coins to PlayerPrefs as JSON
    /// </summary>
    public void SaveCoins()
    {
        PlayerPrefs.SetInt(PLAYER_COINS_FILE, coins);
        Debug.Log("Saved coins: " + coins);
    }

    /// <summary>
    /// Saves purchased items to PlayerPrefs as JSON
    /// </summary>
    public void SavePurchasedItems()
    {
        List<ItemData> dataList = new();

        foreach (var item in purchasedItems)
        {
            ItemData data = new ()
            {
                bodyPart = item.bodyPart,
                prefabName = item.prefab.name,
                isPurchased = item.isPurchased,
            };

            dataList.Add(data);
        }

        // Serialize the list to JSON
        string json = JsonUtility.ToJson(new PurchasedItemsList(dataList));
        PlayerPrefs.SetString(PLAYER_PURCHASED_ITEMS_FILE, json);
        Debug.Log("Saved purchased items: " + json);
    }


    /// <summary>
    /// Save customization to PlayerPrefs as JSON.
    /// </summary>
    public void SaveCustomization()
    {
        List<ItemData> dataList = new();

        // Convert dictionary to a serializable list
        foreach (var kvp in currentCustomization)
        {
            if (kvp.Value != null)
            {
                ItemData data = new ()
                {
                    bodyPart = kvp.Key,
                    prefabName = kvp.Value.prefab.name,
                    isPurchased = kvp.Value.isPurchased,
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
    /// Load purchased items from PlayerPrefs.
    /// </summary>
    public void LoadCoins()
    {
        if (!PlayerPrefs.HasKey(PLAYER_COINS_FILE))
        {
            Debug.LogWarning("No coins found.");
            return;
        }

        coins = PlayerPrefs.GetInt(PLAYER_COINS_FILE, 0); // Default to 0 if no data is found
        Debug.Log("Loaded coins: " + coins);
    }

    /// <summary>
    /// Load purchased items from PlayerPrefs.
    /// </summary>
    public void LoadPurchasedItems()
    {
        string json = PlayerPrefs.GetString(PLAYER_PURCHASED_ITEMS_FILE, "");

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogWarning("No purchased items found.");
            return;
        }

        Debug.Log("Loaded purchased items: " + json);
        
        PurchasedItemsList loadedPurchasedItemsList = JsonUtility.FromJson<PurchasedItemsList>(json);
        
        // Mark each item prefab as purchased
        foreach (var item in loadedPurchasedItemsList.purchasedItems)
        {
            // Use Addressables to load the prefab by name/label
            Addressables.LoadAssetAsync<GameObject>(item.prefabName).Completed += handle => 
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = handle.Result;

                    // Get the CustomizableItem component attached to the prefab
                    CustomizableItem loadedItem = prefab.GetComponent<CustomizableItem>();
                    
                    purchasedItems.Add(loadedItem);
                }
                else
                {
                    Debug.LogError("Failed to load prefab: " + item.prefabName);
                }
            };
        }
        
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
                    newItem.isPurchased = data.isPurchased;

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
public class ItemData {
    public CustomizableCharacter.BodyPart bodyPart;
    public string prefabName; // Label for addressables
    public bool isPurchased;
}

/// <summary>
/// Wrapper class to hold list of CustomizationData (required for Unity serialization)
/// </summary>
[Serializable]
public class CustomizationList {
    public List<ItemData> customizationItems = new ();

    public CustomizationList(List<ItemData> customizationItems){
        this.customizationItems = customizationItems;
    }
}

/// <summary>
/// Wrapper class to hold list of CustomizationData (required for Unity serialization)
/// </summary>
[Serializable]
public class PurchasedItemsList {
    public List<ItemData> purchasedItems = new ();

    public PurchasedItemsList(List<ItemData> purchasedItems){
        this.purchasedItems = purchasedItems;
    }
}