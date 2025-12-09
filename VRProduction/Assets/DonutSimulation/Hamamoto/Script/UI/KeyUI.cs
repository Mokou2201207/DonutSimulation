using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Key‚ÌText•\¦ˆ—
/// </summary>
public class KeyUI : MonoBehaviour
{
    [Header("Key‚Ì˜g‚ÌUI")]
    public Image m_KeyImage;

    [Header("Key‚ÌText"),SerializeField]
    private Text m_KeyText; 

    /// <summary>
    /// ‚±‚±‚ÅKey‚ÌText‚ª•Ï‚í‚é
    /// </summary>
    /// <param name="keyName">Key‚ÌText‚Ì–¼‘O</param>
    public void SetKey(string keyName)
    {
        m_KeyText.text = keyName;
    }
}
