using Data;
using DefaultNamespace;
using Managers;
using UnityEngine;
#pragma warning disable 0649
namespace HelperWindows.ManageWalletWindow
{
    public class ManageWalletWindow : MonoBehaviour
    {
        [HideInInspector]public Saving saving;
        [SerializeField] private ConfirmWindow confirmWindow;

        public void Init(Saving saving)
        {
            gameObject.SetActive(true);
            this.saving = saving;
        }


        public void OnDeleteSaving()
        {
            confirmWindow.Open(() =>
            {
                var deleted = UserDataManager.Savings.Remove(saving);
                if (!deleted)
                    Debug.LogError("Can't delete wallet");
                
                Events.OnUpdateTab?.Invoke();
                gameObject.SetActive(false);
            });
        }
    }
}