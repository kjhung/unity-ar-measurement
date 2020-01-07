using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasuringLine : MonoBehaviour
{
    private LineRenderer m_LineRenderer;
    private Transform m_ContentText;

    private void Awake()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        m_ContentText = transform.GetChild(0);
    }

    private void Update()
    {
        Vector3 direction = m_LineRenderer.GetPosition(1) - m_LineRenderer.GetPosition(0);
        m_ContentText.LookAt(Camera.main.transform, Vector3.up);

        //direction = new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));
        //Vector3 up = Vector3.Cross(direction, content.forward);
        //content.rotation = Quaternion.LookRotation(content.forward, up);
        //Debug.Log("CROSS: " + up);
        //Debug.Log("RIGHT: " + content.right + ", UP" + content.up + ", FORWARD: " + content.forward);
    }
}
