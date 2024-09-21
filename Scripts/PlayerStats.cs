using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static float hp;
    public static float  maxHp = 100;
    public static float killCount = 0;

    private void Start()
    {
        hp = maxHp;
    }


}
