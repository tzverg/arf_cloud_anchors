using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace CloudAnchors
{
    enum CloudAnchorStorage { GoogleCloud, AzureSpatial }
    public class ARCloudAnchorManager : MonoBehaviour
    {
        [SerializeField] private Camera _arCamera = null;
        [SerializeField] private ARPlacementManager _arPlacementManager = null;
        [SerializeField] private ARAnchorManager _arAnchorManager = null;
        [SerializeField] private UIManager _uiManager = null;
        [SerializeField] [Range(0F, 100F)] private uint _resolveAnchorPassedTimeout = 10;

        private ARAnchor _pendingHostARAnchor = null;
        private ARCloudAnchor _cloudAnchor = null;

        [SerializeField] private CloudAnchorStorage _cloudAnchorStorage = CloudAnchorStorage.GoogleCloud;

        private Pose ARCameraPose
        {
            get
            {
                return new Pose(_arCamera.transform.position, _arCamera.transform.rotation);
            }
        }

        private string _anchorToResolveID;
        private bool _anchorResolveInProgress = false;
        private bool _anchorHostingInProgress = false;
        private bool _featureMapQualityCheckingProgress = false;

        private float _safeToResolvePassed = 0F;

        [Inject] private void Construct(
            Camera arCamera,
            UIManager uiManager,
            ARAnchorManager arAnchorManager, 
            ARPlacementManager arPlacementManager
        )
        {
            _arCamera = arCamera;
            _uiManager = uiManager;
            _arAnchorManager = arAnchorManager;
            _arPlacementManager = arPlacementManager;
        }

        #region Cloud Anchor Cycle
            public void QueueAnchor(ARAnchor arAnchor)
            {
                Debug.Log("Anchor pending for hosting progress...");
                _pendingHostARAnchor = arAnchor;
            }

            public void HostAnchor()
            {
                Debug.Log("Host anchor call in progress");

                _cloudAnchor = _arAnchorManager.HostCloudAnchor(_pendingHostARAnchor, 1);

                if(_cloudAnchor == null)
                {
                    Debug.LogError("Unable to host cloud anchor");
                }
                else
                {
                    _featureMapQualityCheckingProgress = true;
                }
            }

            public void ResolveAnchor()
            {
                Debug.Log("Resolve anchor call in progress");

                _anchorToResolveID = _uiManager.GetCloudAnchorIDFromInputField();
                _cloudAnchor = _arAnchorManager.ResolveCloudAnchorId(_anchorToResolveID);
                
                _uiManager.ToggleResolvePanelVisible();

                if(_cloudAnchor == null)
                {
                    Debug.LogError($"Unable to resolve cloud anchor {_anchorToResolveID}");
                }
                else
                {
                    _anchorResolveInProgress = true;
                }
            }

            private void CheckFeatureMapQualityProgress()
            {
                // recomended up to 30 seconds of scanning before calling host anchor
                FeatureMapQuality quality = _arAnchorManager.EstimateFeatureMapQualityForHosting(ARCameraPose);
                _uiManager.SetTextFMQ(quality.ToString());
                
                switch (quality)
                {
                    case FeatureMapQuality.Good:
                        _featureMapQualityCheckingProgress = false;
                        _anchorHostingInProgress = true;
                        break;
                    default:
                        break;
                }
            }

            public void CheckAnchorHostingProgress()
            {
                CloudAnchorState cloudAnchorState = _cloudAnchor.cloudAnchorState;
                _uiManager.SetTextCAS(cloudAnchorState.ToString());

                if(cloudAnchorState == CloudAnchorState.Success)
                {
                    _anchorHostingInProgress = false;
                    _anchorToResolveID = _cloudAnchor.cloudAnchorId;

                    _uiManager.SetCloudAnchorIDInsideInputField(_anchorToResolveID);

                    Debug.Log($"Cloud anchor hosting state: {cloudAnchorState}");
                }
                else if(cloudAnchorState != CloudAnchorState.TaskInProgress)
                {
                    Debug.LogError($"Error while hosting cloud anchor {cloudAnchorState}");
                    _anchorHostingInProgress = false;
                }
            }

            public void CheckAnchorResolvingProgress()
            {
                CloudAnchorState cloudAnchorState = _cloudAnchor.cloudAnchorState;
                _uiManager.SetTextCAS(cloudAnchorState.ToString());

                Debug.Log($"Cloud anchor resolving state: {cloudAnchorState}, id: {_anchorToResolveID}");

                if(cloudAnchorState == CloudAnchorState.Success)
                {
                    _anchorResolveInProgress = false;
                    _arPlacementManager.ReCreatePlacement(_cloudAnchor.transform);
                    
                    Debug.Log($"Cloud anchor resolving state: {cloudAnchorState}");
                }
                else if(cloudAnchorState != CloudAnchorState.TaskInProgress)
                {
                    Debug.LogError($"Error while resolving cloud anchor {cloudAnchorState}");
                    _anchorResolveInProgress = false;
                }
            }
        #endregion

        public void DropdownValueChanged(TMPro.TMP_Dropdown change)
        { 
            switch (change.value)
            {
                case (int)CloudAnchorStorage.AzureSpatial:
                    _cloudAnchorStorage = CloudAnchorStorage.AzureSpatial;
                    break;
                default:
                    _cloudAnchorStorage = CloudAnchorStorage.GoogleCloud;
                    break;
            }
        }

        private void Update()
        {
            if (_featureMapQualityCheckingProgress)
            {
                CheckFeatureMapQualityProgress();
            }

            // checking for host result
            if (_anchorHostingInProgress)
            {
                CheckAnchorHostingProgress();
                return;
            }

            // checking for resolve result
            if (_anchorResolveInProgress && _safeToResolvePassed <= 0)
            {
                _safeToResolvePassed = _resolveAnchorPassedTimeout;

                if (!string.IsNullOrEmpty(_anchorToResolveID))
                {
                    CheckAnchorResolvingProgress();
                }
                else
                {
                    Debug.LogError("ID of anchor to resolve not definited");
                }
            }
            else
            {
                _safeToResolvePassed -= Time.deltaTime * 1.0F;
            }
        }
    }
}