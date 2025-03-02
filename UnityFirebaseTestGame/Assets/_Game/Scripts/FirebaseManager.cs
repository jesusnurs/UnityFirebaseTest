using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using TMPro;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;
    
    private const string GoogleAPI = "886545348489-v8i2cec50q91qfjgprljsq7no6ob0ctm.apps.googleusercontent.com";

    private GoogleSignInConfiguration _configuration;

    private FirebaseAuth _auth;
    private FirebaseUser _user;
    
    public TextMeshProUGUI text;
    
    private void Awake() 
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
        
        _configuration = new GoogleSignInConfiguration
        {
            WebClientId = GoogleAPI,
            RequestIdToken = true,
        };
    }

    private void Start() 
    {
        InitFirebase();
    }

    private void InitFirebase() 
    {
        _auth = FirebaseAuth.DefaultInstance;
    }
    
    #region GoogleSignIn

    public void GoogleSignIn() 
    {
        try
        {
            Google.GoogleSignIn.Configuration = _configuration;
            Google.GoogleSignIn.Configuration.UseGameSignIn = false;
            Google.GoogleSignIn.Configuration.RequestIdToken = true;
            Google.GoogleSignIn.Configuration.RequestEmail = true;

            Google.GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            text.text = e.Message;
            throw;
        }
        
    }

    private void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task) 
    {
        if (task.IsFaulted) 
        {
            text.text = "Faulted";
        } 
        else if (task.IsCanceled)
        {
            text.text = "Cancelled";
        } 
        else 
        {
            Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            _auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => 
            {
                if (task.IsCanceled) 
                {
                    text.text = "Cancelled";
                    return;
                }

                if (task.IsFaulted) {
                    text.text = "SignInWithCredentialAsync encountered an error: " + task.Exception;
                    return;
                }

                _user = _auth.CurrentUser;

                UIManager.Instance.CloseWindow("GoogleSignIn");
                UIManager.Instance.OpenWindow("Profile");
            });
        }
    }
    
    #endregion
    
    #region EmailSignIn
    
    #region SignIn
    
    public void EmailSignIn(string email, string password)
    {
        StartCoroutine(EmailSignInAsync(email, password));
    }

    private IEnumerator EmailSignInAsync(string email, string password)
    {
        var signInTask = _auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => signInTask.IsCompleted);

        if (signInTask.Exception == null)
        {
            _user = signInTask.Result.User;
            UIManager.Instance.CloseWindow("EmailSignIn");
            UIManager.Instance.OpenWindow("Profile");
        }
    }
    
    #endregion

    #region SignUp
    
    public void EmailSignUp(string userName, string email, string password, string confirmPassword)
    {
        StartCoroutine(EmailSignUpAsync(userName, email, password, confirmPassword));
    }

    private IEnumerator EmailSignUpAsync(string userName, string email, string password, string confirmPassword)
    {
        if (userName == "")
        {
            Debug.LogError("User Name is empty");
        }
        else if (email == "")
        {
            Debug.LogError("Email field is empty");
        }
        else if (password != confirmPassword)
        {
            Debug.LogError("Password does not match");
        }
        else
        {
            var registerTask = _auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception == null)
            {
                _user = registerTask.Result.User;

                UserProfile userProfile = new UserProfile { DisplayName = userName };

                var updateProfileTask = _user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    _user.DeleteAsync();
                }
                else
                {
                    UIManager.Instance.CloseWindow("EmailSignUp");
                    UIManager.Instance.OpenWindow("EmailSignIn");
                }
            }
        }
    }
    
    #endregion
    
    #endregion

    public FirebaseUser GetUserData()
    {
        return _user;
    }
}