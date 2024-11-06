using UnityEngine;

public class ReturnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            MonsterState mon = other.GetComponent<MonsterState>();

            mon.TriggerReturn();
        }
    }
}
