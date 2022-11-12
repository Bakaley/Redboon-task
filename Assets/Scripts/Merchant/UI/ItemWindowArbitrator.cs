using UnityEngine;

namespace Merchant.UI
{
    /* Inherited classes will decide what to do in case of dragging, clicking, pointing, etc. in their specific window */
    public abstract class ItemWindowArbitrator : MonoBehaviour
    {
        public abstract bool AttemptToMoveInAnotherWindow(InventoryTable oldTable, InventoryTable newTable, InventoryItem item,
            InventoryCell cellInNewWindow = null);
        public abstract void OnPointerEntersCell(InventoryTable table, InventoryCell cell);
        public abstract void OnPointerItemClick(InventoryTable table, InventoryItem item);
    }
}
