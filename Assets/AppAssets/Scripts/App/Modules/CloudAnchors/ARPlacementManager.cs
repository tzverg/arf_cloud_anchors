using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Zenject;

namespace CloudAnchors
{
    public class ARPlacementManager : MonoBehaviour
    {
        [SerializeField] private GameObject _placedPrefab = null;

        [SerializeField] private ARAnchorManager _arAnchorManager = null;
        [SerializeField] private ARRaycastManager _arRaycastManager = null;
        [SerializeField] private ARCloudAnchorManager _arCloudAnchorManager = null;
        private GameObject _placedGameObject = null;
        private List<ARRaycastHit> _hitResultList;

        [Inject] private void Construct(
            ARAnchorManager arAnchorManager,
            ARRaycastManager arRaycastManager,
            ARCloudAnchorManager arCloudAnchorManager
        )
        {
            _arAnchorManager = arAnchorManager;
            _arRaycastManager = arRaycastManager;
            _arCloudAnchorManager = arCloudAnchorManager;
        }

        public void ReCreatePlacement(Transform transform)
        {
            _placedGameObject = Instantiate(_placedPrefab, transform.position, transform.rotation);
            _placedGameObject.transform.parent = transform;
        }

        private bool TryGetTouchPosition(out Vector2 touchPosition)
        {
            touchPosition = Vector2.zero;

            #if UNITY_ANDROID || UNITY_IOS
                if (Input.touchCount > 0)
                {
                    if(Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        touchPosition = Input.GetTouch(0).position;
                        return true;
                    }
                }
            #else
                if(Input.GetMouseButtonDown(0))
                {
                    touchPosition = Input.mousePosition;
                    return true;
                }
            #endif

            return false;
        }

        private void Awake() 
        {
            _hitResultList = new List<ARRaycastHit>();
        }

        public void ClearPlacedObject()
        {
            if (_placedGameObject != null)
            {
                Debug.Log($"destroy object: {_placedGameObject.name}");
                _placedGameObject.GetComponentInChildren<SpiderKiller>().OnKillObject();
            }
            else
            {
                Debug.LogError($"we have no object for destroy");
            }
        }

        void Update()
        {
            if (_placedGameObject == null)
            {
                if(!TryGetTouchPosition(out Vector2 _touchPosition))
                    return;
                if (_arRaycastManager.Raycast(_touchPosition, _hitResultList, UnityEngine.XR.ARSubsystems.TrackableType.Planes))
                {
                    Pose hitPose = _hitResultList[0].pose;
                    _placedGameObject = Instantiate(_placedPrefab, hitPose.position, hitPose.rotation);
                    //ARAnchor arAnchor = _arAnchorManager.AddAnchor(new Pose(hitPose.position, hitPose.rotation));
                    GameObject arAnchorGO = new GameObject("arAnchor");
                    arAnchorGO.transform.position = _placedGameObject.transform.position;
                    arAnchorGO.transform.parent = _arAnchorManager.transform;
                    ARAnchor arAnchor = arAnchorGO.AddComponent<ARAnchor>();
                    Debug.Log($"anchor added on the scene to position: {arAnchorGO.transform.position.ToString()}");
                    _placedGameObject.transform.parent = arAnchor.transform;

                    _arCloudAnchorManager.QueueAnchor(arAnchor);
                }
            }
        }
    }
}
