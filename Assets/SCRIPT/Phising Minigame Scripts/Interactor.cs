    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactionLayer;

    private Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numColliders;

    private void Update()
    {
        _numColliders = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactPointRadius, _colliders, _interactionLayer);

        if (_numColliders > 0)
        {
            var Interactable = _colliders[0].GetComponent<Iinteractable>();

            if(Interactable != null && Input.GetKeyDown(KeyCode.E))
            {
                Interactable.Interact(this);
            }
        }
    }
}
