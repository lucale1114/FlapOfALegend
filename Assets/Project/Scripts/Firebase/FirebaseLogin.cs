using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System;

public class FirebaseLogin : MonoBehaviour
{
    public event Action LoggedIn;
    public event Action Registered;

    FirebaseAuth auth;
    private TMP_InputField userField;
    private TMP_InputField passwordField;
    private TMP_InputField emailField;
    private TMP_InputField passwordField2;
    private TMP_InputField emailField2;
    private TextMeshProUGUI greetText;

    private void Awake()
    {
        userField = GameObject.Find("Username").GetComponent<TMP_InputField>();
        passwordField = GameObject.Find("Password").GetComponent<TMP_InputField>();
        emailField = GameObject.Find("Email").GetComponent<TMP_InputField>();
        passwordField2 = GameObject.Find("Password2").GetComponent<TMP_InputField>();
        emailField2 = GameObject.Find("Email2").GetComponent<TMP_InputField>();
        greetText = GameObject.Find("Greet").GetComponent<TextMeshProUGUI>();

        print(passwordField.text);

        userField.transform.parent.gameObject.SetActive(false);
        passwordField2.transform.parent.gameObject.SetActive(false);
    }

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogError(task.Exception);

            auth = FirebaseAuth.DefaultInstance;
        });

    }

    void Creatthing()
    {
        LoggedIn?.Invoke();
        //RegisterNewUser("test@gmail.com", "hashed");
        SaveToFirebase("1");
    }

    private void AnonymousSignIn()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task => {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            }
        });
    }

    public void RegisterNewUser()
    {
        Debug.Log("Starting Registration");
        auth.CreateUserWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("User Registerd: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                UserProfile profile = new UserProfile
                {
                    DisplayName = userField.text,
                    PhotoUrl = new System.Uri("https://example.com/jane-q-user/profile.jpg"),
                };
                newUser.UpdateUserProfileAsync(profile).ContinueWith(task2 => {
                    if (task2.IsCanceled)
                    {
                        Debug.LogError("UpdateUserProfileAsync was canceled.");
                        return;
                    }
                    if (task2.IsFaulted)
                    {
                        Debug.LogError("UpdateUserProfileAsync encountered an error: " + task2.Exception);
                        return;
                    }

                    Debug.Log("User profile updated successfully.");
                    Registered?.Invoke();
                });
            }
        });
    }

    public void SignIn()
    {
        auth.SignInWithEmailAndPasswordAsync(emailField2.text, passwordField2.text).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                  newUser.DisplayName, newUser.UserId);
                Invoke("Creatthing", 2);
                greetText.text = "Welcome " + FirebaseAuth.DefaultInstance.CurrentUser.DisplayName;
            }
        });
    }

    private void SaveToFirebase(string data)
    {
        var db = FirebaseDatabase.DefaultInstance;
        var userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        //puts the JSON data in the "users/userId" part of the database.
        db.RootReference.Child("users").Child(userId).Child("bodies").SetRawJsonValueAsync(data);
    }

    private void SignOut()
    {
        auth.SignOut();
        Debug.Log("User signed out");
    }
}