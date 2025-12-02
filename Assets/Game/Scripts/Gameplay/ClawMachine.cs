using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ClawMachine : MonoBehaviour
{
    [SerializeField] Transform rootXBar;
    [SerializeField] Transform rootZBar;
    [SerializeField] Transform rootClawBase;
    [SerializeField] Transform rootClawArm;
    [SerializeField] Transform rootFingerParent;
    [SerializeField] Transform[] rootClawFingers;

    public ClawMachineGrabArea grabArea;
    public Transform rootGrabbedPrize;
    Prize grabbedPrize;

    [Header("Config")]
    [SerializeField] Vector3 minPos;
    [SerializeField] Vector3 maxPos;
    [SerializeField] float moveSpeed;
    [SerializeField] float yMoveSpeed;
    Vector3 moveDirection;

    StageManager stageManager;
    FloatingJoystick joystick;

    Vector3 clawOpenAngle;


    bool isGrabSequence;

    public void Init(StageManager inStageManager)
    {
        stageManager = inStageManager;
        grabArea.Init(this);
        joystick = stageManager.joystick;

        clawOpenAngle = new Vector3(-45, 0, 0);

        Vector3 startPos = new Vector3(minPos.x, rootClawBase.position.y, minPos.z);
        SetPos(startPos);
    }

    public void DoUpdate()
    {
        if (!isGrabSequence)
        {
            moveDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);

            if (moveDirection != Vector3.zero)
            {
                float dt = Time.deltaTime;

                Vector3 pos = rootClawBase.localPosition;


                pos += moveDirection * moveSpeed * dt;

                pos.x = Mathf.Clamp(pos.x, minPos.x, maxPos.x);
                pos.z = Mathf.Clamp(pos.z, minPos.z, maxPos.z);

                SetPos(pos);
            }
            
            if (Input.GetKeyDown(KeyCode.Z)) OpenClaw();
            if (Input.GetKeyDown(KeyCode.X)) CloseClaw();
        }
    }

    void SetPos(Vector3 pos)
    {
        rootClawBase.localPosition = pos;
        rootXBar.localPosition = new Vector3(pos.x, rootXBar.localPosition.y, rootXBar.localPosition.z);
        rootZBar.localPosition = new Vector3(rootZBar.localPosition.x, rootZBar.localPosition.y, pos.z);
    }

    public void OnClickGrab()
    {
        if (!isGrabSequence)
        {
            isGrabSequence = true;
            StartCoroutine(GrabSequence());
        }
    }

    IEnumerator GrabSequence()
    {
        //OpenClaw
        //Down
        //Grab-CloseClaw
        //Up
        //Return to origin pos
        
        OpenClaw();
        yield return new WaitForSeconds(0.3f);

        bool shouldContinue = false;

        while (!shouldContinue)
        {
            float dt = Time.deltaTime;
            rootClawArm.localPosition += -transform.up * yMoveSpeed * dt;
            
            float yArm = rootClawArm.localPosition.y;
            bool reachGround = yArm <= -0.9f;
            bool hasPrize = grabbedPrize != null;

            bool valid = reachGround || hasPrize;
            shouldContinue = valid;
            yield return null;
        }

        CloseClaw();
        yield return new WaitForSeconds(0.2f);

        shouldContinue = false;
        while (!shouldContinue)
        {
            float dt = Time.deltaTime;
            rootClawArm.localPosition += transform.up * yMoveSpeed * dt;

            float yArm = rootClawArm.localPosition.y;
            bool reachTop = yArm >= 0f;
            shouldContinue = reachTop;
            yield return null;
        }

        rootClawArm.localPosition = Vector3.zero;

        float distance = Vector3.Distance(minPos, rootClawBase.localPosition);
        float tweenDuration = (distance / moveSpeed) / 2f;

        Vector3 clawBaseFinalPos = new Vector3(minPos.x, rootClawBase.localPosition.y, minPos.z);
        Vector3 xBarFinalPos = new Vector3(minPos.x, rootXBar.localPosition.y, rootXBar.localPosition.z);
        Vector3 zBarFinalPos = new Vector3(rootZBar.localPosition.x, rootZBar.localPosition.y, minPos.z);

        Sequence seq = DOTween.Sequence();

        Tween doMoveClawBase = rootClawBase.DOLocalMove(clawBaseFinalPos, tweenDuration).SetEase(Ease.Linear);
        Tween doMoveXBar = rootXBar.DOLocalMove(xBarFinalPos, tweenDuration).SetEase(Ease.Linear);
        Tween doMoveZBar = rootZBar.DOLocalMove(zBarFinalPos, tweenDuration).SetEase(Ease.Linear);

        seq.Append(doMoveClawBase);
        seq.Join(doMoveXBar);
        seq.Join(doMoveZBar);

        yield return new WaitForSeconds(tweenDuration);

        if(grabbedPrize != null)
        {
            DropPrize();
            yield return new WaitForSeconds(0.5f);

            CloseClaw();
        }
        
        isGrabSequence = false;
    }

    public void GrabPrize(GameObject prize)
    {
        grabbedPrize = prize.GetComponent<Prize>();
        grabbedPrize.transform.parent = rootGrabbedPrize;
        grabbedPrize.SetPhysics(false);
    }

    public void DropPrize()
    {
        //do drop here
        OpenClaw();
        grabbedPrize.transform.parent = stageManager.prizeFactory.transform;
        grabbedPrize.SetPhysics(true);
        grabbedPrize = null;
    }

    void OpenClaw()
    {
        foreach (Transform t in rootClawFingers)
        {
            t.DOLocalRotate(clawOpenAngle, 0.5f).SetEase(Ease.Linear);
        }
    }

    void CloseClaw()
    {
        foreach (Transform t in rootClawFingers)
        {
            t.DOLocalRotate(Vector3.zero, 0.5f).SetEase(Ease.Linear);
        }
    }
}
