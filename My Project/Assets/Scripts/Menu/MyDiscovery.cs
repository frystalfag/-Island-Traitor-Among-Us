using Mirror;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MyDiscovery : MonoBehaviour
{
    [SerializeField] private Transform listContainer;
    [SerializeField] private GameObject serverButtonPrefab;
    [SerializeField] private Button refreshButton;
    
    private List<GameObject> serverButtons = new List<GameObject>();
    
    private void Start()
    {
        if (refreshButton != null)
        {
            refreshButton.onClick.AddListener(RefreshServerList);
        }
    }
    
    public void RefreshServerList()
    {
        ClearServerList();
        CreateServerButton("127.0.0.1:7777", "Локальный сервер");
        Debug.Log("Поиск серверов...");
    }
    
    private void CreateServerButton(string address, string serverName)
    {
        if (serverButtonPrefab == null || listContainer == null) return;
        
        GameObject button = Instantiate(serverButtonPrefab, listContainer);
        
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = $"{serverName}\n{address}";
        }
        
        Button buttonComponent = button.GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(() => {
                ConnectToServer(address);
            });
        }
        
        serverButtons.Add(button);
    }
    
    private void ConnectToServer(string address)
    {
        if (NetworkManager.singleton != null)
        {
            string[] parts = address.Split(':');
            if (parts.Length == 2)
            {
                NetworkManager.singleton.networkAddress = parts[0];
                if (ushort.TryParse(parts[1], out ushort port))
                {
                    ((kcp2k.KcpTransport)NetworkManager.singleton.transport).Port = port;
                }
            }
            else
            {
                NetworkManager.singleton.networkAddress = address;
            }
            
            NetworkManager.singleton.StartClient();
            Debug.Log($"Подключение к серверу: {address}");
        }
    }
    
    private void ClearServerList()
    {
        foreach (GameObject button in serverButtons)
        {
            if (button != null)
            {
                DestroyImmediate(button);
            }
        }
        serverButtons.Clear();
    }
}