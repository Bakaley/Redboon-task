using Merchant.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Merchant.UI
{
      /* This class handles all mouse events (click, drag, etc) in player and merchant windows
      * that requires access to other windows and tables */
      
      public class TradingWindowArbitrator : ItemWindowArbitrator
    {
        [Inject] private PlayerInventory _playerInventory;

        [SerializeField] private PlayerInventoryTable _playerInventoryTable;
        [SerializeField] private MerchantInventoryTable _merchantInventoryTable;
        [SerializeField] private InfoPanel _infoPanel;
        private bool AbleToBuyItem(InventoryItemSO item) => _playerInventory.CoinsAmount >= item.Price;

        public override bool AttemptToMoveInAnotherWindow(InventoryTable oldTable, InventoryTable newTable,
            InventoryItem item, InventoryCell cellInNewWindow = null)
        {
            if (oldTable == _playerInventoryTable && newTable == _merchantInventoryTable)
            {
                SellItem(item);
                return true;
            }
            else if (oldTable == _merchantInventoryTable && newTable == _playerInventoryTable)
            {
                return AttemptBuyItem(item, cellInNewWindow);
            }
            return false;
        }


        public override void OnPointerEntersCell(InventoryTable table, InventoryCell cell)
        {
            if (table == _playerInventoryTable)
            {
                if(cell.Item != null)
                    _infoPanel.SetItemInfo(cell.Item.Config, _merchantInventoryTable.GetSellingPriceOfItem(cell.Item.Config),
                        InfoPanel.InfoType.Selling);
                else _infoPanel.SetDefaultText();
            }
            else if (table == _merchantInventoryTable)
            {
                if(cell.Item != null)
                    _infoPanel.SetItemInfo(cell.Item.Config, cell.Item.Config.Price, InfoPanel.InfoType.Buying,
                        _playerInventory.CoinsAmount < cell.Item.Config.Price);
                else _infoPanel.SetDefaultText();
            }
        }

        public override void OnPointerItemClick(InventoryTable table, InventoryItem item)
        {
            if(table == _playerInventoryTable) SellItem(item);
            else if (table == _merchantInventoryTable) AttemptBuyItem(item);

        }

        private void Start()
        {
            _playerInventoryTable.Init(this);
            _merchantInventoryTable.Init(this);
        }

        private void SellItem(InventoryItem item)
        {
            //in real game table would be subscribed on player inventory
            //so there would be no need to remove item from model AND from view
            _playerInventory.RemoveItem(item);
            _playerInventoryTable.RemoveItemFromTable(item);
            
            _playerInventory.ChangeCoinsCountOn(_merchantInventoryTable.GetSellingPriceOfItem(item.Config));
            
            //in real game merchant will be also have their own "inventory", but for now its view only
            _merchantInventoryTable.AddItemToTable(item);
        }

        private bool AttemptBuyItem(InventoryItem item, InventoryCell cell = null)
        {
            if (AbleToBuyItem(item.Config))
            {
                //in real game merchant will be also have their own "inventory", but for now its view only
                _merchantInventoryTable.RemoveItemFromTable(item);
                _playerInventory.ChangeCoinsCountOn(- item.Config.Price);
                
                //in real game table would be subscribed on player inventory
                //so there would be no need to add item to model AND to view
                _playerInventoryTable.AddItemToTable(item, cell);
                _playerInventory.AddItem(item);
                return true;
            }
            return false;
        }
    }
}
