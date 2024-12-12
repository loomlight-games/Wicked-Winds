using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CustomizableCharacter
{
    public int coins;
    public List<Garment> purchasedGarments = new();
    public readonly string PLAYER_CUSTOMIZATION_FILE = "PlayerCustomization";
    public readonly string PLAYER_PURCHASED_ITEMS_FILE = "PlayerPurchasedItems";
    public readonly string PLAYER_COINS_FILE = "PlayerCoins";

    Transform model;

    // Dictionary that maintains the relation between each bodypart and its item with its GO
    public Dictionary<BodyPart, Garment> currentCustomization = new(){
        {BodyPart.Head, null},
        {BodyPart.UpperBody, null},
        {BodyPart.LowerBody, null},
        {BodyPart.Broom, null},
    };

    /////////////////////////////////////////////////////////////////////////////////////////////
    public void Awake()
    {
        model = PlayerManager.Instance.model;

        // Load customization,  purchased items and coins
        //Load();
    }

    /// <summary>
    /// Change body part according to item received, creationg or destroying it
    /// </summary>
    public void UpdateBodyPart(Garment newGarment)
    {
        Garment currentGarment = currentCustomization[newGarment.bodyPart];

        // Current item not null
        if (currentGarment != null)
        {
            // Is the same one wearing
            if (currentGarment == newGarment)
                UnwearGarment(newGarment);
            // Is different
            else
                WearGarment(newGarment);
        }
        else // Is null
            WearGarment(newGarment);

        // New item
        if (!newGarment.isPurchased)
        {
            // Add to list
            purchasedGarments.Add(newGarment);
            newGarment.isPurchased = true;
        }

        // Save customization and purchased items after updating
        Save();
    }

    private void WearGarment(Garment newGarment)
    {
        switch (newGarment.bodyPart)
        {
            case BodyPart.Head:
                ActivateGarment(PlayerManager.Instance.headGarments, newGarment);
                break;
            case BodyPart.UpperBody:
                ActivateGarment(PlayerManager.Instance.upperBodyGarments, newGarment);
                break;
            case BodyPart.LowerBody:
                ActivateGarment(PlayerManager.Instance.lowerBodyGarments, newGarment);
                break;
            case BodyPart.Broom:
                ActivateGarment(PlayerManager.Instance.brooms, newGarment);
                break;
            default:
                break;
        }
    }

    private void UnwearGarment(Garment newGarment)
    {
        switch (newGarment.bodyPart)
        {
            case BodyPart.Head:
                DeactivateGarment(PlayerManager.Instance.headGarments, newGarment);
                break;
            case BodyPart.UpperBody:
                DeactivateGarment(PlayerManager.Instance.upperBodyGarments, newGarment);
                break;
            case BodyPart.LowerBody:
                DeactivateGarment(PlayerManager.Instance.lowerBodyGarments, newGarment);
                break;
            case BodyPart.Broom:
                DeactivateGarment(PlayerManager.Instance.brooms, newGarment);
                break;
            default:
                break;
        }
    }

    void ActivateGarment(List<GameObject> garments, Garment newGarment)
    {
        foreach (var garment in garments)
        {
            bool isActive = garment.tag == newGarment.tag;
            garment.SetActive(isActive);

            if (isActive)
                // Update the current customization dictionary
                currentCustomization[newGarment.bodyPart] = newGarment;
        }
    }

    void DeactivateGarment(List<GameObject> garments, Garment newGarment)
    {
        foreach (var garment in garments)
        {
            if (garment.tag == newGarment.tag)
            {
                garment.SetActive(false);

                // Update the current customization dictionary
                currentCustomization[newGarment.bodyPart] = null;

                if (garments == PlayerManager.Instance.brooms)
                    garments[4].SetActive(true);
            }
        }
    }

    internal void UpdateCoins(int coins)
    {
        this.coins = coins;
        SaveCoins();
    }

    ///////////////////////////////////////////////////////////////////////////////////
    #region SERIALIZATION
    public void Save()
    {
        SavePurchasedItems();
        SaveCustomization();
    }

    public void Load()
    {
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
        List<GarmentData> dataList = new();

        foreach (var item in purchasedGarments)
        {
            GarmentData data = new()
            {
                bodyPart = item.bodyPart,
                //prefabName = item.prefab.name,
                isPurchased = item.isPurchased,
            };

            dataList.Add(data);
        }

        // Serialize the list to JSON
        string json = JsonUtility.ToJson(new PurchasedGarmentsList(dataList));
        PlayerPrefs.SetString(PLAYER_PURCHASED_ITEMS_FILE, json);
        Debug.Log("Saved purchased items: " + json);
    }


    /// <summary>
    /// Save customization to PlayerPrefs as JSON.
    /// </summary>
    public void SaveCustomization()
    {
        List<GarmentData> dataList = new();

        // Convert dictionary to a serializable list
        foreach (var kvp in currentCustomization)
        {
            if (kvp.Value != null)
            {
                GarmentData data = new()
                {
                    bodyPart = kvp.Key,
                    //prefabName = kvp.Value.prefab.name,
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
    /// Load coins amount from PlayerPrefs.
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

        PurchasedGarmentsList loadedPurchasedItemsList = JsonUtility.FromJson<PurchasedGarmentsList>(json);

        // Mark each item prefab as purchased
        foreach (var item in loadedPurchasedItemsList.purchasedGarments)
        {
            // Use Addressables to load the prefab by name/label
            Addressables.LoadAssetAsync<GameObject>(item.tag).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = handle.Result;

                    // Get the CustomizableItem component attached to the prefab
                    Garment loadedItem = prefab.GetComponent<Garment>();

                    purchasedGarments.Add(loadedItem);
                }
                else
                {
                    Debug.LogError("Failed to load prefab: " + item.tag);
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

        foreach (var data in customizationList.customizationGarments)
        {
            // Use Addressables to load the prefab by name/label
            Addressables.LoadAssetAsync<GameObject>(data.tag).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = handle.Result;

                    // Get the component attached to the prefab
                    Garment loadedGarment = prefab.GetComponent<Garment>();

                    // Assign data to the new item
                    loadedGarment.bodyPart = data.bodyPart;
                    //newItem.prefab = prefab;
                    loadedGarment.isPurchased = data.isPurchased;

                    WearGarment(loadedGarment);
                }
                else
                {
                    Debug.LogError("Failed to load prefab: " + data.tag);
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
public class GarmentData
{
    public BodyPart bodyPart;
    public string tag;
    public bool isPurchased;
}

/// <summary>
/// Wrapper class to hold list of CustomizationData (required for Unity serialization)
/// </summary>
[Serializable]
public class CustomizationList
{
    public List<GarmentData> customizationGarments = new();

    public CustomizationList(List<GarmentData> customizationGarments)
    {
        this.customizationGarments = customizationGarments;
    }
}

/// <summary>
/// Wrapper class to hold list of CustomizationData (required for Unity serialization)
/// </summary>
[Serializable]
public class PurchasedGarmentsList
{
    public List<GarmentData> purchasedGarments = new();

    public PurchasedGarmentsList(List<GarmentData> purchasedGarments)
    {
        this.purchasedGarments = purchasedGarments;
    }
}