using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanel : Panel
{
    [SerializeField] public TextMeshProUGUI nameText = null;
    [SerializeField] private Button logoutButton = null;
    [SerializeField] private Button leaderboardsButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        logoutButton.onClick.AddListener(SignOut);
        leaderboardsButton.onClick.AddListener(Leaderboards);
        base.Initialize();
    }

    public override void Open()
    {
        UpdatePlayerNameUI();
        base.Open();
    }

    private void SignOut()
    {
        Close();
        GameManager.Instance.SignOut();
    }

    private void UpdatePlayerNameUI()
    {
        nameText.text = AuthenticationService.Instance.PlayerName;
    }

    private void Leaderboards()
    {
        Close();
        PanelManager.Open("TotalTimeLeaderboard");
    }
}
