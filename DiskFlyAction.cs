using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFlyAction : SSAction
{
    public float gravity = -5;                                 
    private Vector3 v1;                              
    private Vector3 v2 = Vector3.zero;             
    private float time;                                        
    private Vector3 curAngle = Vector3.zero;               

    private DiskFlyAction() { }
    public static DiskFlyAction GetSSAction(Vector3 direction, float angle, float power)
    {
  
        DiskFlyAction action = CreateInstance<DiskFlyAction>();
        if (direction.x == -1)
        {
            action.v1 = Quaternion.Euler(new Vector3(0, 0, -angle)) * Vector3.left * power;
        }
        else
        {
            action.v1 = Quaternion.Euler(new Vector3(0, 0, angle)) * Vector3.right * power;
        }
        return action;
    }

    public override void Update()
    {
        time += Time.fixedDeltaTime;
        v2.y = gravity * time;

        transform.position += (v1 + v2) * Time.fixedDeltaTime;
        curAngle.z = Mathf.Atan((v1.y + v2.y) / v1.x) * Mathf.Rad2Deg;
        transform.eulerAngles = curAngle;

        if (this.transform.position.y < -10)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this);      
        }
    }

    public override void Start() { }
}
