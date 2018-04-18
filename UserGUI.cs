using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour
{
    private IUserAction action;

    public int XP = 3;                   
    GUIStyle style1 = new GUIStyle();
    GUIStyle style2 = new GUIStyle();
    GUIStyle style3 = new GUIStyle();
    private int highestScore = 0;            
    private bool isGameStart = false;      

    void Start ()
    {
        action = SSDirector.GetInstance().currentScenceController as IUserAction;
    }
	
	void OnGUI ()
    {
        style2.fontSize = 30;
        style1.fontSize = 30;
        style3.fontSize = 30;

        if (isGameStart)
        {
            
            if (Input.GetButtonDown("Fire1"))
            {
                Vector3 pos = Input.mousePosition;
                action.Hit(pos);
            }

            GUI.Label(new Rect(700, 10, 200, 50), "分数:", style2);
            GUI.Label(new Rect(780, 10, 200, 50), action.GetScore().ToString(), style1);
  
            if (XP == 0)
            {
                highestScore = highestScore > action.GetScore() ? highestScore : action.GetScore();
                GUI.Label(new Rect(700, 400, 100, 100), "游戏结束", style3);
                GUI.Label(new Rect(700, 500, 50, 50), "得分:", style2);
                GUI.Label(new Rect(820, 500, 50, 50), highestScore.ToString(), style2);
                if (GUI.Button(new Rect(Screen.width / 2 - 20, Screen.width / 2 - 150, 100, 50), "重新开始"))
                {
                    XP = 3;
                    action.Restart();
                    return;
                }
                action.GameOver();
            }
        }
        else
        { 
            if (GUI.Button(new Rect(Screen.width / 2 - 20, Screen.width / 2-150, 100, 50), "游戏开始"))
            {
                isGameStart = true;
                action.StartGame();
            }
        }
    }
    public void MinusOneSec()
    {
        if(XP > 0)
            XP--;
    }
}
