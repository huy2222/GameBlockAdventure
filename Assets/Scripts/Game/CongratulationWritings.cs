using System;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationWritings : MonoBehaviour
{
    public List<GameObject> congratulationWritings;
    void OnEnable()
    {
        GameEvent.ShowCongratulationWritings += ShowCongratulationWritings;
    }
    void OnDisable()
    {
        GameEvent.ShowCongratulationWritings -= ShowCongratulationWritings;
    }

    private void ShowCongratulationWritings()
    {
        var index = UnityEngine.Random.Range(0, congratulationWritings.Count);
        congratulationWritings[index].SetActive(true);
    }
}
