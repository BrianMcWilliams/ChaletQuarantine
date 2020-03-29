using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour
{
    private List<DoorLootInteraction> m_DoorLootInteractions;
    private bool m_GameStartedLocally = false;
    // Start is called before the first frame update
    void Start()
    {
        m_DoorLootInteractions = new List<DoorLootInteraction>();
    }

    public void RegisterInteractable(DoorLootInteraction interaction)
    {
        m_DoorLootInteractions.Add(interaction);

        if (m_GameStartedLocally)
            interaction.InitComponent();
    }

    public void OnGameStartLocally()
    {
        m_GameStartedLocally = true;

        foreach(DoorLootInteraction interaction in m_DoorLootInteractions)
        {
            interaction.InitComponent();
        }
    }
}
