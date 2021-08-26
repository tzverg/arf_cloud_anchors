using UnityEngine;
using Zenject;
using AppName.Core.Extensions;

namespace AppName.App.Installers.Main
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField]
        private Camera _mainCamera;

        public override void InstallBindings()
        {
            Container.DIBindFromInstance<Camera>(_mainCamera);
        }
    }
}