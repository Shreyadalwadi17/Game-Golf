using UnityEngine;
using UnityEngine.UI;
using Ubiq.Messaging;

public class ScoreBoard : MonoBehaviour
{
    public Text puttText;
    private int puttValue;

    public Text clubText;
    public int clubValue;

    public Text shottText;
    NetworkContext context;
    public Canvas cnavasGameOver;
    public BallController ballController;
    public LevelManager levelManager;



    private void Start()
    {
        context = NetworkScene.Register(this);

    }

    private struct Message
    {
        public int putts;
    }

    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        // Parse the message
        var m = message.FromJson<Message>();

        puttValue = m.putts;
        puttText.text = puttValue.ToString();
    }

    //public void AddPutt()
    //{
    //    puttValue+=10;
    //    puttText.text = puttValue.ToString();

    //    context.SendJson(new Message()
    //    {
    //        putts = puttValue
    //    });
    //}

    public void ResetPutts()
    {
        puttValue = 0;
    }

    public void ChancesOfClub()
    {
        //if (clubValue <= 1)
        //{

            //ballController.msgCanvas.enabled = false;
            //cnavasGameOver.enabled = true;


        //}
        if (clubValue > 0)
        {
            clubValue -= 1;
            clubText.text = clubValue.ToString();
            
        }
        else
        {
            clubValue = 0;
        }

    }
    public void ResetClubChance()
    {
        clubValue = 0;
    }
    public void CheckShott()
    {
        switch (clubValue)
        {
            case 6:
                shottText.text = "Eagle";
                puttValue += 60;
                puttText.text = puttValue.ToString();
                break;

            case 5:
                shottText.text = "Birdie";
                puttValue += 50;
                puttText.text = puttValue.ToString();
                break;
            case 4:
                shottText.text = "Par";
                puttValue += 40;
                puttText.text = puttValue.ToString();
                break;
            case 3:
                shottText.text = "Bogey";
                puttValue += 30;
                puttText.text = puttValue.ToString();
                break;
            case 2:
                shottText.text = "Double Bogey";
                puttValue += 20;
                puttText.text = puttValue.ToString();
                break;
            case 1:
                shottText.text = "Triple Bogey";
                puttValue += 10;
                puttText.text = puttValue.ToString();
                break;

        }
    }
    public void ClickRestartButton()
    {
        //cnavasGameOver.enabled = false;
        ballController.StartAnotherRound();
        Time.timeScale = 1;
    }



}
