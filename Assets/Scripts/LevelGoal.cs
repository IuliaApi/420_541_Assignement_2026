using UnityEngine;
public class LevelGoal : MonoBehaviour
{
    bool entered;
    void OnTriggerEnter(Collider other)
    {
        if (entered || other.GetComponentInParent<CharacterMovement>() == null) return;
        entered = true; GameManager.Instance.NextLevel();
    }
}
