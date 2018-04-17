using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface IUserAction                              
{

    void Hit(Vector3 pos);

    int GetScore();

    void GameOver();

    void Restart();

    void StartGame();
}
public enum SSActionEventType : int { Started, Competeted }

public interface ISceneController
{
    void LoadResources();
}

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}