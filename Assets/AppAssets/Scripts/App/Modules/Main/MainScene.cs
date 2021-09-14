using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace AppName.App.Modules.Main
{
    public class MainScene : MonoBehaviour
    {
        [SerializeField] private int _targetFrameRate = 60;

        private void Awake()
        {
            UnityEngine.Application.targetFrameRate = _targetFrameRate;
        }
    }
}