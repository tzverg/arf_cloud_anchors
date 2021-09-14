using AppName.Core.Extensions;
using UnityEngine;
using Zenject;

namespace AppName.App.Installers.Main
{
    public class MainUiInstaller : MonoInstaller
    {
        [SerializeField] private CloudAnchors.UIManager _uiManager;
        public override void InstallBindings()
        {
            Container.DIBindFromInstance<CloudAnchors.UIManager>(_uiManager);
        }
    }
}