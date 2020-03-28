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
        Cursor.lockState = CursorLockMode.Confined;

        m_Animator.SetBool("Static_b", true);

        m_Animator = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_Agent.updatePosition = true;
        m_Agent.updateRotation = true;
        m_Agent.updateUpAxis = true;

#if !UNITY_EDITOR
        if(isLocalPlayer || hasAuthority)
#endif
        {
            m_PlayerCameraInstance = Instantiate(m_PlayerCameraPrefab);
            PlayerCameraManager playerCameraManager = m_PlayerCameraInstance.GetComponent<PlayerCameraManager>();
            m_PlayerCameraInstance.transform.position = playerCameraManager.m_CameraTransform.position + transform.position;
            playerCameraManager.m_LookAtTarget = gameObject;
            m_Camera = m_PlayerCameraInstance.GetComponent<Camera>();

            Canvas myCanvas = GetComponentInChildren<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            myCanvas.worldCamera = m_Camera;
            myCanvas.planeDistance = 1;
            myCanvas.gameObject.SetActive(true);

            TextMesh playerLabel = GetComponentInChildren<TextMesh>();
            playerLabel.text = GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_PlayerName;
        }
    }
    void Update()
    {
        if (!m_Animator || !m_Camera || !m_Agent)
            return; //Waiting for init

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

        /*
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Canvas myCanvas = GetComponentInChildren<Canvas>();
            myCanvas.gameObject.SetActive(myCanvas.gameObject.activeSelf);
        }
        */
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
