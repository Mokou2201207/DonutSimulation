using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// オーダーは何個あるか
/// </summary>

[CreateAssetMenu(menuName ="Order/OrderData")]
public class OrderData : ScriptableObject
{
    public List<OrderCondition> conditions;
}
