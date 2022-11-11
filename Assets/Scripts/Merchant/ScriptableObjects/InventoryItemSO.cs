using Sirenix.OdinInspector;
using UnityEngine;

namespace Merchant.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "Configs/ItemConfig")]
    [InlineEditor]
    public class InventoryItemSO : ScriptableObject
    {
        public string id;
        public string title;
        public Sprite icon;
        public int price;
    }
}
