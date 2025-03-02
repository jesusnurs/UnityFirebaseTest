using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class ProfileUI : UIElement
    {
        [SerializeField] private TextMeshProUGUI userNameText;
        [SerializeField] private TextMeshProUGUI emailText;

        private void OnEnable()
        {
            var userData = FirebaseManager.Instance.GetUserData();
            userNameText.text = "Name: " + userData.DisplayName;
            emailText.text = "Email: " + userData.Email;
        }

        private void OnDisable()
        {
            
        }
    }
}
