using Michsky.MUIP;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("----------Login Credentials----------")]
    [SerializeField] private string userName;
    [SerializeField] private string password;
    // Regular expression emailPattern for email validation
    [SerializeField] private string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

    [Header("----------Panels----------")]
    [SerializeField] private GameObject signUpUI;
    [SerializeField] private GameObject signInUI;
    [SerializeField] private GameObject signOutUI;
    [SerializeField] private GameObject exploreCategoriesUI;
    [SerializeField] private GameObject interiorObjectsUI;

    [Header("----------Buttons----------")]
    [SerializeField] private ButtonManager signUpBtn;
    [SerializeField] private ButtonManager signInBtn;
    [SerializeField] private ButtonManager signOutBtn;

    [Header("Toggle")]
    [SerializeField] private Toggle termAndPolicies;

    [Header("----------InputFields----------")]
    [SerializeField] private TMP_InputField[] signUpInputFlds;
    [SerializeField] private TMP_InputField[] signInInputFlds;

    [Header("----------Scripts----------")]
    [SerializeField] private NotifierManager notifierManager;

    private void Reset()
    {
        signUpUI = GameObject.Find("SignUpPanel");
        signInUI = GameObject.Find("SignInPanel");
        signOutUI = GameObject.Find("CameraViewPanel");
        exploreCategoriesUI = GameObject.Find("ExploreCategoriesPanel");
        interiorObjectsUI = GameObject.Find("InteriorObjectsPanel");

        signUpInputFlds = signUpUI.GetComponentsInChildren<TMP_InputField>();
        signInInputFlds = signInUI.GetComponentsInChildren<TMP_InputField>();

        termAndPolicies = FindObjectOfType<CustomToggle>().gameObject.GetComponent<Toggle>();

        signUpBtn = signUpUI.GetComponentInChildren<ButtonManager>();
        signInBtn = signInUI.GetComponentInChildren<ButtonManager>();
        signOutBtn = signOutUI.GetComponentInChildren<ButtonManager>();

        notifierManager = FindObjectOfType<NotifierManager>();
    }

    private void Start()
    {
        signUpBtn.onClick.AddListener(() => SignUpUser());
        signInBtn.onClick.AddListener(() => SignInUser());
        signOutBtn.onClick.AddListener(() => SignOutUser());
        string userNamePref = PlayerPrefs.GetString("userNamePref");
        string passwordPref = PlayerPrefs.GetString("passwordPref");
        if (!string.IsNullOrEmpty(userNamePref) && !string.IsNullOrEmpty(passwordPref))
        {
            Debug.Log("UserPref: " + userNamePref);
            Debug.Log("PassPref: " + password);
            userName = userNamePref;
            password = passwordPref;
            //signUpInputFlds[0].text = userNamePref;
            //signUpInputFlds[1].text = passwordPref;
        }
    }

    private void SignUpUser()
    {
        //Debug.Log("SignUp Clicked");
        //---Check for empty fields---
        if (!CheckNullFields(signUpInputFlds)) { return; }
        
        //---Check for email pattern---
        //if (!CheckEmailAddress(signUpInputFlds[0])) { return; }

        //---Check for Passwords Match---
        if (!ComparePasswords(signUpInputFlds)) { return; }

        //---Check Terms are Agreed---
        if (!CheckTerms(termAndPolicies)) { return; }


        // Do SignUp
        userName = signUpInputFlds[0].text;
        password = signUpInputFlds[1].text;

        PlayerPrefs.SetString("userNamePref", userName);
        PlayerPrefs.SetString("passwordPref", password);

        signUpUI.SetActive(false);
        signInUI.SetActive(true);
    }

    private void SignInUser()
    {
        //Debug.Log("SignInClicked!");
        //---Check for empty fields---
        if (!CheckNullFields(signInInputFlds)) { return; }
        //Debug.Log("Input userName: " + signInInputFlds[0].text + "\n saved UserName: " + userName);
        //Debug.Log("Input password: " + signInInputFlds[1].text + "\n saved Password: " + password);

        //---Validate Email and Password---
        if (!CheckUserNameOrPassword(signInInputFlds)) { return; }

        //---Do SignIn---
        //Debug.Log("Signing In!");
        signInUI.SetActive(false);
        exploreCategoriesUI.SetActive(true);
    }

    private void SignOutUser()
    {
        notifierManager.Notify(notifierManager.typesOfNotification[4]);
        signOutUI.SetActive(false);
        signInUI.SetActive(true);
    }

    private bool CheckNullFields(TMP_InputField[] fieldsFor)
    {
        foreach (var obj in fieldsFor)
        {
            if (string.IsNullOrEmpty(obj.text))
            {
                //Debug.Log("fields are empty");
                notifierManager.Notify(notifierManager.typesOfNotification[0]);
                return false;
            }
        }
        return true;
    }

    private bool CheckEmailAddress(TMP_InputField field)
    {
        string email = field.text;
        //---Check if the email matches the pattern---
        bool isEmailValid = Regex.IsMatch(email, emailPattern);
        notifierManager.Notify(notifierManager.typesOfNotification[2]);
        return isEmailValid;
    }

    private bool ComparePasswords(TMP_InputField[] fields)
    {
        if (fields[1].text != fields[2].text)
        {
            Debug.Log("Passwords Doesnt match!");
            notifierManager.Notify(notifierManager.typesOfNotification[3]);
            signUpInputFlds[1].text = "";
            signUpInputFlds[2].text = "";
            return false;
        }
        return true;
    }

    private bool CheckTerms(Toggle terms)
    {
        if (!terms.isOn)
        {
            Debug.Log("Term are not agreed");
            notifierManager.Notify(notifierManager.typesOfNotification[1]);
            return false;
        }
        return true;
    }

    private bool CheckUserNameOrPassword(TMP_InputField[] fields)
    {
        if (fields[0].text != userName || fields[1].text != password)
        {
            Debug.Log("Username or Password is Incorrect");
            notifierManager.Notify(notifierManager.typesOfNotification[2]);
            return false;
        }
        return true;
    }
}
