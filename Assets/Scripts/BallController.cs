using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class BallController : GraspBehaviour
{
    public float minHoleTime;
    public ScoreBoard scoreBoard;
    public List<GameObject> startPosition;
    public int currentPositionIndex = 0;
    public Rigidbody ball;
    private float puttCoolDown = 1.0f;
    private float lastPuttTime = 0.0f;
    private float holeTime;
    private Vector3 lastPosition; // get ball back when out of bounce
    private Vector3 initialPosition;
    //private Vector3 clubbluePos;
    //private Quaternion clubblueRot;
    private Vector3 clubParentPos;
    private Quaternion clubParentRot;
    private Vector3 attachObj;
    public GameObject attachPos;
    public int currentLevel;
    public LevelManager levelManager;
    public Canvas msgCanvas;
    public Button restartBtn;
    public ClubController clubController;
    //public GameObject clubblue;
    public GameObject clubParent;
    //public XRGrabInteractable XRGrabInteractable;


    internal override void Awake()
    {
        base.Awake();
        ball = GetComponent<Rigidbody>();
        ball.maxAngularVelocity = 1000;
        initialPosition = ball.position;
        lastPosition = ball.position;
        //clubbluePos = clubblue.transform.localPosition;
        //clubblueRot = clubblue.transform.localRotation;
        clubParentPos = clubParent.transform.localPosition;
        clubParentRot = clubParent.transform.localRotation;
        attachObj = attachPos.transform.localPosition;


    }
   
    internal override void Update()
    {
        Debug.Log("ball velocity:" + ball.velocity.magnitude);
        base.Update();

        // put ball back while ghost collision
        if (ball.position.y < -1)
        {
            BackToLastPosition();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hole")
        {
            Debug.Log("Trigger to hole");
            //msgCanvas.enabled = false;
            scoreBoard.CheckShott();
            //scoreBoard.AddPutt();
            if (currentPositionIndex < startPosition.Count)
            {
                transform.position = startPosition[currentPositionIndex].transform.position;
                currentPositionIndex++;
            }
            currentLevel += 1;
            levelManager.LevelCheck();
            CountHoleTime();
        }
    }

    private void CountHoleTime()
    {
        holeTime += Time.deltaTime;
        // player has finished, move to the next player
        if (holeTime >= minHoleTime)
        {
            StartAnotherRound();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hole")
        {

            LeftHole();
        }
    }

    private void LeftHole()
    {
        holeTime = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude <= Mathf.Epsilon) return;

        // if ball fall out of bounds
        if (collision.collider.tag == "Out Of Bounds")
        {
            BackToLastPosition();
        }

        // if club collide with the ball
        if (collision.collider.tag == "Putt")
        {
            ApplyForce(transform.forward, clubController.batDelta * clubController.batPower);
            Debug.Log("Detected: " + clubController.batDelta * clubController.batPower);

            //AudioManager.inst.SoundPlay(AudioManager.SoundName.Sound1);
            scoreBoard.ChancesOfClub();
            WaitMassage();
            //Debug.Log(Time.time);


            if (Time.time > lastPuttTime + puttCoolDown)
            {
                //Debug.Log("time:"+Time.time);
                //Debug.Log(Time.time > lastPuttTime + puttCoolDown);
                //scoreBoard.AddPutt();
                lastPuttTime = Time.time;
                lastPosition = ball.position;

            }


        }
    }

    public void BackToInitialPosition()
    {

        transform.position = initialPosition;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        lastPosition = initialPosition;
        Debug.Log("intial" + initialPosition);
    }

    private void BackToLastPosition()
    {
        transform.position = lastPosition;
        ball.velocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        Debug.Log("last" + lastPosition);
    }

    public void StartAnotherRound()
    {

        holeTime = 0;
        scoreBoard.ResetPutts();
        scoreBoard.ResetClubChance();
        BackToLastPosition();
        //BackToInitialPosition(); // move ball back
    }
    public void WaitMassage()
    {
        msgCanvas.enabled = true;
        Text msgText = msgCanvas.GetComponentInChildren<Text>();
        msgText.text = "Please Wait !!";

        StartCoroutine(ClubDisable());

        Debug.Log("before velocity:" + ball.velocity.magnitude);
        //ball.drag = 1;
        //ball.angularDrag = 2;

        if (ball.velocity.magnitude >= 0.00001579362 || ball.velocity.magnitude <= 0.9864704 || ball.velocity.magnitude <= 9.5468)
        {
            StartCoroutine(WaitAndShott());

        }

    }
    private IEnumerator WaitAndShott()
    {
        yield return new WaitForSeconds(2);
        if (scoreBoard.clubValue == 0)
        {
            //Text Text = msgCanvas.GetComponentInChildren<Text>();
            //Text.text = "GameOver";
            //restartBtn.enabled = true;
            msgCanvas.enabled = false;
            scoreBoard.cnavasGameOver.enabled = true;
            //ball.drag = 1;
            //ball.angularDrag = 1;

        }
        else
        {
            Text msgText = msgCanvas.GetComponentInChildren<Text>();
            msgText.text = "Now Play Remaining Shott";
            //ball.drag = 1;
            //ball.angularDrag = 1;
        }
        yield return new WaitForSeconds(4);
        msgCanvas.enabled = false;
        MeshRenderer meshRenderer = clubController.club.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer)
        {
            clubController.club.GetComponentInChildren<Collider>().enabled = true;
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                Color32 color = meshRenderer.materials[i].color;
                color.a = 255;
                meshRenderer.materials[i].color = color;
            }
        }
        clubController.club.isKinematic = false;

    }

    private IEnumerator ClubDisable()
    {
        yield return new WaitForSeconds(0.6f);
        MeshRenderer meshRenderer = clubController.club.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer)
        {
            clubController.club.GetComponentInChildren<Collider>().enabled = false;
            for (int i = 0; i < meshRenderer.materials.Length; i++)
            {
                Color32 color = meshRenderer.materials[i].color;
                color.a = 60;
                meshRenderer.materials[i].color = color;
                Debug.Log("alphaa" + color);
            }
        }
        clubController.club.isKinematic = true;

    }
    public void RestartBtn()
    {
        levelManager.LevelCheck();
        transform.position = initialPosition;
        //clubblue.transform.localPosition = clubbluePos;
        //clubblue.transform.localRotation = clubblueRot;
        clubParent.transform.localPosition = clubParentPos;
        clubParent.transform.localRotation = clubParentRot;
        attachPos.transform.localPosition = attachObj;
        scoreBoard.cnavasGameOver.enabled = false;
    }
    //public void Grabb(SelectEnterEventArgs args0)
    //{
    //    club.isKinematic = false;
    //}

    public void ApplyForce(Vector3 angleVector, float velocity)
    {
        ball.AddForce(angleVector.normalized * velocity, ForceMode.Impulse);
    }

}










