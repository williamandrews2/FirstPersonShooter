using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    SpawnPoint[] spawnpoints;

    void Awake()
    {       
        Instance = this;
        spawnpoints = GetComponentsInChildren<SpawnPoint>();          
    }

    public Transform GetInitialSpawnpoint()
    {
        // Set the spawn to a random choice from the spawnpoints array.
        return spawnpoints[Random.Range(0, spawnpoints.Length)].transform; 
    }

    public Transform GetRespawnpoint()
    {
        SpawnPoint farthestSpawnpoint = null;
        float maxDistance = float.MinValue;

        foreach (var spawnPoint in spawnpoints)
        {
            float minDistanceToPlayer = float.MaxValue;

            foreach(var player in PhotonNetwork.PlayerList) 
            {
                GameObject playerGameObject = GetPlayerGameObject(player);
                float dist = Vector3.Distance(spawnPoint.transform.position, playerGameObject.transform.position);

                if(dist < minDistanceToPlayer)
                {
                    minDistanceToPlayer = dist;
                }
            }

            if(minDistanceToPlayer > maxDistance)
            {
                maxDistance = minDistanceToPlayer;
                farthestSpawnpoint = spawnPoint;
            }
        }
          
        return farthestSpawnpoint.transform;
    }

    private GameObject GetPlayerGameObject(Player player)
    {
        foreach(GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            PhotonView PV = go.GetComponent<PhotonView>();
            if(PV != null && PV.Owner == player)
            {
                // Return the GameObject if it is found.
                return go;
            }            
        }
        return null;
    }
}
