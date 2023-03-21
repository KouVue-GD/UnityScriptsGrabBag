using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextButtonChange : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] string defaultString = "Default String";

    public void ChangeText(){
        text.text = defaultString;
    }

    public void ChangeText(string passedString){
        text.text = passedString;
    }
}
