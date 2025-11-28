using UnityEngine;
//家具のオーナーここから子に行く
public abstract class FurnitureOwner : MonoBehaviour
{
    [Header("表示するキー（例：E, F, Q, LMB）")]
    public string m_KeyHint = "Q";
    public UseKey useKey = UseKey.E;  // どのキーで実行する家具か
    public abstract void Interact();
    public enum UseKey
    {
        LeftClick,
        RightClick,
        E,
        F,
        Q
    }
}
