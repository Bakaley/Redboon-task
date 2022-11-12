using Sirenix.OdinInspector;
using UnityEngine;

namespace Merchant.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Item", menuName = "Configs/ItemConfig")]
    [InlineEditor]
    public class InventoryItemSO : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _title;
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _price;

        public string ID => _id;
        public string Title => _title;
        public Sprite Icon => _icon;
        public int Price => _price;
    }
}
