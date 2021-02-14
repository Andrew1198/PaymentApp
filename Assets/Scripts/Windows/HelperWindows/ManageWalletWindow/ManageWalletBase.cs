using Data;
using TMPro;
using UnityEngine;

#pragma warning disable 0649
namespace Windows.HelperWindows.ManageWalletWindow
{
    public abstract class ManageWalletBase : MonoBehaviour
    {
        [SerializeField] private ManageWalletWindow manageWalletWindow;
        [SerializeField] protected TextMeshProUGUI selectedWalletText;
        [SerializeField] protected TMP_InputField countField;

        protected Saving SelectedSaving => manageWalletWindow.saving;

        protected virtual void Reset()
        {
            selectedWalletText.text = null;
            countField.text = null;
        }


        public virtual void Init()
        {
            selectedWalletText.text = SelectedSaving.name;
            gameObject.SetActive(true);
        }

        public virtual void OnOk()
        {
        }

        public virtual void OnClose()
        {
            gameObject.SetActive(false);
            manageWalletWindow.gameObject.SetActive(false);
            Reset();
        }
    }
}