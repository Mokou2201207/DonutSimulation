using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public Image m_Image;
    public Text m_KeyText; // UIのテキスト
    public void SetKey(string keyName)
    {
        m_KeyText.text = keyName;
    }
}
