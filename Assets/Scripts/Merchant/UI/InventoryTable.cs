using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Merchant.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Merchant.UI
{
    /* I am not sure in this solution, but i had several problems needed to be solved:
     Images draw themselves in order on their scene hierarchy, so items need their own canvas with overrided sorting order.
     I could add to each item its own canvas, but, as i know, 100+ canvases in each frame is not very healthy for performance,
     especially for mobile platforms.
     So i decided to add one parent canvas for all items on grid, maybe that was a mistake
          
     I wish i know a better way to solve the sorting order problem, but for now, i don't know such
     */
    
    public abstract class InventoryTable : MonoBehaviour
    {
        private readonly int _cellsCount = 40;
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private InventoryCell _cellSampler;
        [SerializeField] private InventoryItem _itemSampler;

        // Items need their own canvas with overrided sorting layer number so that items are drawn regardless of scene hierarchy.
        [SerializeField] private Canvas _itemCanvas;
        
        // During cells initialization we must wait for GUI update in which cells in grid will apply new transforms
        // Only after that we can snap items to cells' transform positions without parenting them
        private bool _cellsInited = false;

        protected ItemWindowArbitrator Arbitrator { get; private set; }
        protected List<InventoryCell> Cells { get; private set; }
        protected List<InventoryItem> Items { get; private set; }  = new List<InventoryItem>();
        protected abstract List<InventoryItemSO> ListToFill { get; }

        public void Init(ItemWindowArbitrator arbitrator)
        {
            Arbitrator = arbitrator;
        }
        
        public void AddItemToTable(InventoryItem item, InventoryCell cell = null)
        {
            if(cell == null || !cell.IsEmpty) cell = FindFirstEmpty();
            cell.BindItem(item);
            Items.Add(item);
        }

        public void RemoveItemFromTable(InventoryItem item)
        {
            var cell = Cells.Find(tableCell => tableCell.Item == item);
            cell.UnbindItem();
            Items.Remove(item);
        }

        protected abstract void OnCellPointerEnterHandler(InventoryCell cell, InventoryItem item);
        protected abstract void OnItemClickHandler(InventoryCell cell, InventoryItem item);
        protected abstract void OnItemDraggedHandler(InventoryCell oldCell, InventoryCell newCell, InventoryItem item);

        protected virtual void Awake()
        {
            StartCoroutine(FillWithCells(_cellSampler, _cellsCount));
        }
        
        protected virtual async void Start()
        {
            while (!_cellsInited)
            { 
                await Task.Yield();
            }
            
            FillWithItems(ListToFill);
            
            foreach (var cell in Cells)
            {
                cell.OnPointerEnterCell += OnCellPointerEnterHandler;
                cell.OnItemClick += OnItemClickHandler;
                cell.OnItemDragged += OnItemDraggedHandler;
            }
        }
        
        protected void MoveItem(InventoryCell oldCell, InventoryCell newCell)
        {
            var item = oldCell.Item;
            if (newCell.IsEmpty)
            {
                //just move item to another cell
                oldCell.UnbindItem();
                newCell.BindItem(item);
            }
            else
            {
                //swap items' places
                oldCell.BindItem(newCell.Item);
                newCell.BindItem(item);
            }
        }

        private IEnumerator FillWithCells(InventoryCell sampler, int cellsCount)
        {
            for (int i = 0; i < cellsCount; i++)
            {
                var cell = Instantiate(sampler.gameObject, _grid.transform).GetComponent<InventoryCell>();
                cell.Init(this);
            }
            yield return new WaitForEndOfFrame();
            Cells = GetComponentsInChildren<InventoryCell>().ToList();
            _cellsInited = true;
        }

        private void FillWithItems(List<InventoryItemSO> itemList)
        {
            foreach (var item in itemList) CreateItem(item);
        }
        
        private void CreateItem(InventoryItemSO itemSO)
        {
            var item = Instantiate(_itemSampler.gameObject, _itemCanvas.transform).GetComponent<InventoryItem>();
            item.Init(itemSO);
            AddItemToTable(item);
        }

        private InventoryCell FindFirstEmpty()
        {
            return Cells.Find(cell => cell.IsEmpty);
        }
    }
}