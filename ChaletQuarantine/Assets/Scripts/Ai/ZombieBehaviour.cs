using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;
public class ZombieBehaviour : NetworkBehaviour
{
    public NavMeshAgent m_Agent;
    public Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator.SetBool("Static_b", true);

        m_Animator = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updatePosition = true;
        m_Agent.updateRotation = true;
        m_Agent.updateUpAxis = true;

        NetworkServer.Spawn(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length == 0)
            return;

        Vector3 currentPosition = transform.position;
        //Find closest player
        GameObject currentTarget = players[0];
        float currentDistance = (currentTarget.transform.position - currentPosition).magnitude;
        foreach(GameObject player in players)
        {
            float calculatedDistance = (player.transform.position - currentPosition).magnitude;

            if(calculatedDistance < currentDistance)
            {
                currentTarget = player;
                currentDistance = calculatedDistance;
            }
        }

        if( (m_Agent.destination - currentTarget.transform.position).magnitude > 2f )
        {
            m_Agent.SetDestination(currentTarget.transform.position);
        }
    }
}
