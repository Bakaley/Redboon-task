using System.Collections.Generic;
using Merchant.ScriptableObjects;
using Zenject;

namespace Merchant.UI
{
    public class PlayerInventoryTable : InventoryTable
    {
        [Inject] private PlayerInventory _playerInventory;

        protected override List<InventoryItemSO> ListToFill => _playerInventory.InventoryItems;

        protected override void OnCellPointerEnterHandler(InventoryCell cell, InventoryItem item)
        {
            //we can handle it by ourself in some special windows,
            //or call an Arbitrator (in this case, we need to change InfoPanel)
            Arbitrator.OnPointerEntersCell(this, cell);
        }

        protected override void OnItemClickHandler(InventoryCell cell, InventoryItem item)
        {
            //we can handle it by ourself in some special windows,
            //or call an Arbitrator (in this case, we need to sell item)
            Arbitrator.OnPointerItemClick(this, item);
        }

        protected override void OnItemDraggedHandler(InventoryCell oldCell, InventoryCell newCell, InventoryItem item)
        {
            if (newCell.Table == this)
            {
                //dragging item in the same table in new cell
                MoveItem(oldCell, newCell);
            }
            else
            {
                //item was dragged to cell in another table
                //calling an Arbitrator to decide what to do
                var result = Arbitrator.AttemptToMoveInAnotherWindow(this, newCell.Table, item);
                //if we couldn't move item to another table (we are trying to put sword in helm slot, for example)
                //we return item back to cell
                if (!result) oldCell.CancelDragging();
            }
        }
    }
}
