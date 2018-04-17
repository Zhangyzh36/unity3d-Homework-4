using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyActionManager : SSActionManager
{

    public DiskFlyAction fly;                            
    public FirstController scene_controller;             

    protected void Start()
    {
        scene_controller = (FirstController)SSDirector.GetInstance().currentScenceController;
        scene_controller.actionManager = this;     
    }
   
    public void DiskFly(GameObject disk, float angle, float power)
    {
        fly = DiskFlyAction.GetSSAction(disk.GetComponent<DiskData>().direction, angle, power);
        this.RunAction(disk, fly, this);
    }
}
