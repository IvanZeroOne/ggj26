using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Attachable : MonoBehaviour
{
    [SerializeField] Transform _parent;

    public Transform Parent => _parent;
}