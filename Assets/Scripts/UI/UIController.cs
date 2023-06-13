using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
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
        exploreCategoriesUI = GameObject.Find("ExploreCategoriesPanel");
        interiorObjectsUI = GameObject.Find("InteriorObjectsPanel");

        signUpInputFlds = signUpUI.GetComponentsInChildren<TMP_InputField>();
        signInInputFlds = signInUI.GetComponentsInChildren<TMP_InputField>();

        termAndPolicies = FindObjectOfType<CustomToggle>().gameObject.GetComponent<Toggle>();

        signUpBtn = signUpUI.GetComponentInChildren<ButtonManager>();
        signInBtn = signInUI.GetComponentInChildren<ButtonManager>();

        notifierManager = FindObjectOfType<NotifierManager>();
    }

    private void Start()
    {
        signUpBtn.onClick.AddListener(() => SignUpUser());
        signInBtn.onClick.AddListener(() => SignInUser());
    }

    private void SignUpUser()
    {
        //Debug.Log("SignUp Clicked");
        if (!CheckFields(signUpInputFlds)) { return; }
        if (!termAndPolicies.isOn)
        {
            //Debug.Log("Term are not agreed");
            //notifierManager.Notify(notifierManager.errorTitle, "Please Agree to Terms & Conditions");
            notifierManager.Notify(notifierManager.typesOfNotification[1]);
            return;
        }

        // Do SignUp
        signUpUI.SetActive(false);
        signInUI.SetActive(true);
    }

    private void SignInUser()
    {
        if (!CheckFields(signInInputFlds)) { return; }

        // Do SignIn
        signInUI.SetActive(false);
        exploreCategoriesUI.SetActive(true);
    }

    private bool CheckFields(TMP_InputField[] fieldsFor)
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
}
