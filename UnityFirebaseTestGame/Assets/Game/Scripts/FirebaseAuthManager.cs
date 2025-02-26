using System.Collections;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuthManager Instance;

    private DependencyStatus _dependencyStatus;
    private FirebaseAuth _auth;
    private FirebaseUser _user;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
        
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            _dependencyStatus = task.Result;

            if (_dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + _dependencyStatus);
            }
        });
    }
    
    void InitializeFirebase()
    {
        _auth = FirebaseAuth.DefaultInstance;

        _auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (_auth.CurrentUser != _user)
        {
            bool signedIn = _user != _auth.CurrentUser && _auth.CurrentUser != null;

            if (!signedIn && _user != null)
            {
                Debug.Log("Signed out " + _user.UserId);
            }

            _user = _auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + _user.UserId);
            }
        }
    }

    #region SignIn
    
    public void SignIn(string email, string password)
    {
        StartCoroutine(SignInAsync(email, password));
    }

    private IEnumerator SignInAsync(string email, string password)
    {
        var signInTask = _auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => signInTask.IsCompleted);

        if (signInTask.Exception != null)
        {
            Debug.LogError(signInTask.Exception);

            FirebaseException firebaseException = signInTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;


            string failedMessage = "Login Failed! Because ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }

            Debug.Log(failedMessage);
        }
        else
        {
            _user = signInTask.Result.User;
            Debug.LogFormat("{0} You Are Successfully Logged In", _user.DisplayName);
            UIManager.Instance.CloseWindow("SignIn");
            UIManager.Instance.OpenWindow("Profile");
        }
    }
    
    #endregion

    #region SignUp
    
    public void SignUp(string userName, string email, string password, string confirmPassword)
    {
        StartCoroutine(RegisterAsync(userName, email, password, confirmPassword));
    }

    private IEnumerator RegisterAsync(string userName, string email, string password, string confirmPassword)
    {
        if (userName == "")
        {
            Debug.LogError("User Name is empty");
        }
        else if (email == "")
        {
            Debug.LogError("email field is empty");
        }
        else if (password != confirmPassword)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }

                Debug.Log(failedMessage);
            }
            else
            {
                // Get The User After Registration Success
                _user = registerTask.Result.User;

                UserProfile userProfile = new UserProfile { DisplayName = userName };

                var updateProfileTask = _user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    _user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;


                    string failedMessage = "Profile update Failed! Becuase ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }

                    Debug.Log(failedMessage);
                }
                else
                {
                    Debug.Log("Registration Successful Welcome " + _user.DisplayName);
                    UIManager.Instance.CloseWindow("SignUp");
                    UIManager.Instance.OpenWindow("SignIn");
                }
            }
        }
    }
    
    #endregion

    public FirebaseUser GetCurrentUserData()
    {
        return _user;
    }
}
