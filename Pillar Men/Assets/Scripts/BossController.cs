using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    int bossMaxLife = 300; // 300 
    int bossCurrentLife;

    void Start()
    {
        bossCurrentLife = bossMaxLife;
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if ()
    }*/

    public void Damage(int amount)
    {
        bossCurrentLife -= amount;
       // if (am)
    }

    void Update()
    {
        
    }
}
