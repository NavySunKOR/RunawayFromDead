using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchTrigger : MonoBehaviour {

    private AIStatus status;
    private SphereCollider[] punches;

    private void Start()
    {
        status = GetComponentInParent<AIStatus>();
        punches = GetComponents<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats stats = other.transform.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.TookDamage(status.damage);
            foreach (SphereCollider coll in punches)
            {
                coll.enabled = false;
            }
        }
    }
}
