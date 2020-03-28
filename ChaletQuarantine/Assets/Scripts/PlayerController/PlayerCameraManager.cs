using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    public const float Y_ANGLE_MIN = 0f;
    public const float Y_ANGLE_MAX = 50f;

    public GameObject m_LookAtTarget;
    public Transform m_CameraTransform;

    private Camera m_Camera;

    private float m_Distance = 16f;
    private float m_CurrentX = 0f;
    private float m_CurrentY = 0f;
    private float m_SensitivityX = 4f;
    private float m_SensitivityY = 1f;
    // Start is called before the first frame update
    void Start()
    {
        m_CameraTransform = transform;
        m_Camera = Camera.main;

        m_CurrentY = Y_ANGLE_MAX;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            m_CurrentX += Input.GetAxis("Mouse X");
            m_CurrentY -= Input.GetAxis("Mouse Y");

            m_CurrentY = Mathf.Clamp(m_CurrentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }
    }

    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -m_Distance);

        Vector3 follow = Vector3.Slerp(transform.forward, m_LookAtTarget.transform.forward, Time.deltaTime);
        //Quaternion rotation = Quaternion.Euler(m_CurrentY, m_CurrentX, 0f);
        Quaternion rotation = Quaternion.Euler(m_CurrentY, m_CurrentX, 0f);

        m_CameraTransform.position = m_LookAtTarget.transform.position + follow + rotation * dir;
        m_CameraTransform.LookAt(m_LookAtTarget.transform.position);
    }
}
