using TMPro;
using UnityEngine;
using Zenject;

namespace Merchant.UI
{
    public class CoinsWidget : MonoBehaviour
    {
        [Inject] private PlayerInventory _playerInventory;
        [SerializeField] private TextMeshProUGUI _counter;

        private readonly string _coinSymbol = "$";

        private void Start()
        {
            RefreshCoinsCounter(_playerInventory.CoinsAmount);
        }

        private void OnEnable()
        {
            _playerInventory.CoinsAmountChanged += RefreshCoinsCounter;
        }

        private void RefreshCoinsCounter(int newAmount)
        {
            _counter.text = newAmount + _coinSymbol;
        }

        private void OnDisable()
        {
            _playerInventory.CoinsAmountChanged -= RefreshCoinsCounter;
        }
    }
}
