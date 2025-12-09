using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 家具のオーナー
/// </summary>
public abstract class FurnitureOwner : MonoBehaviour
{
    [Header("表示するキー（例：E, F, Q, LMB）")]
    public string m_KeyHint = "Q";

    [Header("どのキーで実行する家具か")]
    public UseKey m_UseKey = UseKey.E;

    /// <summary>
    /// Interactで他の家具を制御
    /// </summary>
    public abstract void Interact();

    /// <summary>
    /// 使用するKeyの名前を設定
    /// </summary>
    public enum UseKey
    {
        LeftClick,
        RightClick,
        E,
        F,
        Q
    }
}
