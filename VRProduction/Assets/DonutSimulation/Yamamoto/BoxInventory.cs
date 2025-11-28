using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BoxInventory : MonoBehaviour
{
    public int stock = 10;          // 初期の在庫数
    public GameObject itemPrefab;   // 取り出されるアイテム
    public Transform spawnPoint;    // アイテムが出てくる位置
    public ColiiderEvent searchCol;
    private Hand hand;
    private PlayerInput input;
    private InputAction gripAction;
    private void Awake()
    {
        searchCol.triggerEnter.AddListener(OnSearchTriggerEnter);
        searchCol.triggerExit.AddListener(OnSearchTriggerExit);
        input=GetComponent<PlayerInput>();
    }
    private void OnSearchTriggerEnter(Collider other)
    {
        Hand hitHand = other.GetComponent<Hand>();
        if( hitHand != null )
        {
            hand = hitHand;
            bool left = hand.direction==HandDirection.Left;
            input.SwitchCurrentActionMap(left ? "InputL" : "InputR");
            gripAction = input.currentActionMap.FindAction("Grip");
        }
       
    }
    private void OnSearchTriggerExit(Collider other)
    {
        Hand hitHand = other.GetComponent<Hand>();
        if (hitHand != null)
        {
            if (hand == hitHand)
            {
                hand = null;
            }
        }

    }
    // 在庫があるか？
    public bool HasStock()
    {
        return stock > 0;
    }

    // 在庫を減らす & アイテムを生成
    public GameObject TakeItem()
    {
        if (stock <= 0) return null;

        stock--;

        // 商品を生成
        GameObject item = Instantiate(itemPrefab, spawnPoint.position, spawnPoint.rotation);

        return item;
    }
    private void Update()
    {
        if(hand != null)
        {
            if(gripAction != null && gripAction.WasPressedThisFrame())
            {
                GameObject item = TakeItem();
                XRGrabInteractable interactable=item.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    InteractionManager.StartGrab(hand.NfInteractor, interactable);
                }
            }
        }
    }
}
