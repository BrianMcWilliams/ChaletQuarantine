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
        if (hasAuthority == false)
            return;

        CmdServerRequestSpawn();
    }

    GameObject myGameObject;
    [Command]
    void CmdServerRequestSpawn()
    {
        myGameObject = Instantiate(m_PlayerPrefab);

        NetworkServer.SpawnWithClientAuthority(myGameObject, connectionToClient);
    }
}
