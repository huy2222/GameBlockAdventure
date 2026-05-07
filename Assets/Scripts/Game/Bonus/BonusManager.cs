using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public List<GameObject> bonusPrefabs;

    void OnEnable()
    {
        GameEvent.ShowBonusScreen += ShowBonusScreen;
    }
    void OnDisable()
    {
        GameEvent.ShowBonusScreen -= ShowBonusScreen;
    }

    void ShowBonusScreen(List<SquareColor> colors)
    {
        StartCoroutine(ShowBonusSequential(colors));
    }

    IEnumerator ShowBonusSequential(List<SquareColor> colors)
    {
        foreach (var color in colors)
        {
            GameObject obj = null;

            foreach (var bonus in bonusPrefabs)
            {
                var comp = bonus.GetComponent<Bonus>();
                if (comp.bonusColor == color)
                {
                    obj = bonus;
                    obj.SetActive(true);
                    break;
                }
            }

            if (obj == null)
            {
                Debug.LogError("No bonus found for color: " + color);
                continue;
            }

            // đợi 2s trước khi tắt
            yield return new WaitForSeconds(2f);

            obj.SetActive(false);

            // (tuỳ chọn) delay giữa các lần hiện
            yield return new WaitForSeconds(0.3f);
        }
    }

}

