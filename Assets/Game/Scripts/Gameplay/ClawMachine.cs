using DG.Tweening;
using GogoGaga.OptimizedRopesAndCables;
using System.Collections;
using System.Collections.Generic;
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
    UserData userData;

    Vector3 clawOpenAngle;

    bool isGrabSequence;

    public void Init(GameManager inGameManager)
    {
        stageManager = inGameManager.stageManager;
        userData = inGameManager.userData;
        grabArea.Init(this);
        joystick = stageManager.joystick;

        clawOpenAngle = new Vector3(0, -20, 0);

        Vector3 startPos = rootStartPos.localPosition;
        SetPos(startPos);

        ropeMesh.GetRopeScript().ropeLength = ropeMinLength;
    }

    public void DoUpdate(float dt)
    {
        if (!isGrabSequence)
        {
            moveDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);

            if (moveDirection != Vector3.zero)
            {
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

    public void StartGrabSequence()
    {
        if (!isGrabSequence)
        {
            isGrabSequence = true;
            StartCoroutine(GrabSequence());
        }
    }

    IEnumerator GrabSequence()
    {
        string prizeId = string.Empty;
        bool isDoneFailSequence = false;
        var rope = ropeMesh.GetRopeScript();

        bool success = false;

        OpenClaw();
        yield return new WaitForSeconds(0.3f);

        grabArea.SetColliderEnable(true);

        bool shouldContinue = false;
        while (!shouldContinue)
        {
            float dt = Time.deltaTime;

            rope.ropeLength += yMoveSpeed * dt;
            rope.ropeLength = Mathf.Min(rope.ropeLength, ropeMaxLength);

            bool reachBottom = rope.ropeLength >= ropeMaxLength;
            bool hasPrize = grabbedPrize != null;
            bool valid = reachBottom || hasPrize;
            shouldContinue = valid;

            yield return null;
        }

        if(grabbedPrize != null)
        {
            //Probability here
            PrizeData prizeData = grabbedPrize.prizeData;
            PrizeRarity rarity = prizeData.rarity;
            int rarityIndex = (int)rarity;
            List<bool> probabilityList = userData.probabilityDatas[rarityIndex];

            int count = probabilityList.Count;
            if(count <= 0)
            {
                stageManager.GenerateProbabilityDatas(rarity);
            }

            success = probabilityList[0];
            userData.probabilityDatas[rarityIndex].RemoveAt(0);

            prizeId = grabbedPrize.prizeData.id;

            Debug.Log($"Grabbing prize {prizeData.name},{prizeData.rarity}: {success}");
        }

        CloseClaw();
        yield return new WaitForSeconds(0.2f);

        float rndLengthOnFail = 0;

        if (!success)
        {
            float offset = 0.5f;
            rndLengthOnFail = Random.Range(ropeMinLength + offset, rope.ropeLength - offset);
        }

        shouldContinue = false;
        while (!shouldContinue)
        {
            float dt = Time.deltaTime;

            rope.ropeLength -= yMoveSpeed * dt;
            rope.ropeLength = Mathf.Max(rope.ropeLength, ropeMinLength);

            bool isFail = !success;
            bool hasAnyGrabbedPrize = grabbedPrize != null;
            bool valid = isFail && hasAnyGrabbedPrize && !isDoneFailSequence;

            if (valid)
            {
                if (rope.ropeLength <= rndLengthOnFail)
                {
                    DropPrize(false);
                    isDoneFailSequence = true;
                }
            }

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

            grabArea.SetColliderEnable(false);
        }
        else
        {
            stageManager.SaveRecord(null, 0, false);
        }

        ResetClawRotation();

        isGrabSequence = false;
    }

    public void GrabPrize(GameObject prize)
    {
        grabbedPrize = prize.GetComponent<Prize>();
        grabbedPrize.transform.parent = rootGrabbedPrize;
        grabbedPrize.transform.DOLocalMove(Vector3.zero, 0.5f);
        grabbedPrize.SetPhysics(false);
    }

    public void DropPrize(bool animate = true)
    {
        if(animate)
        {
            OpenClaw();
        }
        grabbedPrize.transform.SetParent(stageManager.prizeFactory.transform);
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
