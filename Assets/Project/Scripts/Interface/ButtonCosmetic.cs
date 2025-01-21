using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCosmetic : MonoBehaviour
{
    Transform text;
    Vector3 defaultP;

    private void Awake()
    {
        text = transform.GetChild(0);
        defaultP = text.localPosition;
    }

    public void Up()
    {
        print("yueah");
        text.transform.localPosition = defaultP;
    }

    public void Down()
    {
        text.transform.localPosition = new Vector3(0, -20, 0);
    }
}
