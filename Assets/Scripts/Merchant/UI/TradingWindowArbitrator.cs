using System;
using System.Threading.Tasks;
using Merchant.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Merchant.UI
{
    /*This class handles all mouse events (click, drag, etc) in player and merchant windows */
    public class TradingWindowArbitrator : MonoBehaviour
    {
        [Inject] private PlayerInventory _playerInventory;
        
        [SerializeField] private InventoryTable _playerInventoryTable;
        [SerializeField] private InventoryTable _merchantInventoryTable;
        [SerializeField] private MerchantSO _merchantConfig;
        [SerializeField] private InfoPanel _infoPanel;
        
        private int GetSellingPriceOfItem(InventoryItemSO item) => (int) (item.price * _merchantConfig.sellingPriceModifier);
        private bool AbleToBuyItem(InventoryItemSO item) => _playerInventory.CoinsAmount >= item.price;
        
        private async void Start()
        {
            await _playerInventoryTable.Init(_playerInventory.InventoryItems);
            foreach (var cell in _playerInventoryTable.Cells)
            {
                cell.OnPointerEnterCell += OnPlayerCellPointerEnterHandler;
                cell.OnItemClick += OnPlayerItemClickHandler;
                cell.OnItemDragged += OnPlayerItemDraggedHandler;
            }
            
            await _merchantInventoryTable.Init(_merchantConfig.listForTrading);
            foreach (var cell in _merchantInventoryTable.Cells)
            {
                cell.OnPointerEnterCell += OnMerchantCellPointerEnterHandler;
                cell.OnItemClick += OnMerchantItemClickHandler;
                cell.OnItemDragged += OnMerchantItemDraggedHandler;
            }
        }

        private void SellItem(InventoryItem item)
        {
            //in real game table would be subscribed on player inventory
            //so there would be no need to remove item from model AND from view
            _playerInventory.RemoveItem(item);
            _playerInventoryTable.RemoveItemFromTable(item);
            
            _playerInventory.ChangeCoinsCountOn(GetSellingPriceOfItem(item.Config));
            
            //in real game merchant will be also have their own "inventory", but for now its view only
            _merchantInventoryTable.AddItemToTable(item);
        }

        private bool TryBuyItem(InventoryItem item, InventoryCell cell = null)
        {
            if (AbleToBuyItem(item.Config))
            {
                //in real game merchant will be also have their own "inventory", but for now its view only
                _merchantInventoryTable.RemoveItemFromTable(item);
                _playerInventory.ChangeCoinsCountOn(- item.Config.price);
                
                //in real game table would be subscribed on player inventory
                //so there would be no need to add item to model AND to view
                _playerInventoryTable.AddItemToTable(item, cell);
                _playerInventory.AddItem(item);
                return true;
            }
            return false;
        }

        private void OnPlayerCellPointerEnterHandler(InventoryCell cell)
        {
            if(cell.Item != null)
                _infoPanel.SetItemInfo(cell.Item.Config, GetSellingPriceOfItem(cell.Item.Config), InfoPanel.InfoType.Selling);
            else _infoPanel.SetDefaultText();
        }

        private void OnMerchantCellPointerEnterHandler(InventoryCell cell)
        {
            if(cell.Item != null)
                _infoPanel.SetItemInfo(cell.Item.Config, cell.Item.Config.price, InfoPanel.InfoType.Buying,
                    _playerInventory.CoinsAmount < cell.Item.Config.price);
            else _infoPanel.SetDefaultText();
        }

        private void OnPlayerItemClickHandler(InventoryCell cell, InventoryItem item)
        {
            SellItem(item);
        }
        
        private void OnMerchantItemClickHandler(InventoryCell cell, InventoryItem item)
        {
            TryBuyItem(item);
        }

        private void OnPlayerItemDraggedHandler(InventoryCell oldCell, InventoryCell newCell, InventoryItem item)
        {
            if (newCell.Table == _merchantInventoryTable)
            {
                //item was dragged to merchant table, selling it
                SellItem(item);
            }
            else
            {
                //dragging item in the same table in new cell
                _playerInventoryTable.MoveItem(oldCell, newCell);
            }
        }
        
        private void OnMerchantItemDraggedHandler(InventoryCell oldCell, InventoryCell newCell, InventoryItem item)
        {
            if (newCell.Table == _playerInventoryTable)
            {
                //item was dragged to player table, trying to buy it
                var result = TryBuyItem(item, newCell);
                if(!result)
                {
                    //player had not enough coins, putting item back
                    oldCell.CancelDragging();
                }
            }
            else
            {
                //dragging items inside merchant's inventory is not allowed (like in Witcher 3)
                oldCell.CancelDragging();
            }
        }
    }
}
