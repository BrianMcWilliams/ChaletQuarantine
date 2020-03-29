using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class NetworkPlayerObject : NetworkBehaviour
{
    public GameObject m_PlayerPrefab;
    // Update is called once per frame
    private void Start()
    {
        if (isLocalPlayer == false)
            return;

        CmdServerRequestSpawn(GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_PlayerName);
    }

    GameObject myGameObject;
    [Command]
    void CmdServerRequestSpawn(string playerName)
    {
        myGameObject = Instantiate(m_PlayerPrefab, GameObject.FindGameObjectWithTag("Player_Spawn").transform);
        transform.Find("Player Label").GetComponent<TextMesh>().text = playerName;

        NetworkServer.SpawnWithClientAuthority(myGameObject, connectionToClient);
    }
}
