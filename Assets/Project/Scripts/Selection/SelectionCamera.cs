using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionCamera : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 0.05f;
    private Vector3 lastPosition;
    private float zoomMin = 1;
    private float zoomMax = 8;
    private GameObject infoFrame;

    private void Awake()
    {
        infoFrame = GameObject.Find("InfoFrame");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1Pos = touch1.position - touch1.deltaPosition;
            Vector2 touch2Pos = touch2.position - touch2.deltaPosition;

            float prevMagnitude = (touch1Pos - touch2Pos).magnitude;
            float currentMagnitude = (touch1.position - touch2.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);
        }
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                lastPosition = Input.mousePosition;
            }
            Vector3 delta = Input.mousePosition - lastPosition;
            transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
            lastPosition = Input.mousePosition;
        }
    }

    void Zoom(float inc)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - inc, zoomMin, zoomMax);
    }
}
