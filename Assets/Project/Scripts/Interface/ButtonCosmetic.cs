using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCosmetic : MonoBehaviour
{
    Transform text;
    Vector3 defaultP;
    private float offset = -20;

    private void Awake()
    {
        text = transform.GetChild(0);
        defaultP = text.localPosition;
        if (transform.parent.parent.GetComponent<Canvas>().renderMode == RenderMode.WorldSpace)
        {
            offset = -0.001f;
        }
    }

    public void Up()
    {
        print("yueah");
        text.transform.localPosition = defaultP;
    }

    public void Down()
    {
        text.transform.localPosition = new Vector3(0, offset, 0);
    }
}
