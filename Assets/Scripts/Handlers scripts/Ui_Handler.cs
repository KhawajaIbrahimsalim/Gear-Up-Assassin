using TMPro;
using UnityEngine;

public class Ui_Handler : MonoBehaviour
{
    [Header("Play Mode UI")]
    public TMP_Text Gun_name;

    [Header("UI anime fields"), SerializeField, Range(0.01f, 0.1f)]
    private float fade_out_rate;

    public void Text_fade_out(TMP_Text text)
    {
        if (text)
        {
            if (text.alpha > 0)
            text.alpha -= fade_out_rate;
        }
    }
    public void Text_in(TMP_Text text)
    {
        if (text)
        {
            text.alpha = 1;
        }
    }
}
