using Sirenix.OdinInspector;
using UnityEngine;

namespace Merchant.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "Configs/ItemConfig")]
    [InlineEditor]
    public class InventoryItemSO : ScriptableObject
    {
        [SerializeField] public string _id;
        [SerializeField] public string _title;
        [SerializeField] public Sprite _icon;
        [SerializeField] public int _price;

        public string ID => _id;
        public string Title => _title;
        public Sprite Icon => _icon;
        public int Price => _price;
    }
}
