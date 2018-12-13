using UnityEngine;
using System.Collections;

public class CamTarget : MonoBehaviour
{
    public Transform target;
    float camSpeed = 5.0f;
    private Vector3 lerpPos;

    void LateUpdate() 
	{
		//transform.position = target.position;
		lerpPos = (target.position-transform.position)* Time.deltaTime * camSpeed;
		transform.position += lerpPos;
	}


    // Use this for initialization
    /*void Start()
    {
        lerpPos = transform.position - target.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {    //LateUpdate在其内部代码执行完毕后再执行
        transform.position = target.transform.position + lerpPos;
    }*/
}