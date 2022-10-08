using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int HighScore = 0;

    public SaveData(int HighScore)
    {
        this.HighScore = HighScore;
    }

    public SaveData()
    {
        this.HighScore = 0;
    }
}
