using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Spawner : NetworkBehaviour
{
 //   [SerializeField] public GameObject spawnee;

//
//  [SerializeField] public GameObject gZone;
//  [SerializeField] public GameObject bZone;
//  [SerializeField] public GameObject yZone;
//  [SerializeField] public GameObject rZone;
//
//  private Transform gz1;
//  private Transform gz2;
//  private Transform gz3;
//  private Transform gz4;
//                     
//  private Transform bz1;
//  private Transform bz2;
//  private Transform bz3;
//  private Transform bz4;
//
//  private Transform yz1;
//  private Transform yz2;
//  private Transform yz3;
//  private Transform yz4;
//                 
//  private Transform rz1;
//  private Transform rz2;
//  private Transform rz3;
//  private Transform rz4;
//
//  private GameObject[] zoneList = new GameObject[4];
//
//  Random rng = new Random();
//
//  private int random;
//  
//
//
//
//  public void Start()
//  {
//
//   zoneList[0] = gZone;
//   zoneList[1] = bZone;
//   zoneList[2] = yZone;
//   zoneList[3] = rZone;
//
//    gZone.GetComponent<ZoneBehaviour>().setValues();
//    bZone.GetComponent<ZoneBehaviour>().setValues();
//    rZone.GetComponent<ZoneBehaviour>().setValues();
//    yZone.GetComponent<ZoneBehaviour>().setValues();
//
//   gz1 = gZone.GetComponent<ZoneBehaviour>().getStartPoints(0);
//   gz2 = gZone.GetComponent<ZoneBehaviour>().getStartPoints(1);
//   gz3 = gZone.GetComponent<ZoneBehaviour>().getStartPoints(2);
//   gz4 = gZone.GetComponent<ZoneBehaviour>().getStartPoints(3);
// 
//   bz1 =  bZone.GetComponent<ZoneBehaviour>().getStartPoints(0);
//   bz2 =  bZone.GetComponent<ZoneBehaviour>().getStartPoints(1);
//   bz3 =  bZone.GetComponent<ZoneBehaviour>().getStartPoints(2);
//   bz4 =  bZone.GetComponent<ZoneBehaviour>().getStartPoints(3);
//
//
//   NetworkServer.Spawn(Instantiate(spawnee, gz1.position, gz1.rotation));
//   NetworkServer.Spawn(Instantiate(spawnee, gz2.position, gz2.rotation));
//   NetworkServer.Spawn(Instantiate(spawnee, gz3.position, gz3.rotation));
//   NetworkServer.Spawn(Instantiate(spawnee, gz4.position, gz4.rotation));
//
//   NetworkServer.Spawn(Instantiate(spawnee, bz1.position, gz1.rotation));
//   NetworkServer.Spawn(Instantiate(spawnee, bz2.position, gz2.rotation));
//   NetworkServer.Spawn(Instantiate(spawnee, bz3.position, gz3.rotation));
//   NetworkServer.Spawn(Instantiate(spawnee, bz4.position, gz4.rotation));
//  }
//
 
}
