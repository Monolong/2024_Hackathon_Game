using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int Money { get; private set; }
    public float Satisfaction { get; private set; }

    // Initial values
    [SerializeField] private int initialMoney = 0;
    [SerializeField] private float initialSatisfaction = 50f;

    void Awake()
    {
        // Set initial values for money and satisfaction
        Money = initialMoney;
        Satisfaction = initialSatisfaction;
    }

    // Earn money
    public void EarnMoney(int amount)
    {
        AddMoney(amount);
    }

    // Use money
    public bool UseMoney(int amount)
    {
        if (SpendMoney(amount))
        {
            Debug.Log("Spent money: " + amount + ", Remaining money: " + Money);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough money to spend.");
            return false;
        }
    }

    // Increase satisfaction
    public void IncreaseSatisfaction(float amount)
    {
        ChangeSatisfaction(amount);
    }

    // Decrease satisfaction
    public void DecreaseSatisfaction(float amount)
    {
        ChangeSatisfaction(-amount);
    }

    // Add or subtract money
    private void AddMoney(int amount)
    {
        Money += amount;
    }

    private bool SpendMoney(int amount)
    {
        if (Money >= amount)
        {
            Money -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Change satisfaction
    private void ChangeSatisfaction(float amount)
    {
        Satisfaction += amount;

        // Keep satisfaction within the range of 0 to 100
        Satisfaction = Mathf.Clamp(Satisfaction, 0.0f, 100.0f);
    }
}
