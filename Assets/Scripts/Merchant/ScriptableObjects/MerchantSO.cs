using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Merchant.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MerchantSO", menuName = "Configs/MerchantConfig")]
    [InlineEditor]
    public class MerchantSO : ScriptableObject
    {
        [Range(0, 1)] [SerializeField] private float _sellingPriceModifier = 0.5f;
        [SerializeField] private List<InventoryItemSO> _listForTrading;

        public float SellingPriceModifier => _sellingPriceModifier;
        public List<InventoryItemSO> ListForTrading => _listForTrading;

    }
}
