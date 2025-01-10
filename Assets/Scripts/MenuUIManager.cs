using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    public TMP_InputField signUpLogin;
    public TMP_InputField signUpPassword;

    public TMP_InputField signInLogin;
    public TMP_InputField signInPassword;

    public LoginManager loginManager;

    public Canvas MainMenu;
    public Canvas SignUp;
    public Canvas SignIn;

    


    public async void OnSignUpValidateButton()
    {
        await loginManager.SignUpWithUsernamePasswordAsync(signUpLogin.text, signUpPassword.text);
        MainMenu.gameObject.SetActive(true);
        SignUp.gameObject.SetActive(false);
    }
    public async void OnSignInValidateButton()
    {
        await loginManager.SignInWithUsernamePasswordAsync(signInLogin.text, signInPassword.text);
        MainMenu.gameObject.SetActive(true);
        SignIn.gameObject.SetActive(false);

    }
    public void OnJouerButton()
    {
        SceneManager.LoadScene(1);
    }

}
