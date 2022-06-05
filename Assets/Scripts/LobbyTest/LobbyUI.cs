using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance;
    [SerializeField] TMP_InputField joinMatchID;
    [SerializeField] Button joinButton;
    [SerializeField] Button hostButton;
    [SerializeField] Canvas lobbyCanvas;
    [SerializeField] GameObject menuButtons;

    private void Start()
    {
        Instance = this;
    }
    public void Host()
    {
        SwitchButtons(false);
        MyPl.Instance.HostGame();

    }
    public void HostSuccess(bool success)
    {
        if (success)
        {
            lobbyCanvas.gameObject.SetActive(true);
            menuButtons.gameObject.SetActive(false);

        }
        else
        {
            SwitchButtons(!success);
        }
    }
    public void JoinSuccess(bool success)
    {
        if (success)
        {

        }
        else
        {
            SwitchButtons(!success);
        }
    }
    public void Join()
    {
        SwitchButtons(false);
    }
    private void SwitchButtons(bool condition)
    {
        joinMatchID.interactable = condition;
        joinButton.interactable = condition;
        hostButton.interactable = condition;
    }




}
