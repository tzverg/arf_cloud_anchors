using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CloudAnchors
{    
    public class UIManager : MonoBehaviour 
    {
        [SerializeField] private TextMeshProUGUI _featureMapQualityTextLable;
        [SerializeField] private TextMeshProUGUI _cloudAnchorStateTextLable;
        [SerializeField] private TMP_InputField _cloudAnchorInputField;

        [SerializeField] private GameObject _resolvePanelGO;
        [SerializeField] private Toggle _changeAnchorPosition;

        public bool GetChangeAnchorPositionToggleValue()
        {
            return _changeAnchorPosition.isOn;
        }

        public void ToggleResolvePanelVisible()
        {
            _resolvePanelGO.SetActive(!_resolvePanelGO.activeInHierarchy);
        }

        public void SetCloudAnchorIDInsideInputField(string textValue)
        {
            _cloudAnchorInputField.text = textValue;
        }

        public string GetCloudAnchorIDFromInputField()
        {
            return _cloudAnchorInputField.text;
        }

        public void SetTextFMQ(string textValue)
        {
            _featureMapQualityTextLable.text = textValue;
        }

        public void SetTextCAS(string textValue)
        {
            _cloudAnchorStateTextLable.text = textValue;
        }
    }
}