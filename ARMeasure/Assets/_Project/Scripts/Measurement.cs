using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Measurement : MonoBehaviour
{
    [SerializeField]
    private GameObject m_pointPrefab;
    public GameObject pointPrefab
    {
        get { return m_pointPrefab; }
        private set { m_pointPrefab = value; }
    }

    [SerializeField]
    private GameObject m_linePrefab;
    public GameObject linePrefab
    {
        get { return m_linePrefab; }
        private set { m_linePrefab = value; }
    }

    [SerializeField]
    private Button buttonPlacePoint;

    [SerializeField]
    private Button buttonClear;

    [SerializeField]
    private Button buttonUndo;

    public event Action onPlacedObject;

    private List<GameObject> spawnedPoints = new List<GameObject>();
    private List<GameObject> spawnedLines = new List<GameObject>();

    private void Awake()
    {
        buttonPlacePoint.onClick.AddListener(ClickPlacePoint);
        buttonClear.onClick.AddListener(ClearPointsAndLines);
        buttonUndo.onClick.AddListener(ClickUndo);
    }

    public void ClickPlacePoint()
    {
        Pose hitPose = PlaceARFocus.s_Hits[0].pose;

        GameObject spawnedPoint = Instantiate(pointPrefab, hitPose.position, hitPose.rotation);
        spawnedPoints.Add(spawnedPoint);

        if (spawnedPoints.Count > 1)
        {
            Vector3 from = spawnedPoints[spawnedPoints.Count - 2].transform.position;
            Vector3 to = spawnedPoints[spawnedPoints.Count - 1].transform.position;

            GameObject line = Instantiate(linePrefab);
            line.GetComponent<LineRenderer>().SetPosition(0, from);
            line.GetComponent<LineRenderer>().SetPosition(1, to);
            spawnedLines.Add(line);

            Vector3 centerPoint = (to + from) / 2f;
            line.transform.GetChild(0).localPosition = centerPoint;

            Vector3 direction = to - from;
            float distance = Vector3.Magnitude(direction);
            line.GetComponentInChildren<Text>().text = Mathf.Round(distance * 100f).ToString() + " cm";

            Debug.Log(
                "From A: " + from + " to B: " + to + "\n" +
                "Center of Line: " + centerPoint + "\n" +
                "Direction: " + direction + "\n");
        }

        if (onPlacedObject != null)
        {
            onPlacedObject();
        }
    }

    private void ClickUndo()
    {
        if(spawnedPoints.Count > 0)
        {
            Destroy(spawnedPoints[spawnedPoints.Count - 1]);
            spawnedPoints.RemoveAt(spawnedPoints.Count - 1);
        }

        if (spawnedLines.Count > 0)
        {
            Destroy(spawnedLines[spawnedLines.Count - 1]);
            spawnedLines.RemoveAt(spawnedLines.Count - 1);
        }
    }

    private void ClearPointsAndLines()
    {
        foreach(var point in spawnedPoints)
        {
            Destroy(point);
        }
        spawnedPoints.Clear();

        foreach (var line in spawnedLines)
        {
            Destroy(line);
        }
        spawnedLines.Clear();
    }
}
