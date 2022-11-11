using System.Linq;
using System.Text;
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
            DefaultText,
            Selling,
            Buying
        }

        public void SetDefaultText()
        {
            _text.text = _defaultText;
        }

        public void SetItemInfo(InventoryItemSO item, int price, InfoType type, bool makePriceRed = false)
        {
            string newValue = item.title;
            newValue += _indent;
            switch (type)
            {
                case InfoType.Buying:
                    if (makePriceRed)
                    {
                        newValue += _redColorTag;
                        newValue += _buyingString;
                        newValue += price;
                        newValue += _coinSymbol;
                        newValue += _redColorCloseTag;
                    }
                    else
                    {
                        newValue += _buyingString;
                        newValue += price;
                        newValue += _coinSymbol;
                    }
                    break;
                
                case InfoType.Selling:
                    newValue += _sellingString;
                    newValue += price;
                    newValue += _coinSymbol;
                    break;
            }
            _text.text = newValue;
        }
    }
}
