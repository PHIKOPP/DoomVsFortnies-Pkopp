using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletshot : MonoBehaviour
{
    public bool shotByBot = false;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.GetComponent<Bot>() && !shotByBot)
        {
            Bot bot = other.gameObject.GetComponent<Bot>();
            bot.health -= Random.Range(25, 100);
            if (bot.health > 0)
                bot.gotHit();
        }
        else if (other.gameObject.GetComponent<PlayerController>())
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.health -= Random.Range(25, 50);
        }
    }

}
