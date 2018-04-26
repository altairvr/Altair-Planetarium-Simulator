using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial_Button : MonoBehaviour
{

    private string Name;
    public Text ButtonText;
    public Tutorial_ScrollView ScrollView;

    public void SetName(string name)
    {
        Name = name;
        ButtonText.text = name;
    }
    public void Button_Click()
    {
        ScrollView.ButtonClicked(Name);
    }
}