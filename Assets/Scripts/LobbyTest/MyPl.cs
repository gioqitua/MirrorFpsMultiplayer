using UnityEngine;
using Mirror;
using System;
public class MyPl : NetworkBehaviour
{
    public static MyPl Instance;
    NetworkMatch networkMatch;
    [SyncVar] public int matchID;
    private void Start()
    {
        if (isLocalPlayer)
        {
            Instance = this;
        }

        networkMatch = GetComponent<NetworkMatch>();
    }

    public void HostGame()
    {

        int matchID = Matchmaker.GetRandomMatchID();


        CmdHostGame(matchID);
    }
    [Server]
    private void CmdHostGame(int _matchID)
    {

        matchID = _matchID;
        if (Matchmaker.Instance.HostGame(_matchID, gameObject))
        {
            Debug.Log("Match Starts -- ID : " + _matchID);
            networkMatch.matchId = _matchID.ToGuid();
            TargetHostGame(true, _matchID);
        }
        else
        {
            Debug.Log("Cant Start Match");
            TargetHostGame(false, _matchID);
        }
    }
    [TargetRpc]
    void TargetHostGame(bool success, int _matchID)
    {
        Debug.Log("matchID :" + matchID + " = " + _matchID);
        LobbyUI.Instance.HostSuccess(success);
    }
}
