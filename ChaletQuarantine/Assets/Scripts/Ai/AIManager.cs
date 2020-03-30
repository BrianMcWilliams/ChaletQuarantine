using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AIManager : NetworkBehaviour
{
    public float m_DeltaTime = 10;
    public float m_LastUpdatedTime;
    public GameObject m_ZombiePrefab;
    // Start is called before the first frame update
    void Start()
    {
        m_LastUpdatedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
            return;

        if((Time.time - m_LastUpdatedTime) > m_DeltaTime)
        {
            m_LastUpdatedTime = Time.time;

            CmdSpawnZombie();
        }
    }

    [Command]
    void CmdSpawnZombie()
    {
        GameObject spawnZombie = Instantiate(m_ZombiePrefab, GameObject.FindGameObjectWithTag("Zombie_Spawn").transform);

        NetworkServer.Spawn(spawnZombie);
    }
}
