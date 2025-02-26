using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class ProfileUI : UIElement
    {
        [SerializeField] private TextMeshProUGUI userNameText;
        [SerializeField] private TextMeshProUGUI emailText;
        
        [SerializeField] private Button changePasswordButton;
        [SerializeField] private Button deleteAccountButton;

        private void OnEnable()
        {
            var userData = FirebaseAuthManager.Instance.GetCurrentUserData();
            userNameText.text = "Name: " + userData.DisplayName;
            emailText.text = "Email: " + userData.Email;
            
            changePasswordButton.onClick.AddListener(OpenChangePasswordUI);
            deleteAccountButton.onClick.AddListener(OpenConfirmDeleteAccountUI);
        }

        private void OnDisable()
        {
            changePasswordButton.onClick.RemoveListener(OpenChangePasswordUI);
            deleteAccountButton.onClick.AddListener(OpenConfirmDeleteAccountUI);
        }

        private void OpenChangePasswordUI()
        {
            //OpenWindow("ChangePasswordUI");
        }

        private void OpenConfirmDeleteAccountUI()
        {
            //OpenWindow("ConfirmDeleteAccountUI");
        }
    }
}
