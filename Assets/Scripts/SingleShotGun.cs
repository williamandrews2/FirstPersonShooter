using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            // Cast the itemInfo as a GunInfo since the itemInfo class does not contain the damage value.
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);

            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Debug.Log("Hit position is " + hitPosition);

        Instantiate(bulletImpactPrefab, hitPosition, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
    }
}
