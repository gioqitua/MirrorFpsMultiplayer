using Mirror;
using UnityEngine;

public class AutoHostClient : NetworkBehaviour
{
    [SerializeField] NetworkManager networkManager;

    private void Start()
    {
        if (!Application.isBatchMode) networkManager.StartClient();
    }
    public void JoinLocal()
    {
        networkManager.networkAddress = "localhost";
        networkManager.StartClient();
    } 
}
