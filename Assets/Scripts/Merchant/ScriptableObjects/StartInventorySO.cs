using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Merchant.ScriptableObjects
{
    [CreateAssetMenu(fileName = "StartInventory", menuName = "Configs/StartInventoryConfig")]
    [InlineEditor]
    public class StartInventorySO : ScriptableObject
    {
        [SerializeField] private int _startPlayerCash = 1000;
        [SerializeField] public List<InventoryItemSO> _startPlayerInventoryList;

        public int StartPlayerCash => _startPlayerCash;
        public List<InventoryItemSO> StartPlayerInventoryList => _startPlayerInventoryList;
    }
}
