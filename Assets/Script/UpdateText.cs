using TMPro;
using UnityEngine;

public class UpdateText : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void setup(string setuptext)
    {
        text.text = setuptext;
    }

}
