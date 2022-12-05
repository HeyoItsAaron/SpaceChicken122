using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyDoor : MonoBehaviour
{
    // variables
    public PlayerStats player;
    public int doorCost;

    // methods
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>();
    }

    public void BuyDoor()
    {
        var random = new Random();
        var list = new List<int> { 750, 1000, 1250, 1500 };
        int doorCost = random.Next(list.Count);
        
        if (player.currCurrency >= doorCost)
        {
            player.currCurrency -= doorCost;
            gameObject.SetActive(false);
        }
    }
}
