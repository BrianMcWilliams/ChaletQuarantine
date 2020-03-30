using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkPlayerObject : NetworkBehaviour
{
    public GameObject m_PlayerPrefab;
    [SyncVar]
    public string m_PlayerName;
    private void Start()
    {
        CmdServerRequestSpawn();
        GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_LocalPlayer = gameObject;
    }

    public void SetPlayerName(string newName)
    {
        m_PlayerName = newName;
    }
    GameObject myGameObject;
    [Command]
    void CmdServerRequestSpawn()
    {
        myGameObject = Instantiate(m_PlayerPrefab, GameObject.FindGameObjectWithTag("Player_Spawn").transform);

        NetworkServer.Spawn(myGameObject, connectionToClient);
    }
}
