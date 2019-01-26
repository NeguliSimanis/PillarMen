using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [SerializeField]
    int bossMaxLife = 300; // 300 
    int bossCurrentLife;

    [SerializeField]
    Slider bossHPBar;
    [SerializeField]
    GameStateManager gameStateManager;

    bool isDead = false;


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
        UpdateHPBar();
        if (bossCurrentLife <= 0)
        {
            Die();
        }
    }

    private void UpdateHPBar()
    {
        bossHPBar.value = (bossCurrentLife * 1f) / bossMaxLife;
    }

    private void Die()
    {
        if (isDead)
            return;
        isDead = true;
        gameStateManager.WinGame();
    }

    void Update()
    {
        
    }
}
