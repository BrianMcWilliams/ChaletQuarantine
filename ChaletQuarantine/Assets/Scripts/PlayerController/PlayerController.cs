using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public Animator m_Animator;
    Camera m_Camera;
    public NavMeshAgent m_Agent;
    public GameObject m_PlayerPrefab;
    public GameObject m_PlayerCameraPrefab;

    GameObject m_PlayerCameraInstance;
    // Update is called once per frame
    private void Start()
    {
        m_Animator.SetBool("Static_b", true);

        m_Animator = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updatePosition = true;
        m_Agent.updateRotation = true;
        m_Agent.updateUpAxis = true;

        if(localPlayerAuthority)
        {
            m_PlayerCameraInstance = Instantiate(m_PlayerCameraPrefab);

            EzCamera ezCamera = m_PlayerCameraInstance.GetComponent<EzCamera>();
            ezCamera.SetCameraTarget(transform);
            m_Camera = m_PlayerCameraInstance.GetComponent<Camera>();
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
