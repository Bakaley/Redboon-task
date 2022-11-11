using Merchant.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Merchant.MonoInstallers
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
