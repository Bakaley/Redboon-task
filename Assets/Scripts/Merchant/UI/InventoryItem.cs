using Merchant.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Merchant.UI
{
    [RequireComponent(typeof(Image))]
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private Color _draggingColor;
        [SerializeField] private Color _defaultColor;
        
        private InventoryItemSO _config;
        private Image _icon;

        public InventoryItemSO Config => _config;
        
        private void Awake()
        {
            _icon = GetComponent<Image>();
            _icon.color = _defaultColor;
        }

        public void Init(InventoryItemSO config)
        {
            _config = config;
            _icon.sprite = config.Icon;
        }

        public void BeginDrag()
        {
            _icon.color = _draggingColor;
        }

        public void OnDrag()
        {
            transform.position = Input.mousePosition;
        }
        
        public void EndDrag()
        {
            _icon.color = _defaultColor;
        }
    }
}
