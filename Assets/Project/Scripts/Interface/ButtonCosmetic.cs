using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCosmetic : MonoBehaviour
{
    Transform text;
    Vector3 defaultP;
    private float offset = -20;

    private void Awake()
    {
        text = transform.GetChild(0);
        defaultP = text.localPosition;
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            offset = -0.001f;
        }
    }

    public void Up()
    {
        text.transform.localPosition = defaultP;
    }

    public void Down()
    {
        if (GameVariables.Instance.CanInteractSelect())
        {
            AudioManager.PlaySound(GameVariables.Instance.ButtonClickSound, 0.5f, 0.8f);
        }
        text.transform.localPosition = new Vector3(0, offset, 0);
    }
}
