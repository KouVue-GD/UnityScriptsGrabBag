using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    enum StartPoint{
        startTarget, startPos
    }
    enum EndPoint{
        endTarget, endMouse
    }

    [SerializeField] StartPoint startPoint;
    [SerializeField] EndPoint endPoint;

    [SerializeField] Transform startPos;
    [SerializeField] GameObject startTarget;

    [SerializeField] GameObject endTarget;

    LineRenderer lineRenderer;

    Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        if(lineRenderer == null){
            throw new MissingComponentException("Missing Line Renderer in LineRendererController.cs");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(startPoint == StartPoint.startPos){
            SetStartPoint(startPos);
        }

        if(startPoint == StartPoint.startTarget){
            SetStartPoint(startTarget);
        }


        if(endPoint == EndPoint.endTarget){
            SetEndPointToTarget(endTarget);
        }

        if(endPoint == EndPoint.endMouse){
            SetEndPointToMouse();
        }
    }

    void SetStartPoint(Transform passedTransform){
        lineRenderer.SetPosition(0, startPos.position);
    }

    void SetStartPoint(GameObject passedTarget){
        lineRenderer.SetPosition(0, startTarget.transform.position);
    }

    void SetEndPointToTarget(GameObject passedTarget){
        lineRenderer.SetPosition(1, passedTarget.transform.position);
    }

    void SetEndPointToMouse(){
        lineRenderer.SetPosition(1, mainCam.ScreenToWorldPoint(Input.mousePosition));
    }
}
