using UnityEngine;
using TMPro;

public class introPasswordVisibilityScript : MonoBehaviour
{
    bool isPasswordHidden = true;

    [SerializeField]
    private TMP_InputField _myPasswordField;

    public void togglePasswordVisibility()
    {
        if(isPasswordHidden)
        {
            _myPasswordField.contentType = TMP_InputField.ContentType.Standard;
            _myPasswordField.Select();
            isPasswordHidden = false;
        }
        else
        {
            _myPasswordField.contentType = TMP_InputField.ContentType.Password;
            _myPasswordField.Select();
            isPasswordHidden = true;
        }
    }
    
}
