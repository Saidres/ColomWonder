using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    public int coinAmount = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.instance.PlayRandomFromList(SoundManager.instance.coinSounds);
            GameObject.Find("CurrencyController").GetComponent<CurrencyController>().AddCurrency(coinAmount);
            Destroy(gameObject);
        }
    }
}
