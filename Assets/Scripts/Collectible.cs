using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public int points = 10;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {   
            Eat();
        }
    }

    protected virtual void Eat()
    {
        FindObjectOfType<GameManager>().CollectibleEaten(this);
    }
}
