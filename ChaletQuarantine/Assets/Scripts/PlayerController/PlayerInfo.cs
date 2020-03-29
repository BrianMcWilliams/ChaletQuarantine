using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfo : NetworkBehaviour
{
    public string m_PlayerName = "";
    public GameObject m_LocalPlayer;
    public Camera m_PlayerCamera;
    public int m_PlayerCash = 250;
    public void SetPlayerName(string newName)
    {
        m_PlayerName = newName;
    }

    public void SetPlayerCamera(Camera camera)
    {
        m_PlayerCamera = camera;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
