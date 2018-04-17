using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour
{
    public GameObject diskPrefab = null;        
    
    private List<DiskData> used = new List<DiskData>();   
    private List<DiskData> free = new List<DiskData>();   

    public GameObject GetDisk(int round)
    {          
        float offsetY = -10f;                             
        string tag;
        diskPrefab = null;

        int rand = Random(0, 3);
        if (rand == 1)
            tag = "disk1";
         else if (rand == 2)
            tag = "disk2";
         else 
            tag = "disk3";
       

        for(int i= free.Count - 1; i >= 0;i--)
        {
            if(free[i].tag == tag)
            {
                diskPrefab = free[i].gameObject;
                free.Remove(free[i]);
                break;
            }
        }
        
        if(diskPrefab == null)
        {
           
            diskPrefab = Instantiate(Resources.Load<GameObject>("Prefabs/" + tag), new Vector3(0, offsetY, 0), Quaternion.identity);
            
            float offsetX = Random.Range(-1f, 1f) < 0 ? -1 : 1;
            diskPrefab.GetComponent<Renderer>().material.color = diskPrefab.GetComponent<DiskData>().color;
            diskPrefab.GetComponent<DiskData>().direction = new Vector3(offsetX, offsetY, 0);
            diskPrefab.transform.localScale = diskPrefab.GetComponent<DiskData>().scale;
        }
     
        used.Add(diskPrefab.GetComponent<DiskData>());
        return diskPrefab;
    }

 
    public void FreeDisk(GameObject disk)
    {
        for(int i = 0;i < used.Count; i++)
        {
            if (disk.GetInstanceID() == used[i].gameObject.GetInstanceID())
            {
                used[i].gameObject.SetActive(false);
                free.Add(used[i]);
                used.Remove(used[i]);
                break;
            }
        }
    }
}
