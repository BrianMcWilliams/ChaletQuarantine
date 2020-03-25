using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public Animator m_Animator;
    public Camera m_Camera;
    public NavMeshAgent m_Agent;
    public GameObject m_PlayerPrefab;

    // Update is called once per frame
    private void Start()
    {
        m_Animator.SetBool("Static_b", true);

        m_Camera.GetComponentInChildren<Camera>();
        m_Animator = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updatePosition = true;
        m_Agent.updateRotation = true;
        m_Agent.updateUpAxis = true;

        if(isLocalPlayer == false)
        { 
            m_Camera.gameObject.active = false; 
        }
    }
    void Update()
    {
        m_Animator.SetFloat("Speed_f", m_Agent.velocity.magnitude);

        if (hasAuthority == false)
            return;

        if(Input.GetMouseButton(0))
        {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                m_Agent.SetDestination(hit.point);
                CmdRequestMove(hit.point);
            }
        }
    }

    [Command]
    void CmdRequestMove(Vector3 destination)
    {
        m_Agent.SetDestination(destination);
        
        RpcUpdateDestination(destination);
    }

    [ClientRpc]
    void RpcUpdateDestination(Vector3 destination)
    {
        if (hasAuthority)
            return;

        m_Agent.SetDestination(destination);
    }
}
