using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : Item
{
    public abstract override void Use();

    // Must be available for all guns.
    public GameObject bulletImpactPrefab;
}
