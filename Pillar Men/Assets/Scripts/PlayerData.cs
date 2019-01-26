using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static PlayerData current;

    #region LIFE
    public int currentLife;
    public int maxLife = 100;
    #endregion

    #region MELEEATTACK
    public int meleeDamage = 20;
    #endregion

}
