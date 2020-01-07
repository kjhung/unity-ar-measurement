using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    ARCameraManager m_CameraManager;
    public ARCameraManager cameraManager
    {
        get { return m_CameraManager; }
        set
        {
            if (m_CameraManager == value)
                return;

            if (m_CameraManager != null)
                m_CameraManager.frameReceived -= FrameChanged;

            m_CameraManager = value;

            if (m_CameraManager != null & enabled)
                m_CameraManager.frameReceived += FrameChanged;
        }
    }

    [SerializeField]
    ARPlaneManager m_PlaneManager;
    public ARPlaneManager planeManager { get { return m_PlaneManager; } set { m_PlaneManager = value; } }

    [SerializeField]
    Measurement m_Measurement;
    public Measurement measurement { get { return m_Measurement; } set { m_Measurement = value; } }

    [SerializeField]
    CanvasGroup m_MoveDeviceCanvasGroup;
    public CanvasGroup moveDeviceCanvasGroup { get { return m_MoveDeviceCanvasGroup; } set { m_MoveDeviceCanvasGroup = value; } }

    [SerializeField]
    CanvasGroup m_PlacePointCanvasGroup;
    public CanvasGroup placePointCanvasGroup { get { return m_PlacePointCanvasGroup; } set { m_PlacePointCanvasGroup = value; } }

    private bool m_ShowingMoveDevice = true;
    private bool m_ShowingPlacePoint = false;

    private void OnEnable()
    {
        if (m_CameraManager != null)
            m_CameraManager.frameReceived += FrameChanged;
        measurement.onPlacedObject += PlacePoint;
    }

    private void OnDisable()
    {
        if (m_CameraManager != null)
            m_CameraManager.frameReceived -= FrameChanged;
        measurement.onPlacedObject -= PlacePoint;
    }

    private void Awake()
    {
        moveDeviceCanvasGroup.alpha = 1f;
        placePointCanvasGroup.alpha = 0f;
    }

    private void FrameChanged(ARCameraFrameEventArgs args)
    {
        if (PlanesFound() && m_ShowingMoveDevice)
        {
            if (moveDeviceCanvasGroup)
                moveDeviceCanvasGroup.alpha = 0f;

            if (placePointCanvasGroup)
                placePointCanvasGroup.alpha = 1f;

            m_ShowingMoveDevice = false;
            m_ShowingPlacePoint = true;
        }
    }

    private bool PlanesFound()
    {
        if (planeManager == null)
            return false;

        return planeManager.trackables.count > 0;
    }

    private void PlacePoint()
    {
        if (m_ShowingPlacePoint)
        {
            if (placePointCanvasGroup)
                placePointCanvasGroup.alpha = 0f;

            m_ShowingPlacePoint = false;
        }
    }
}
