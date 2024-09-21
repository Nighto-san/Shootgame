using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public TMP_Text Kills;
    


    private void OnEnable()
    {
        Kills.text = "Kills:" + PlayerStats.killCount.ToString();

    }
}
