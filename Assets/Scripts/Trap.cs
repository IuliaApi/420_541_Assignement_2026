using UnityEngine;
public class Trap : MonoBehaviour
{
    void OnTriggerEnter(Collider other) { Hit(other); }
    void OnTriggerStay(Collider other) { Hit(other); }
    void Hit(Collider other) { var p = other.GetComponentInParent<CharacterMovement>(); if (p != null) p.Damage(); }
}
