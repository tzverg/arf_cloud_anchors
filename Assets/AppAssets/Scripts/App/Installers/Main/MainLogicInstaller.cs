using AppName.Core.Extensions;
using CloudAnchors;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace AppName.App.Installers.Main
{
    public class MainLogicInstaller : MonoInstaller
    {
        [SerializeField] private ARAnchorManager _arAnchorManager;
        [SerializeField] private ARPlacementManager _arPlacementManager;
        [SerializeField] private ARRaycastManager _arRaycastManager;
        [SerializeField] private ARCloudAnchorManager _arCloudAnchorManager;
        public override void InstallBindings()
        {
            Container.DIBindFromInstance<ARAnchorManager>(_arAnchorManager);
            Container.DIBindFromInstance<ARPlacementManager>(_arPlacementManager);
            Container.DIBindFromInstance<ARRaycastManager>(_arRaycastManager);
            Container.DIBindFromInstance<ARCloudAnchorManager>(_arCloudAnchorManager);
        }
    }
}