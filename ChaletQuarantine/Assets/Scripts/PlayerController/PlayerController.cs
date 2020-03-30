using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    public Animator m_Animator;
    Camera m_Camera;
    public NavMeshAgent m_Agent;
    public GameObject m_PlayerPrefab;
    public GameObject m_PlayerCameraPrefab;
    public GameObject m_TurretPrefab;
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

        GameObject.FindGameObjectWithTag("Network Manager").GetComponent<NetworkPlayerManager>().AddPlayer(gameObject);

        if(isLocalPlayer || hasAuthority)
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

            GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_LocalPlayer = gameObject;
            GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_PlayerCamera = m_Camera;
            TextMesh[] meshes = GetComponentsInChildren<TextMesh>();
            foreach (TextMesh playerLabel in meshes)
                playerLabel.text = GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_PlayerName;

            GameObject.FindGameObjectWithTag("Network Manager").GetComponent<InteractableManager>().OnGameStartLocally();
            GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_LocalPlayer.GetComponentInChildren<Canvas>().GetComponent<HUD>().OnMoneyChange();
        }
    }
    void Update()
    {
        if (!m_Animator || !m_Camera || !m_Agent)
            return; //Waiting for init

        m_Animator.SetFloat("Speed_f", m_Agent.velocity.magnitude);

        if (hasAuthority == false)
            return;

        if (Input.GetMouseButton(0))
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

    public void SpawnTurret()
    {
        if(isLocalPlayer || hasAuthority)
        {
            CmdSpawnTurret();
        }

    }

    [Command] 
    void CmdSpawnTurret()
    {
        GameObject turret = Instantiate(m_TurretPrefab, transform.position + Vector3.up, transform.rotation);

        NetworkServer.Spawn(turret, connectionToClient);
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
