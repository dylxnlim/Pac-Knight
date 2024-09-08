using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Collectible
{
    public float duration = 8.0f;
    public string name;

    protected override void Eat()
    {
        FindObjectOfType<GameManager>().ItemEaten(this, name);
    }
}
