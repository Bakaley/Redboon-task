using System.Collections.Generic;
using Merchant.ScriptableObjects;
using UnityEngine;

namespace Merchant.UI
{
    public class MerchantInventoryTable : InventoryTable
    {
        [SerializeField] private MerchantSO _merchantConfig;

        protected override List<InventoryItemSO> ListToFill => _merchantConfig.ListForTrading;

        //in real game that method would be somewhere else, like Merchant.cs
        public int GetSellingPriceOfItem(InventoryItemSO item) =>
            (int) (item.Price * _merchantConfig.SellingPriceModifier);

        protected override void OnCellPointerEnterHandler(InventoryCell cell, InventoryItem item)
        {
            //we can handle it by ourself in some special windows,
            //or call an Arbitrator (in this case, we need to change InfoPanel)
            Arbitrator.OnPointerEntersCell(this, cell);
        }

        protected override void OnItemClickHandler(InventoryCell cell, InventoryItem item)
        {
            //we can handle it by ourself in some special windows,
            //or call an Arbitrator (in this case, we are trying to buy item)
            Arbitrator.OnPointerItemClick(this, item);
        }

        protected override void OnItemDraggedHandler(InventoryCell oldCell, InventoryCell newCell, InventoryItem item)
        {
            if (newCell.Table == this)
            {
                //dragging items inside merchant's inventory is not allowed (like in Witcher 3)
                oldCell.CancelDragging();
            }
            else
            {
                //item was dragged to cell in another table
                //calling an Arbitrator to decide what to do
                var result = Arbitrator.AttemptToMoveInAnotherWindow(this, newCell.Table, item, newCell);
                //if we couldn't move item to another table (not enough money, for example)
                //we return item back to cell
                if (!result) oldCell.CancelDragging();
            }
        }
    }
}
