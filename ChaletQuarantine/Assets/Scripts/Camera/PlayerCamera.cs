using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform m_InitialTransform;
    public GameObject m_PlayerObject;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void ResetCamera()
    {
        //Set Camera position;
        Transform playerPosition = m_PlayerObject.transform;
        transform.position = playerPosition.position + m_InitialTransform.localPosition;
        transform.rotation = m_InitialTransform.localRotation;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 IdealPosition = m_PlayerObject.transform.position + m_InitialTransform.localPosition;
        IdealPosition = Quaternion.AngleAxis(-45, Vector3.up) * IdealPosition;

        if ((transform.position - IdealPosition).magnitude > 2f)
        {
            transform.position = Vector3.Lerp(transform.position, IdealPosition, 2f * Time.deltaTime);
        }
    }
}
