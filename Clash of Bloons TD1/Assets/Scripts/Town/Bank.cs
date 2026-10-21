using UnityEngine;
using System.Collections;

public class Bank : MonoBehaviour
{
    [Header("Bank Instellingen")]
    public int incomeAmount = 10;
    public float incomeInterval = 5f;

    void Start()
    {
        StartCoroutine(GenerateIncomeRoutine());
    }

    IEnumerator GenerateIncomeRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(incomeInterval);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.townCurrency += incomeAmount;
                Debug.Log("Bank genereerde " + incomeAmount + " munten!");
            }
        }
    }
}