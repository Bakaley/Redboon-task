using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class CoinsWidget : MonoBehaviour
{
    [Inject] private PlayerInventory _playerInventory;
    [SerializeField] private TextMeshProUGUI _counter;

    private void Start()
    {
        RefreshCoinsCounter(_playerInventory.CoinsAmount);
    }

    private void OnEnable()
    {
        _playerInventory.CoinsAmountChanged += RefreshCoinsCounter;
    }

    private void RefreshCoinsCounter(int newAmount)
    {
        _counter.text = newAmount + "$";
    }

    private void OnDisable()
    {
        _playerInventory.CoinsAmountChanged -= RefreshCoinsCounter;
    }
}
