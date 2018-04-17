using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, ISceneController, IUserAction
{actionManager
    public FlyActionManager actionManager;
    public DiskFactory diskFactory;
    public UserGUI GUI;
    public ScoreRecorder scoreRecorder;

    private Queue<GameObject> diskQueue = new Queue<GameObject>();          
    private List<GameObject> notBeShoted = new List<GameObject>();     
    
    private int round = 1;                                                   
    private float speed = 1.5f;                                               
    private bool isInGame = false;                                       
    private bool isGameOver = false;                                          
    private bool isGameStart = false;

    private int s1 = 10;                                          
    private int s3 = 25;                                          

    void Start ()
    {
        SSDirector director = SSDirector.GetInstance();     
        director.currentScenceController = this;             
        diskFactory = Singleton<DiskFactory>.Instance;
        scoreRecorder = Singleton<ScoreRecorder>.Instance;
        actionManager = gameObject.AddComponent<FlyActionManager>() as FlyActionManager;
        GUI = gameObject.AddComponent<UserGUI>() as UserGUI;
    }
	
	void Update ()
    {
        if(isGameStart)
        {
            
            if (isGameOver)
            {
                CancelInvoke("LoadResources");
            }
            
            if (!isInGame)
            {
                InvokeRepeating("LoadResources", 1f, speed);
                isInGame = true;
            }
      
            SendDisk();
            if (scoreRecorder.score >= s1 && round == 1)
            {
                round = 2;
                speed = speed - 0.5f;
                CancelInvoke("LoadResources");
                isInGame = false;
            }
            else if (scoreRecorder.score >= s3 && round == 2)
            {
                round = 3;
                speed = speed - 0.5f;
                CancelInvoke("LoadResources");
                isInGame = false;
            }
        }
    }

    public void LoadResources()
    {
        diskQueue.Enqueue(diskFactory.GetDisk(round)); 
    }

    private void SendDisk()
    {
        float position_x = 16;                       
        if (diskQueue.Count != 0)
        {
            GameObject disk = diskQueue.Dequeue();
            notBeShoted.Add(disk);
            disk.SetActive(true);
       
            float randomY = Random.Range(2f, 4f);
            float offsetX = Random.Range(-1f, 1f) < 0 ? -1 : 1;
            disk.GetComponent<DiskData>().direction = new Vector3(offsetX, randomY, 0);
            Vector3 position = new Vector3(-disk.GetComponent<DiskData>().direction.x * position_x, randomY, 0);
            disk.transform.position = position;
            
            float pow = Random.Range(10f, 15f);
            float ang = Random.Range(15f, 28f);
            actionManager.UFOFly(disk,ang,pow);
        }

        for (int i = 0; i < notBeShoted.Count; i++)
        {
            GameObject temp = notBeShoted[i];
      
            if (temp.transform.position.y < -10 && temp.gameObject.activeSelf == true)
            {
                diskFactory.FreeDisk(notBeShoted[i]);
                notBeShoted.Remove(notBeShoted[i]);
                GUI.MinusOneSec();
            }
        }
    }

    public void Hit(Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        bool beHit = false;
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider.gameObject.GetComponent<DiskData>() != null)
            {
                for (int j = 0; j < notBeShoted.Count; j++)
                {
                    if (hit.collider.gameObject.GetInstanceID() == notBeShoted[j].gameObject.GetInstanceID())
                    {
                        beHit = true;
                    }
                }
                if(!beHit)
                {
                    return;
                }
                notBeShoted.Remove(hit.collider.gameObject);
                scoreRecorder.Record(hit.collider.gameObject);
               
                diskFactory.FreeDisk(hit.collider.gameObject);
                break;
            }
        }
    }
    public int GetScore()
    {
        return scoreRecorder.score;
    }
    public void Restart()
    {
        isGameOver = false;
        isInGame = false;
        scoreRecorder.score = 0;
        round = 1;
        speed = 2f;
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    public void StartGame()
    {
        isGameStart = true;
    }
}
