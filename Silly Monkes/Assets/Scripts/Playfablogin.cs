using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using Photon.Pun;
using Photon.VR;

public class Playfablogin : MonoBehaviourPun
{
    public static Playfablogin instance;

    [Header("COSMETICS")]
    public string MyPlayFabID;
    public string CatalogName;
    public List<GameObject> specialitems;
    public List<GameObject> disableitems;

    [Header("CURRENCY")]
    public string CurrencyName;
    public string CurrenyCode = "AP";
    public float TimeBeforeUpdatingMone = 2f;
    public TextMeshPro currencyText;
    [HideInInspector] public int coins;

    [Header("BAN STUFF")]
    public bool shouldQuitIfBanned = true;
    public GameObject[] StuffToDisable;
    public GameObject[] StuffToEnable;
    public MeshRenderer[] StuffToMaterialChange;
    public Material MaterialToChangeToo;
    public TextMeshPro[] BanTimes;
    public TextMeshPro[] BanReasons;

    [Header("TITLE DATA")]
    public TextMeshPro MOTDText;

    [Header("PLAYER DATA")]
    public TextMeshPro UserName;
    public string StartingUsername;
    public string Name;
    public bool UpdateName;
    public bool IsBanned = false;

    public HashSet<string> ownedItemIds;

    [Header("DON'T DESTROY ON LOAD")]
    public GameObject[] DDOLObjects;

    [Header("Anticheat")]
    public string expectedAppName = "com.COMPANYNAME.GAMENAME";
    public string modFolderPath = "/storage/emulated/0/Android/data/com.YOURCOMPANY.YOURGAMENAME/files/Mods";

    public List<ItemInstance> currentInventory;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        login();
        InvokeRepeating(nameof(Checkmone), TimeBeforeUpdatingMone, 10f);

        // Anticheat - APK integrity check
        if (Application.identifier != expectedAppName)
        {
            Debug.LogWarning("Modded APK detected");
            Application.Quit();
        }

        // Anticheat - Mod folder check
        if (Directory.Exists(modFolderPath))
        {
            Debug.LogWarning("Mod folder detected");
            Application.Quit();
        }
    }

    void Checkmone()
    {
        GetVirtualCurrencies();
    }

    public void login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = GetDeviceId(),
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginError);
    }

    string GetDeviceId()
    {
        return SystemInfo.deviceUniqueIdentifier;
    }

    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("PlayFab login successful");

        MyPlayFabID = result.PlayFabId;
        GetAccountInfo();
        GetVirtualCurrencies();
        GetMOTD();

        StartCoroutine(DDOLStuff());
        StartCoroutine(Inv());
        StartCoroutine(BanPoller());
    }

    void GetAccountInfo()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), AccountInfoSuccess, OnLoginError);
    }

    void AccountInfoSuccess(GetAccountInfoResult result)
    {
        MyPlayFabID = result.AccountInfo.PlayFabId;
        UpdateInventory();
    }

    void UpdateInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            HandleInventoryUpdate(result.Inventory);
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    IEnumerator Inv()
    {
        while (true)
        {
            CheckInventory();
            yield return new WaitForSeconds(10f);
        }
    }

    void CheckInventory()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetInventorySuccess, OnLoginError);
    }

    void OnGetInventorySuccess(GetUserInventoryResult result)
    {
        if (currentInventory == null || IsInventoryChanged(result.Inventory))
        {
            currentInventory = result.Inventory;
            HandleInventoryUpdate(result.Inventory);
        }
    }

    bool IsInventoryChanged(List<ItemInstance> newInventory)
    {
        if (newInventory.Count != currentInventory.Count) return true;

        foreach (var newItem in newInventory)
        {
            bool found = currentInventory.Exists(i => i.ItemInstanceId == newItem.ItemInstanceId);
            if (!found) return true;
        }

        return false;
    }

    void HandleInventoryUpdate(List<ItemInstance> inventory)
    {
        if (inventory == null)
        {
            Debug.LogWarning("Inventory is null — disabling all special items.");
            foreach (var obj in specialitems)
                obj.SetActive(false);
            return;
        }

        ownedItemIds = new HashSet<string>();
        foreach (var item in inventory)
        {
            if (item.CatalogVersion == CatalogName)
                ownedItemIds.Add(item.ItemId);
        }

        foreach (var obj in specialitems)
            obj.SetActive(ownedItemIds.Contains(obj.name));

        foreach (var obj in disableitems)
            obj.SetActive(!ownedItemIds.Contains(obj.name));
    }

    public void GetVirtualCurrencies()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            if (result.VirtualCurrency.TryGetValue(CurrenyCode, out int value))
            {
                coins = value;
                currencyText.text = $"You Have : {coins} {CurrencyName}";
            }
        }, OnLoginError);
    }

    void OnLoginError(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.AccountBanned)
        {
            HandleBan(error);
        }
        else if (error.ErrorMessage.Contains("Could not resolve host") || error.HttpCode == 0)
        {
            Debug.LogError("Could not resolve PlayFab host");
            UnityEngine.Application.Quit();
        }
        else
        {
            Debug.LogWarning($"PlayFab error: {error.GenerateErrorReport()}");

            // --- FAIL CLOSED PATCH ---
            if (error.HttpCode == 0 || error.ErrorMessage.Contains("timeout") || error.ErrorMessage.Contains("Could not resolve host"))
            {
                Debug.LogWarning("Could not access PlayFab API");
                foreach (var obj in specialitems)
                    obj.SetActive(false);
                foreach (var obj in disableitems)
                    obj.SetActive(true);
            }
            // --------------------------

            Invoke(nameof(login), 5f); // retry after delay
        }
    }

    void HandleBan(PlayFabError error)
    {
        PhotonVRManager.Manager.Disconnect();
        IsBanned = true;

        foreach (var obj in StuffToDisable) obj.SetActive(false);
        foreach (var obj in StuffToEnable) obj.SetActive(true);
        foreach (var rend in StuffToMaterialChange) rend.material = MaterialToChangeToo;

        foreach (var item in error.ErrorDetails)
        {
            string banTime = item.Value[0];

            foreach (var t in BanTimes)
            {
                if (banTime == "Indefinite")
                {
                    t.text = "Permanent Ban";
                }
                else if (DateTime.TryParse(banTime, out DateTime parsedTime))
                {
                    TimeSpan remaining = parsedTime.Subtract(DateTime.UtcNow);
                    t.text = $"Hours Left: {(int)remaining.TotalHours}";
                }
                else
                {
                    t.text = "Ban time parsing error";
                }
            }

            foreach (var r in BanReasons)
                r.text = $"Reason: {item.Key}";
        }

        if (shouldQuitIfBanned)
        {
            Application.Quit();
        }
    }

    public void GetMOTD()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), result =>
        {
            if (result.Data != null && result.Data.ContainsKey("MOTD"))
            {
                MOTDText.text = result.Data["MOTD"];
            }
            else
            {
                Debug.Log("No MOTD found");
            }
        }, OnLoginError);
    }

    IEnumerator DDOLStuff()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (var obj in DDOLObjects)
        {
            DontDestroyOnLoad(obj);
        }
    }

    IEnumerator BanPoller()
    {
        while (true)
        {
            GetAccountInfo();
            yield return new WaitForSeconds(15f);
        }
    }
}