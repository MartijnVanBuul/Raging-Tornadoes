using UnityEngine;
using System.Collections;

public class HandCollider : MonoBehaviour
{

    public delegate void gTriggerFunction(Collider aCollider);
    public gTriggerFunction gTriggerEnter = delegate { };
    public gTriggerFunction gTriggerExit = delegate { };
    public LayerMask targetLayer;
    
    void OnTriggerEnter(Collider aCollider)
    {
        if (GameUtility.IsInLayerMask(aCollider.gameObject, targetLayer))
        {
            gTriggerEnter(aCollider);
        }
    }

    void OnTriggerExit(Collider aCollider)
    {
        if (GameUtility.IsInLayerMask(aCollider.gameObject, targetLayer))
        {
            gTriggerExit(aCollider);
        }
    }
}
