using System.Collections;
using System.Linq;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using UnityEngine;
using VitaliyNULL.Managers;
using UserProfile = Firebase.Auth.UserProfile;

namespace VitaliyNULL.Firebase
{
    public class FirebaseManager : MonoBehaviour
    {
        private readonly string _passwordKey = "PASSWORD";
        private readonly string _emailKey = "EMAIL";
        private bool _isSavingData = false;
        private bool _isAutoLogin = false;

        //Firebase variables
        public FirebaseAuth Auth;
        public FirebaseUser User;
        public DatabaseReference DataBaseReference;
        private bool _isInitialized;
        public static FirebaseManager Instance;

        //Login variables
        [Header("Login")] public TMP_InputField emailLoginField;
        public TMP_InputField passwordLoginField;
        public TMP_Text warningLoginText;
        public TMP_Text confirmLoginText;

        //Register variables
        [Header("Register")] public TMP_InputField usernameRegisterField;
        public TMP_InputField emailRegisterField;
        public TMP_InputField passwordRegisterField;
        public TMP_InputField passwordRegisterVerifyField;
        public TMP_Text warningRegisterText;

        void Start()
        {
            UIManager.Instance.OpenLoadingWidnow();
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            else
            {
                Instance = this;
            }

            // FirebaseApp.CheckAndFixDependenciesAsync();
            InitializeFirebase();
            if (PlayerPrefs.HasKey(_emailKey))
            {
                _isAutoLogin = true;
                StartCoroutine(Login(PlayerPrefs.GetString(_emailKey), PlayerPrefs.GetString(_passwordKey)));
            }
            else
            {
                UIManager.Instance.OpenRegistrationWindow();
                UIManager.Instance.CloseLoadingWindow();
            }

            StartCoroutine(LoadScoreBoardData());
        }

        private void InitializeFirebase()
        {
            Auth = FirebaseAuth.DefaultInstance;
            DataBaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        }


        //Function for the login button
        public void LoginButton()
        {
            //Call the login coroutine passing the email and password
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        }

        //Function for the register button
        public void RegisterButton()
        {
            //Call the register coroutine passing the email, password, and username
            StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
        }

        private IEnumerator Login(string email, string password)
        {
            //Call the Firebase auth signin function passing the email and password
            var loginTask = Auth.SignInWithEmailAndPasswordAsync(email, password);
            //Wait until the task completes
            yield return new WaitUntil(() => loginTask.IsCompleted);

            if (loginTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {loginTask.Exception}");
                FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
                Debug.Log(firebaseEx?.Message);
                if (firebaseEx == null)
                {
                    Debug.Log("FirebaseException is null");
                }
                else
                {
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                    string message = "Login Failed!";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WrongPassword:
                            message = "Wrong Password";
                            break;
                        case AuthError.InvalidEmail:
                            message = "Invalid Email";
                            break;
                        case AuthError.UserNotFound:
                            message = "Account does not exist";
                            break;
                    }

                    Debug.Log(message);
                    warningLoginText.text = message;
                }
            }
            else
            {
                //User is now logged in
                //Now get the result
                User = loginTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
                warningLoginText.text = "";
                StartCoroutine(LoadUserData());
                // StartCoroutine(LoadScoreBoardData());
                if (!_isAutoLogin)
                {
                    PlayerPrefs.SetString(_passwordKey, passwordLoginField.text);
                    PlayerPrefs.SetString(_emailKey, emailLoginField.text);
                }
                UIManager.Instance.OpenMainMenu();
                UIManager.Instance.CloseLoadingWindow();
            }
        }

        private IEnumerator Register(string email, string password, string username)
        {
            if (username == "")
            {
                //If the username field is blank show a warning
                warningRegisterText.text = "Missing Username";
            }
            else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
            {
                //If the password does not match show a warning
                warningRegisterText.text = "Password Does Not Match!";
            }
            else
            {
                //Call the Firebase auth signin function passing the email and password
                var registerTask = Auth.CreateUserWithEmailAndPasswordAsync(email, password);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

                if (registerTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
                    FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                    if (firebaseEx == null)
                    {
                        Debug.Log("FirebaseException is null");
                    }
                    else
                    {
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                        string message = "Register Failed!";
                        switch (errorCode)
                        {
                            case AuthError.MissingEmail:
                                message = "Missing Email";
                                break;
                            case AuthError.MissingPassword:
                                message = "Missing Password";
                                break;
                            case AuthError.WeakPassword:
                                message = "Weak Password";
                                break;
                            case AuthError.EmailAlreadyInUse:
                                message = "Email Already In Use";
                                break;
                        }

                        warningRegisterText.text = message;
                    }
                }
                else
                {
                    //User has now been created
                    //Now get the result
                    User = registerTask.Result;

                    if (User != null)
                    {
                        //Create a user profile and set the username
                        UserProfile profile = new UserProfile { DisplayName = username };

                        //Call the Firebase auth update user profile function passing the profile with the username
                        var profileTask = User.UpdateUserProfileAsync(profile);
                        //Wait until the task completes
                        yield return new WaitUntil(predicate: () => profileTask.IsCompleted);

                        if (profileTask.Exception != null)
                        {
                            //If there are errors handle them
                            Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");

                            warningRegisterText.text = "Username Set Failed!";
                        }
                        else
                        {
                            StartCoroutine(LoadUserData());
                            // StartCoroutine(LoadScoreBoardData());
                            PlayerPrefs.SetString(_passwordKey, passwordRegisterField.text);
                            PlayerPrefs.SetString(_emailKey, emailRegisterField.text);
                            _isAutoLogin = true;
                            UIManager.Instance.OpenMainMenu();
                            UIManager.Instance.CloseLoadingWindow();
                            warningRegisterText.text = "";
                        }
                    }
                }
            }
        }

        public void ExitAccout()
        {
            Auth.SignOut();
            ScoreManager.Instance.ResetBestScoreForNewCustomer();
            _isAutoLogin = false;
            UIManager.Instance.OpenAuthWindow();
        }

        public void SaveData()
        {
            if (!_isSavingData)
            {
                StartCoroutine(UpdateUsernameAuth(Auth.CurrentUser.DisplayName));
                StartCoroutine(UpdateUsernameDatabase(Auth.CurrentUser.DisplayName));
                StartCoroutine(UpdateBestScore(ScoreManager.Instance.GetBestScore()));
            }
        }

        public void RegisterNewAccount()
        {
            Auth.SignOut();
            ScoreManager.Instance.ResetBestScoreForNewCustomer();
            _isAutoLogin = false;
            UIManager.Instance.OpenRegistrationWindow();
        }

        public void SignInExistingAccount()
        {
            Auth.SignOut();
            ScoreManager.Instance.ResetBestScoreForNewCustomer();
            _isAutoLogin = false;
            UIManager.Instance.OpenAuthWindow();
        }

        private IEnumerator UpdateUsernameAuth(string username)
        {
            UserProfile profile = new UserProfile() { DisplayName = username };
            Debug.Log(User);
            var profileTask = User.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(predicate: () => profileTask.IsCompleted);
            if (profileTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
            }
            else
            {
                //Auth username is now updated 
            }
        }

        private IEnumerator UpdateUsernameDatabase(string username)
        {
            var databaseTask = DataBaseReference.Child("users")
                .Child(User.UserId)
                .Child("username")
                .SetValueAsync(username);
            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
            if (databaseTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {databaseTask.Exception}");
            }
            else
            {
                //Database username is now updated
            }
        }

        private IEnumerator UpdateBestScore(int bestScore)
        {
            var databaseTask = DataBaseReference.Child("users").Child(User.UserId).Child("bestScore")
                .SetValueAsync(bestScore);
            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
            if (databaseTask.Exception != null)
            {
                Debug.LogWarning($"Failed to register task with {databaseTask.Exception}");
            }

            _isSavingData = false;
        }

        private IEnumerator LoadUserData()
        {
            var databaseTask = DataBaseReference.Child("users").Child(User.UserId).GetValueAsync();
            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
            if (databaseTask.Exception != null)
            {
                Debug.LogWarning($"Failed to register task with {databaseTask.Exception}");
            }
            else if (databaseTask.Result.Value == null)
            {
                //No Data exist
                ScoreManager.Instance.SetBestScore(0);
            }
            else
            {
                DataSnapshot dataSnapshot = databaseTask.Result;
                var bestScore = dataSnapshot.Child("bestScore").Value;
                ScoreManager.Instance.SetBestScore(int.Parse(bestScore.ToString()));
            }
        }

        private IEnumerator LoadScoreBoardData()
        {
            var databaseTask = DataBaseReference.Child("users").OrderByChild("bestScore").GetValueAsync();
            yield return new WaitUntil(predicate: () => databaseTask.IsCompleted);
            if (databaseTask.Exception != null)
            {
                Debug.LogWarning($"Failed to register task with{databaseTask.Exception}");
            }
            else
            {
                DataSnapshot dataSnapshot = databaseTask.Result;
                UIManager.Instance.DestroyAllScoreBoardItems();
                foreach (DataSnapshot childSnapshot in dataSnapshot.Children.Reverse<DataSnapshot>())
                {
                    string username = childSnapshot.Child("username").Value.ToString();
                    int bestScore = int.Parse(childSnapshot.Child("bestScore").Value.ToString());
                    UIManager.Instance.AddScoreBoardItem(username, bestScore);
                }
            }
        }
    }
}