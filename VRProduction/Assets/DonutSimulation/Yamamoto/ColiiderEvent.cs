using UnityEngine;
using UnityEngine.Events;

public class ColiiderEvent : MonoBehaviour
{
   private UnityEvent<Collider> m_triggerEnter=new UnityEvent<Collider>();
    public UnityEvent<Collider> triggerEnter {  get { return m_triggerEnter; } }

    private UnityEvent<Collider> m_triggerStay=new UnityEvent<Collider>();
    public UnityEvent<Collider>triggerStay { get { return m_triggerStay; } }

    private UnityEvent<Collider>m_triggerExit=new UnityEvent<Collider>();
    public UnityEvent<Collider>triggerExit { get { return m_triggerExit; } }

    private void OnTriggerEnter(Collider other)
    {
        m_triggerEnter.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        m_triggerExit.Invoke(other);
    }
    private void OnTriggerStay(Collider other)
    {
        m_triggerStay.Invoke(other);
    }
}
