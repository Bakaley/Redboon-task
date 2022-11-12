using Merchant.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace Merchant.UI
{
    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private readonly string _defaultText = "Наведите курсор на предмет, чтобы увидеть информацию о нём";
        private readonly string _indent = "\n\n";
        private readonly string _buyingString = "Цена покупки: ";
        private readonly string _sellingString = "Цена продажи: ";
        private readonly string _redColorTag = "<color=\"red\">";
        private readonly string _redColorCloseTag = "</color>";
        private readonly string _coinSymbol = "$";
        
        public enum InfoType
        {
            Selling,
            Buying
        }

        public void SetDefaultText()
        {
            _text.text = _defaultText;
        }

        public void SetItemInfo(InventoryItemSO item, int price, InfoType type, bool makePriceRed = false)
        {
            string newValue = item.Title + _indent;
            switch (type)
            {
                case InfoType.Buying:
                    if (makePriceRed)
                    {
                        newValue += _redColorTag + _buyingString + price + _coinSymbol + _redColorCloseTag;
                    }
                    else
                    {
                        newValue += _buyingString + price + _coinSymbol;
                    }
                    break;
                case InfoType.Selling:
                    newValue += _sellingString + price + _coinSymbol;
                    break;
            }
            _text.text = newValue;
        }
    }
}
