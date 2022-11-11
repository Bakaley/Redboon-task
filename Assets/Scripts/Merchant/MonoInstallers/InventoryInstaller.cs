using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

namespace Merchant
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private StartInventorySO _startInventorySo;
        
        private PlayerInventory _playerInventory;
        public override void InstallBindings()
        {
            _playerInventory = new PlayerInventory(_startInventorySo);
            Container.Bind<PlayerInventory>().FromInstance(_playerInventory);
        }
    }
}
