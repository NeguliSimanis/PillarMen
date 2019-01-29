using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public static PlayerData current;

    #region LIFE
    public int currentLife;
    public int maxLife = 300;
    #endregion

    #region MELEEATTACK
    public int meleeDamage = 20;
    #endregion

}
