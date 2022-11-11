using System.Collections.Generic;
using Merchant.ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Merchant
{
    [CreateAssetMenu(fileName = "StartInventory", menuName = "Configs/StartInventoryConfig")]
    [InlineEditor]
    public class StartInventorySO : ScriptableObject
    {
        public int startPlayerCash = 1000;
        public List<InventoryItemSO> startPlayerInventoryList;
    }
}
