using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceARFocus : MonoBehaviour
{
    [SerializeField]
    private GameObject m_FocusPrefab;
    public GameObject focusPrefab
    {
        get { return m_FocusPrefab; }
        set { m_FocusPrefab = value; }
    }

    public GameObject spawnedFocus { get; private set; }

    private ARPlaneManager m_planeManager;
    private ARRaycastManager m_RaycastManager;

    public static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private Vector2 m_Center;

    private void Awake()
    {
        m_planeManager = GetComponent<ARPlaneManager>();
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_Center = new Vector2(Screen.width / 2f, Screen.height / 2f);
    }

    private void Update()
    {
        if (m_RaycastManager.Raycast(m_Center, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = s_Hits[0].pose;
            if (spawnedFocus == null)
            {
                spawnedFocus = Instantiate(m_FocusPrefab, hitPose.position, hitPose.rotation);
                //if (onPlacedFocus != null)
                //{
                //    onPlacedFocus();
                //}
            }

            spawnedFocus.transform.localPosition = hitPose.position;

            ARPlane plane = m_planeManager.GetPlane(s_Hits[0].trackableId);
            if (plane.alignment == PlaneAlignment.Vertical)
            {
                Vector3 eulerAngles = new Vector3(90f, plane.transform.localEulerAngles.y, 0f);
                spawnedFocus.transform.GetChild(0).rotation = Quaternion.Euler(eulerAngles);
            }
            else
            {
                spawnedFocus.transform.GetChild(0).rotation = Quaternion.Euler(Vector3.zero);
            }

        }
    }
}
