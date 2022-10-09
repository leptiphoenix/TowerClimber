using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int HighScore = 0;
    public bool pref = true;

    public SaveData(int HighScore, bool pref)
    {
        this.HighScore = HighScore;
        this.pref = pref;
    }

    public SaveData()
    {
        this.HighScore = 0;
        this.pref = true;
    }
}
