using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Leaderboards;
using System;

public class TotalTimeLeaderboard : Panel 
{
    string LeaderboardID = "ElapsedTime";
    [SerializeField] private int playersPerPage = 8;
    [SerializeField] private LeaderboardsPlayerItem playerItemPrefab = null;

    //for asigning the playersItems prefabs (contents's children)
    [SerializeField] private RectTransform playersContainer = null; 
    [SerializeField] public TextMeshProUGUI pageText = null;
    [SerializeField] private Button nextButton = null;
    [SerializeField] private Button prevButton = null;
    [SerializeField] private Button ProfileButton = null;
    [SerializeField] private Button AverageButton = null;
    [SerializeField] private Button misionCountButton = null;   


    private int currentPage = 1;
    private int totalPages = 0;


    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ClearPlayersList();
        ProfileButton.onClick.AddListener(ProfilePanel);
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);
        AverageButton.onClick.AddListener(OpenAveragePanel);
        misionCountButton.onClick.AddListener(OpenMisionCountPanel);
        base.Initialize();
    }
    /// <summary>
    /// reset the leaderboard
    /// </summary>
    public override void Open()
    {
        pageText.text = "-";
        nextButton.interactable = false;
        prevButton.interactable = false;
        base.Open();
        ClearPlayersList();
        currentPage = 1;
        totalPages = 0;
        LoadPlayers(1);
        //LoadPlayersWithoutSignIn(1);
        AddGameScore();

    }

    //add the player score from the gameplay scene
    private void AddGameScore()
    {
        AddScoreAsync(PlayerPrefs.GetInt(GameManager.Instance.PLAYER_SCORE_FILE, 0));
    }
    private void AddScore()
    {
        AddScoreAsync(PlayerPrefs.GetInt(GameManager.Instance.PLAYER_SCORE_FILE,0));
    }
    public async void AddScoreAsync(int score)
    {

        try
        {
            //
            var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardID, score);
            LoadPlayers(currentPage);
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }

    }

    private async void LoadPlayers(int page)
    {
        nextButton.interactable = false;
        prevButton.interactable = false;
        try
        {
            //splitting players in different pages
            GetScoresOptions options = new GetScoresOptions();
            options.Offset = (page - 1) * playersPerPage; //amount of records ignores
            options.Limit = playersPerPage; //limit of records it loads

            var scores = await LeaderboardsService.Instance.GetScoresAsync(LeaderboardID, options);
            ClearPlayersList();

            //for every score it creates an instance of leaderboardsPlayersItem
            for (int i = 0; i < scores.Results.Count; i++)
            {
                LeaderboardsPlayerItem item = Instantiate(playerItemPrefab, playersContainer);
                item.Initialize(scores.Results[i]);
            }

            totalPages = Mathf.CeilToInt((float)scores.Total / (float)scores.Limit);
            currentPage = page;
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
        pageText.text = currentPage.ToString() + "/" + totalPages.ToString();
        nextButton.interactable = currentPage < totalPages && totalPages > 1;
        prevButton.interactable = currentPage > 1 && totalPages > 1;
    }
    private async void LoadPlayersWithoutSignIn(int page)
    {
        nextButton.interactable = false;
        prevButton.interactable = false;
        try
        {
            //splitting players in different pages
            GetScoresOptions options = new GetScoresOptions();
            options.Offset = (page - 1) * playersPerPage; //amount of records ignores
            options.Limit = playersPerPage; //limit of records it loads

            var scores = await LeaderboardsService.Instance.GetVersionScoresAsync(LeaderboardID, "ElapsedTime");
            ClearPlayersList();

            //for every score it creates an instance of leaderboardsPlayersItem
            for (int i = 0; i < scores.Results.Count; i++)
            {
                LeaderboardsPlayerItem item = Instantiate(playerItemPrefab, playersContainer);
                item.Initialize(scores.Results[i]);
            }

            totalPages = Mathf.CeilToInt((float)scores.Total / (float)scores.Limit);
            currentPage = page;
        }
        catch (Exception exception)
        {
            Debug.Log(exception.Message);
        }
        pageText.text = currentPage.ToString() + "/" + totalPages.ToString();
        nextButton.interactable = currentPage < totalPages && totalPages > 1;
        prevButton.interactable = currentPage > 1 && totalPages > 1;
    }

    //PANEL MANAGEMENT
    private void ProfilePanel()
    {
        PanelManager.Open("profile");
    }

    private void OpenAveragePanel()
    {
        Close();
        PanelManager.Open("AverageLeaderboard");
    }
    private void OpenMisionCountPanel()
    {
        Close();
        PanelManager.Open("MisionCountLeaderboard");

    }

    /// <summary>
    /// clear the list of players
    /// </summary>
    private void ClearPlayersList()
    {
        LeaderboardsPlayerItem[] items = playersContainer.GetComponentsInChildren<LeaderboardsPlayerItem>();
        if (items != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Destroy(items[i].gameObject);
            }
        }
    }


    /// <summary>
    /// control the leaderboard pages
    /// </summary>
    private void NextPage()
    {
        if (currentPage + 1 > totalPages)
        {
            LoadPlayers(1);
        }
        else
        {
            LoadPlayers(currentPage + 1);
        }
    }

    private void PrevPage()
    {
        if (currentPage - 1 <= 0)
        {
            LoadPlayers(totalPages);
        }
        else
        {
            LoadPlayers(currentPage - 1);
        }
    }
}
