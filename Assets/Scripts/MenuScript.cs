using Mirror;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject gamePanel;
    bool paused;
     
    private void Start()
    {
        paused = false;
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }
        if (NetworkServer.active || NetworkClient.active)
        {
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
            if (paused)
            {
                gamePanel.SetActive(true);
            }
            else
            {
                gamePanel.SetActive(false);
            }
        }
        else
        {
            menuPanel.SetActive(true);
            gamePanel.SetActive(false);
        }
    }

    public void StartHost()
    {
        networkManager.StartHost();
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    public void StartClient()
    {
        networkManager.StartClient();
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    public void SetIp(string ip)
    {
        networkManager.networkAddress = ip;
    }
    public void StopGame()
    {
        if (networkManager.mode == NetworkManagerMode.Host)
        {
           networkManager.StopHost();
        }
        if (networkManager.mode == NetworkManagerMode.ClientOnly)
        {
             networkManager.StopClient();
        }
        paused = false;
    }
}
