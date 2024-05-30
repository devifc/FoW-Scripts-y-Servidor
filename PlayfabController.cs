using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayfabController : MonoBehaviour
{
    public GameObject loginPanel, signupPanel, profilePanel, recoverPasswordPanel, recoverUserPanel, notificationPanel;
    public InputField loginEmail, loginPassword,
        signupEmail, signupPassword, signupCPassword, signupUserName,
        recoverPassEmail, recoverPassUser, recoverUserEmail,
        emailOnLogin, emailOnSignUp;
    public Text notifTitle, notifMessage, profileUserNameText;
    public Toggle rememberMeLogin, rememberMeSignup, acceptPolicy;
    [SerializeField]
    private GameManager gameManager;
    private static string playerName;
    private static bool logged=false;

    public void Start()
    {
        if (logged==true)
        {
            OpenProfilePanel();
            profileUserNameText.text = playerName;
        }
    }
    public void Update()
    {
        
    }

    #region Panels
    public void ReadPrivacyPolicy()
    {

    }
    private void EmailFromLoginToSignUp(string email, bool x)
    {
        if (x == true)
        {
            emailOnSignUp.text = "" + email;
        }
        else
        {
            emailOnLogin.text = "" + email;
        }
    }
    private void ShowNotificationMessage(string title, string message)
    {
        notifTitle.text = "" + title;
        notifMessage.text = "" + message;
        notificationPanel.SetActive(true);
    }
    public void CloseNotificationPanel()
    {
        notificationPanel.SetActive(false);
    }
    public void OpenLoginPanel(bool fromProfile) 
    {
        if (fromProfile==false) 
        {
            EmailFromLoginToSignUp(emailOnSignUp.text, false);
        }
        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        recoverPasswordPanel.SetActive(false);
        recoverUserPanel.SetActive(false);
        rememberMeLogin.enabled = false;
        rememberMeSignup.enabled = false;
    }

    public void OpenSignupPanel()
    {
        EmailFromLoginToSignUp(emailOnLogin.text, true);
        loginPanel.SetActive(false);
        signupPanel.SetActive(true);
        profilePanel.SetActive(false);
        recoverPasswordPanel.SetActive(false);
        recoverUserPanel.SetActive(false);
        rememberMeLogin.enabled = false;
        rememberMeSignup.enabled = false;
    }
    public void OpenProfilePanel()
    {
        //profileUserNameText.text = signupUserName.text;
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(true);
        recoverPasswordPanel.SetActive(false);
        recoverUserPanel.SetActive(false);
    }
    public void OpenRecoverPassPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        recoverPasswordPanel.SetActive(true);
        recoverUserPanel.SetActive(false);
    }
    public void OpenRecoverUserPanel()
    {
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        profilePanel.SetActive(false);
        recoverPasswordPanel.SetActive(false);
        recoverUserPanel.SetActive(true);
    }
    #endregion

    #region Logout, Login and Signup
    public void LogOut()
    {
        logged = false;
        profileUserNameText.text = "";
        //loginEmail.text = "";
        loginPassword.text = "";
        signupEmail.text = "";
        signupPassword.text = "";
        signupCPassword.text = "";
        signupUserName.text = "";
        playerName = "";
        OpenLoginPanel(true);
    }

    public void LoginUser() 
    {
        //fill the fields
        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text)) 
        {
            ShowNotificationMessage("FIELDS EMPTY", "Please, introduce all details.");
            return;
        }
        //if password is less than 6 message = too short
        if (loginPassword.text.Length < 6)
        {
            ShowNotificationMessage("Sorry!", "Password too short!\nIntroduce at least 6 characters.");
            return;
        }

        //Do login
        var request = new LoginWithEmailAddressRequest
        {
            Email = loginEmail.text,
            Password = loginPassword.text,

            InfoRequestParameters = new PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        string displayName = null;
        if (result.InfoResultPayload != null) 
        {
            displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        StartCoroutine(LoadNext(displayName));
    }
    IEnumerator LoadNext(string displayName) 
    {
        logged = true;
        playerName = displayName;
        profileUserNameText.text = displayName;
        ShowNotificationMessage("", "Welcome " + displayName + "!");
        yield return new WaitForSeconds(2);
        OpenProfilePanel();
        notificationPanel.SetActive(false);
    }

    public void SignUpUser() 
    {
        //username > 6
        if (signupUserName.text.Length < 6)
        {
            ShowNotificationMessage("Sorry!", "Username too short!\nIntroduce at least 6 characters.");
            return;
        }
        //username too long
        if (signupUserName.text.Length > 12)
        {
            ShowNotificationMessage("Sorry!", "Username too long!\nMax 12 characters.");
            return;
        }
        //fill the fields
        if (string.IsNullOrEmpty(signupEmail.text) && string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupCPassword.text) && string.IsNullOrEmpty(signupUserName.text))
        {
            ShowNotificationMessage("FIELDS EMPTY", "Please, introduce all details.");
            return;
        }

        //if password is less than 6 message = too short
        if (signupPassword.text.Length < 6) 
        {
            ShowNotificationMessage("Sorry!", "Password too short!\nIntroduce at least 6 characters.");
            return;
        }
        //confirm password
        if (!signupPassword.text.Equals(signupCPassword.text))
        {
            ShowNotificationMessage("Sorry!", "The password confirmation is not the same.");
            return;
        }
        //accept policy
        if (!acceptPolicy.isOn)
        {
            ShowNotificationMessage("Sorry!", "You need to accept our policy.");
            return;
        }

        //Do SignUp
        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = signupUserName.text,
            Username = signupUserName.text,
            Email = signupEmail.text,
            Password = signupPassword.text,

            RequireBothUsernameAndEmail = true,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignUpSuccess, OnError);
        
    }

    private void OnSignUpSuccess(RegisterPlayFabUserResult result)
    {
        //ShowNotificationMessage("Welcome!", "Your account have been created successfuly.");
        StartCoroutine(LoadNext(signupUserName.text));
        //OpenProfilePanel();
    }
    #endregion

    private void OnError(PlayFabError error)
    {
        ShowNotificationMessage("Sorry!", error.ErrorMessage);
        Debug.Log(error.GenerateErrorReport());
    }

    #region Recover Access
    public void RecoverPass()
    {
        if (string.IsNullOrEmpty(recoverPassEmail.text) && string.IsNullOrEmpty(recoverPassUser.text))
        {
            ShowNotificationMessage("EMAIL OR USER EMPTY", "Please, introduce your email and your user.");
            return;
        }
        //Recover password
        
    }
    public void RecoverUser()
    {
        if (string.IsNullOrEmpty(recoverUserEmail.text))
        {
            ShowNotificationMessage("EMAIL EMPTY","Please, introduce your email.");
            return;
        }
        //Recover user
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = recoverUserEmail.text,
            TitleId = "E46C1",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySuccess, OnError);
    }

    private void OnRecoverySuccess(SendAccountRecoveryEmailResult result)
    {
        OpenLoginPanel(false);
        ShowNotificationMessage("Success!","Recovery mail sent.");
    }
    #endregion

    #region Scenes
    public void playPVP()
    {
        //move the playerName to the GameManager object for have it on the next Game Scene
        //if (gameManager != null)
        //{
            gameManager.playerName = profileUserNameText.text;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
    }
    public void openDB()
    {
        //if (gameManager != null)
        //{
            if (logged == true)
            {
                gameManager.playerName = profileUserNameText.text;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        //}
    }
    public void openUserInterface()
    {
        //if (gameManager != null)
        //{
            //as Logged
            if (logged == true)
            {
                gameManager.playerName = profileUserNameText.text;
            }
            //as NO logged
            SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(0).buildIndex + 1);
        //}
    }
    #endregion

}
