using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CurrencyController : MonoBehaviour
{
    public TMP_Text currencyText;
    public int currencyAmount = 0;

    private void Start()
    {
        UpdateCurrencyText();
    }

    public void AddCurrency(int amount)
    {
        currencyAmount += amount;
        UpdateCurrencyText();
    }

    public void RemoveCurrency(int amount)
    {
        currencyAmount -= amount;
        if (currencyAmount < 0)
        {
            currencyAmount = 0;
        }
        UpdateCurrencyText();
    }

    public void UpdateCurrencyText()
    {
        currencyText.text = currencyAmount.ToString();
    }
}
