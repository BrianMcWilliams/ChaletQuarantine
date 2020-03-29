using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public Canvas m_Canvas;
    public void OnClickQuit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OnClickBuy()
    {
        if (GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_PlayerCash >= 75)
        {
            GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_PlayerCash -= 75;
            OnMoneyChange();

            GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_LocalPlayer.GetComponent<PlayerController>().SpawnTurret();
        }
    }

    public void OnMoneyChange()
    {
        TextMesh cashCount = GameObject.FindGameObjectWithTag("UI_CashCount").GetComponent<TextMesh>();

        cashCount.text = $"Cash ${GameObject.FindGameObjectWithTag("Network Manager").GetComponent<PlayerInfo>().m_PlayerCash}"; 
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Canvas = GetComponent<Canvas>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
