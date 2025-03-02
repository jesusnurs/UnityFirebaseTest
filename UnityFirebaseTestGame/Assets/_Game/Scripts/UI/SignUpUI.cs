using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class SignUpUI : UIElement
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField confirmPasswordInputField;
        
        [SerializeField] private Button signUpButton;
        [SerializeField] private Button signInButton;

        private void OnEnable()
        {
            signInButton.onClick.AddListener(OpenSignInUI);
            signUpButton.onClick.AddListener(SignUp);
        }
        
        private void OnDisable()
        {
            signInButton.onClick.RemoveListener(OpenSignInUI);
            signUpButton.onClick.RemoveListener(SignUp);
        }

        private void SignUp()
        {
            FirebaseManager.Instance.EmailSignUp(nameInputField.text, emailInputField.text, 
                passwordInputField.text, confirmPasswordInputField.text);
            
            //TODO Open Profile UI
        }

        private void OpenSignInUI()
        {
            CloseWindow();
            OpenWindow("EmailSignIn");
        }
    }
}
