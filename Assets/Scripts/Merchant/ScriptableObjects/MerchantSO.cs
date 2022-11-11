using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Merchant.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MerchantSO", menuName = "Configs/MerchantConfig")]
    [InlineEditor]
    public class MerchantSO : ScriptableObject
    {
        [Range(0, 1)] public float sellingPriceModifier = 0.5f;
        public List<InventoryItemSO> listForTrading;
    }
}
