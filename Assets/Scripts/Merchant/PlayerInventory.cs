using System;
using System.Collections;
using System.Collections.Generic;
using Merchant;
using Merchant.ScriptableObjects;
using UnityEngine;

public class PlayerInventory
{
    private int _coinsAmount;
    private List<InventoryItemSO> _items;
    
    public event Action<int> CoinsAmountChanged;

    public PlayerInventory(StartInventorySO startInventory)
    {
        _coinsAmount = startInventory.startPlayerCash;
        
        //we are creating a copy so changes in this list will not affect scriptable object
        _items = new List<InventoryItemSO>(startInventory.startPlayerInventoryList);
    }

    public int CoinsAmount => _coinsAmount;
    public List<InventoryItemSO> InventoryItems => _items;

    public void ChangeCoinsCountOn(int delta)
    {
        _coinsAmount += delta;
        CoinsAmountChanged?.Invoke(_coinsAmount);
    }

    public void AddItem(InventoryItem item)
    {
        _items.Add(item.Config);
    }

    public void RemoveItem(InventoryItem item)
    {
        _items.Remove(item.Config);
    }
}
