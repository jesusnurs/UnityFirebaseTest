using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Game.Scripts.UI
{
    public class GoogleSignInUI : UIElement
    {
        [SerializeField] private Button googleSignInButton;

        private void OnEnable()
        {
            googleSignInButton.onClick.AddListener(SignIn);
        }

        private void OnDisable()
        {
            googleSignInButton.onClick.RemoveListener(SignIn);
        }

        private void SignIn()
        {
            FirebaseManager.Instance.GoogleSignIn();
        }
    }
}
