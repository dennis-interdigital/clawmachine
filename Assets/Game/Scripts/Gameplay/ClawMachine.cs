using DG.Tweening;
using GogoGaga.OptimizedRopesAndCables;
using System.Collections;
using UnityEngine;

public class ClawMachine : MonoBehaviour
{
    [SerializeField] Transform rootXBar;
    [SerializeField] Transform rootZBar;
    [SerializeField] Transform rootClawBase;
    [SerializeField] Transform rootClawArm;
    [SerializeField] Transform[] rootClawFingers;
    [SerializeField] Transform rootMin;
    [SerializeField] Transform rootMax;
    [SerializeField] Transform rootStartPos;
    [SerializeField] RopeMesh ropeMesh;

    public ClawMachineGrabArea grabArea;
    public Transform rootGrabbedPrize;

    [HideInInspector] public Prize grabbedPrize;

    [Header("Config")]
    [SerializeField] float moveSpeed;
    [SerializeField] float yMoveSpeed;
    [SerializeField] float ropeMinLength = 4f;
    [SerializeField] float ropeMaxLength = 8.5f;

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

        clawOpenAngle = new Vector3(0, -20, 0);

        Vector3 startPos = rootStartPos.localPosition;
        SetPos(startPos);

        ropeMesh.GetRopeScript().ropeLength = ropeMinLength;
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

                float minX = rootMin.localPosition.x;
                float minZ = rootMin.localPosition.z;
                float maxX = rootMax.localPosition.x;
                float maxZ = rootMax.localPosition.z;

                pos.x = Mathf.Clamp(pos.x, minX, maxX);
                pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

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

    public void StartGrabSequence(bool success)
    {
        if (!isGrabSequence)
        {
            isGrabSequence = true;
            StartCoroutine(GrabSequence(success));
        }
    }

    IEnumerator GrabSequence(bool success)
    {
        OpenClaw();
        yield return new WaitForSeconds(0.3f);

        grabArea.SetColliderEnable(true);

        bool shouldContinue = false;
        while (!shouldContinue)
        {
            float dt = Time.deltaTime;
            var rope = ropeMesh.GetRopeScript();

            rope.ropeLength += yMoveSpeed * dt;
            rope.ropeLength = Mathf.Min(rope.ropeLength, ropeMaxLength);

            bool reachBottom = rope.ropeLength >= ropeMaxLength;
            bool hasPrize = grabbedPrize != null;

            shouldContinue = reachBottom || hasPrize;
            yield return null;
        }

        CloseClaw();
        yield return new WaitForSeconds(0.2f);

        shouldContinue = false;
        while (!shouldContinue)
        {
            float dt = Time.deltaTime;
            var rope = ropeMesh.GetRopeScript();

            rope.ropeLength -= yMoveSpeed * dt;
            rope.ropeLength = Mathf.Max(rope.ropeLength, ropeMinLength);

            bool reachTop = rope.ropeLength <= ropeMinLength;
            shouldContinue = reachTop;
            yield return null;
        }

        ropeMesh.GetRopeScript().ropeLength = ropeMinLength;

        float distance = Vector3.Distance(rootMin.localPosition, rootClawBase.localPosition);
        float tweenDuration = (distance / moveSpeed);

        float minX = rootMin.localPosition.x;
        float minZ = rootMin.localPosition.z;

        Vector3 clawBaseFinalPos = rootStartPos.localPosition;
        Vector3 xBarFinalPos = new Vector3(minX, rootXBar.localPosition.y, rootXBar.localPosition.z);
        Vector3 zBarFinalPos = new Vector3(rootZBar.localPosition.x, rootZBar.localPosition.y, minZ);

        Sequence seq = DOTween.Sequence();

        Tween doMoveClawBase = rootClawBase.DOLocalMove(clawBaseFinalPos, tweenDuration).SetEase(Ease.Linear);
        Tween doMoveXBar = rootXBar.DOLocalMove(xBarFinalPos, tweenDuration).SetEase(Ease.Linear);
        Tween doMoveZBar = rootZBar.DOLocalMove(zBarFinalPos, tweenDuration).SetEase(Ease.Linear);

        seq.Append(doMoveClawBase);
        seq.Join(doMoveXBar);
        seq.Join(doMoveZBar);

        yield return new WaitForSeconds(tweenDuration);

        if (grabbedPrize != null)
        {
            DropPrize();
            yield return new WaitForSeconds(0.5f);
            ResetClawRotation();
        }

        grabArea.SetColliderEnable(false);
        isGrabSequence = false;
    }

    public void GrabPrize(GameObject prize)
    {
        grabbedPrize = prize.GetComponent<Prize>();
        grabbedPrize.transform.parent = rootGrabbedPrize;
        //var rootPos = rootGrabbedPrize.localPosition;
        //Vector3 newPos = new Vector3(rootPos.x, grabbedPrize.transform.localPosition.y, rootPos.z);
        grabbedPrize.transform.DOLocalMove(Vector3.zero, 0.5f);
        grabbedPrize.SetPhysics(false);
    }

    public void DropPrize()
    {
        OpenClaw();
        grabbedPrize.transform.parent = stageManager.prizeFactory.transform;
        grabbedPrize.SetPhysics(true);
        grabbedPrize = null;
    }

    void OpenClaw()
    {
        foreach (Transform t in rootClawFingers)
        {
            t.DOLocalRotate(clawOpenAngle, 0.2f).SetEase(Ease.Linear);
        }
    }

    void CloseClaw()
    {
        Vector3 newRot = new Vector3(0, 10, 0);
        foreach (Transform t in rootClawFingers)
        {
            t.DOLocalRotate(newRot, 0.2f).SetEase(Ease.Linear);
        }
    }

    void ResetClawRotation()
    {
        foreach (Transform t in rootClawFingers)
        {
            t.DOLocalRotate(Vector3.zero, 0.2f);
        }
    }
}
