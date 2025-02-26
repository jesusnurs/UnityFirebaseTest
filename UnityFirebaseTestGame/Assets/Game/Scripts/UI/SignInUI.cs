using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class SignInUI : UIElement
    {
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        
        [SerializeField] private Button signInButton;
        [SerializeField] private Button signUpButton;

        private void OnEnable()
        {
            signInButton.onClick.AddListener(SignIn);
            signUpButton.onClick.AddListener(OpenSignUpUI);
        }

        private void OnDisable()
        {
            signInButton.onClick.RemoveListener(SignIn);
            signUpButton.onClick.RemoveListener(OpenSignUpUI);
        }

        private void SignIn()
        {
            FirebaseAuthManager.Instance.SignIn(emailInputField.text, passwordInputField.text);
        }

        private void OpenSignUpUI()
        {
            CloseWindow();
            OpenWindow("SignUp");
        }
    }
}
