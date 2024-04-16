using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactionPopularitySlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popularityText;
    [SerializeField] private TextMeshProUGUI changeAmountText;
    [SerializeField] private Color positiveChangeColor;
    [SerializeField] private Color negativeChangeColor;

    public void UpdateUI(int count, int changeAmount)
    {
        popularityText.text = "";

        for (int i = 0; i < count; i++)
        {
            popularityText.text += "X ";
        }

        if (changeAmount == 0)
            ToggleChangeText(false);
        else
            ToggleChangeText(true);

        if (changeAmount > 0)
        {
            changeAmountText.color = positiveChangeColor;
            changeAmountText.text = "+" + changeAmount.ToString();
        }
        else
        {
            changeAmountText.color = negativeChangeColor;
            changeAmountText.text = "-" + Mathf.Abs(changeAmount).ToString();
        }
    }

    public void ToggleChangeText(bool toggle)
    {
        changeAmountText.gameObject.SetActive(toggle);
    }
}
