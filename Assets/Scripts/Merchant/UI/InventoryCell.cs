using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Merchant.UI
{
    [RequireComponent(typeof(Image))]
    public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private Sprite _pointerHighlightSprite;

        private InventoryTable _tableParent;
        private InventoryItem _currentItem;
        private Image _cellImage;

        private readonly float _maxDurationClickThreshold = 0.25f;
        
        private PointerEventData _pointerEventData = new PointerEventData(null);
        private List<RaycastResult> _raycastResults = new List<RaycastResult>();

        public bool IsEmpty => _currentItem == null;
        public InventoryItem Item => _currentItem;
        public InventoryTable Table => _tableParent;
        
        public event Action<InventoryCell, InventoryItem> OnPointerEnterCell;
        public event Action<InventoryCell, InventoryItem> OnItemClick;
        public event Action<InventoryCell, InventoryCell, InventoryItem> OnItemDragged;

        public void CancelDragging()
        {
            SnapItem();
        }
        
        public void BindItem(InventoryItem item)
        {
            _currentItem = item;
            SnapItem();
        }

        public void UnbindItem()
        {
            _currentItem = null;
        }

        public void Init(InventoryTable parentTable)
        {
            _tableParent = parentTable;
        }
        
        private void Awake()
        {
            _cellImage = GetComponent<Image>();
        }

        private InventoryCell GetCellUnderMouse()
        {
            _pointerEventData.position = Input.mousePosition;
            EventSystem.current.RaycastAll(_pointerEventData, _raycastResults);
            foreach (var result in _raycastResults)
            {
                if (result.gameObject.TryGetComponent(out InventoryCell cell))
                {
                    return cell;
                }
            }
            return null;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _cellImage.sprite = _pointerHighlightSprite;
            OnPointerEnterCell?.Invoke(this, Item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _cellImage.sprite = _defaultSprite;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_currentItem != null)
            {
                if(Time.unscaledTime - eventData.clickTime < _maxDurationClickThreshold) OnItemClick?.Invoke(this, _currentItem);
                else CancelDragging();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_currentItem) _currentItem.BeginDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(_currentItem) _currentItem.OnDrag();
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_currentItem)
            {
                _currentItem.EndDrag();
                
                var cellUnderMouse = GetCellUnderMouse();
                if (cellUnderMouse != null)
                { 
                    OnItemDragged?.Invoke(this, cellUnderMouse, Item);
                }
                else
                {
                    CancelDragging();
                }
            }
        }

        private void SnapItem()
        {
            _currentItem.transform.position = this.transform.position;
        }
    }
}
