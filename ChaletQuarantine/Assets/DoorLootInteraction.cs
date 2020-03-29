using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorLootInteraction : MonoBehaviour
{
    private GameObject m_PlayerObject;
    private Canvas m_MyCanvas;
    private Button m_MyButton;
    private bool m_HasBeenInteractedWith = false;
    private bool m_HasBeenInitialised = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Network Manager").GetComponent<InteractableManager>().RegisterInteractable(this);

        m_MyButton = GetComponentInChildren<Button>();
        m_MyButton.interactable = false;
    }

    public void InitComponent()
    {
        m_MyCanvas = GetComponent<Canvas>();
        m_PlayerObject = GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_LocalPlayer;
        m_MyCanvas.worldCamera = GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_PlayerCamera;

        m_MyButton.interactable = false;

        m_HasBeenInitialised = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_HasBeenInitialised || m_HasBeenInteractedWith)
            return;

        float playerDistance = (transform.position - m_PlayerObject.transform.position).magnitude;
        if (!m_MyButton.interactable && playerDistance < 3f)
        {
            m_MyButton.interactable = true;
        }

        if (m_MyButton.interactable && playerDistance > 3f)
        {
            m_MyButton.interactable = false;
        }
    }

    public void OnClickDoor()
    {
        m_HasBeenInteractedWith = true;

        m_MyButton.interactable = false;

        GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_PlayerCash += 100;
        GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_LocalPlayer.GetComponentInChildren<Canvas>().GetComponent<HUD>().OnMoneyChange();
    }
}
