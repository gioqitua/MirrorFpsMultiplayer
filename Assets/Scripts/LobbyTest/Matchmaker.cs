using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Security.Cryptography;
using System.Text;

public class Matchmaker : NetworkBehaviour
{
    public static Matchmaker Instance;
    readonly public SyncListMatch matches = new SyncListMatch();
    readonly public SyncList<int> matchIDs = new SyncList<int>();
    private void Start()
    {
        Instance = this;
    }
    public bool HostGame(int matchID, GameObject player)
    {
        if (!matchIDs.Contains(matchID))
        {
            matchIDs.Add(matchID);

            matches.Add(new Match(matchID, player));

            Debug.Log("Match Generated");

            return true;
        }
        else
        {
            Debug.Log("Match ID Already Exist");
            return false;
        }
    }
    public static int GetRandomMatchID()
    {
        return UnityEngine.Random.Range(10000, 99999);
    }
}
public static class MatchExtensions
{
    public static Guid ToGuid(this int id)
    {
        var newID = id.ToString();
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = System.Text.Encoding.Default.GetBytes(newID);
        byte[] hashBytes = provider.ComputeHash(inputBytes);
        return new Guid(hashBytes);
    }
}

public class Match : NetworkBehaviour
{
    public int matchID;
    readonly public SyncListGameObject players = new SyncListGameObject();
    public Match(int _matchID, GameObject _player)
    {
        matchID = _matchID;
        players.Add(_player);
    }
    public Match()
    {

    }
}


public class SyncListGameObject : SyncList<UnityEngine.GameObject> { }


public class SyncListMatch : SyncList<Match> { }
