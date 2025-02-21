using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interfaces aren't supported so this is what we've come to *puke*
// Also it needs Start for disabling instead of Awake which I use for initializing variables and only adds insult to injury.
public class GrossActiveItemScript : MonoBehaviour
{
    private BirdHealth birdHealth;

    private void Start()
    {
        birdHealth = GameObject.Find("Bird").GetComponent<BirdHealth>();
    }

    IEnumerator HealUpBird(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(0.2f);
            birdHealth.AddHealth(1);
        }
    }

    public void UseItem(string itemName)
    {

        switch(itemName)
        {
            case "Grapes":
                Grape();
                break;
        }

        void Grape()
        {
            StartCoroutine(HealUpBird(birdHealth.Containers));
        }
    }
}
