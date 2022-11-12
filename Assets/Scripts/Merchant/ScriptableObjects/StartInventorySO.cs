using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Merchant.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StartInventory", menuName = "Configs/StartInventoryConfig")]
    [InlineEditor]
    public class StartInventorySO : ScriptableObject
    {
        [SerializeField] private int _startPlayerCash = 1000;
        [SerializeField] private List<InventoryItemSO> _startPlayerInventoryList;

        public int StartPlayerCash => _startPlayerCash;
        public List<InventoryItemSO> StartPlayerInventoryList => _startPlayerInventoryList;
    }
}
