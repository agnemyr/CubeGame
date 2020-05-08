using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    private const float FlashOnset = 11;
    private readonly float[] _flashInvisibleDur = {0.3f, 0.6f};
    [FormerlySerializedAs("startingTime")] public float counter = 15 + 1f; //a one second delay is added
    private float _startTime;
    private float _startServerTime;

    public Text countdownText;

    // Start is called before the first frame update
    void Start()
    {
        _startTime = counter;
        StartCountdownTimer();
        _startServerTime = getServerTime();
    }

    private float getServerTime()
    {
        foreach (GameBehaviour gb in FindObjectsOfType<GameBehaviour>())
        {
            if (gb.isLocalPlayer)
            {
                return gb.GetServerTime();
            }
        }
        return 0;
    }

    void StartCountdownTimer()
    {
        if (countdownText != null)
            countdownText.text = "3:00";
        InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);
    }

    // Update is called once per frame
    void UpdateTimer()
    {
        if (counter <= 0)
        {
            GameObject.Find("EventCanvas").transform.GetChild(1).transform.gameObject.SetActive(true);
            FindObjectOfType<GoalBehaviour>().Invoke(nameof(GoalBehaviour.PauseGame), 4f);
            return;
        }
        
        if (countdownText != null)
        {
            //startingTime -= Time.deltaTime;
            counter = _startTime - (getServerTime() - _startServerTime);

            if (counter <= 0)
            {
                countdownText.text = "0:00";
            }
            else
            {
                string minutes = Mathf.FloorToInt(counter / 60).ToString("0");
                string seconds = Mathf.FloorToInt(counter % 60).ToString("00");
                countdownText.text = minutes + ":" + seconds;
                
                if (counter <= 6)
                    countdownText.color = Color.red;
            }
            Flash();
        }
    }

    private void Flash()
    {
        var fraction = counter - Mathf.Floor(counter);
        if (counter < FlashOnset && counter >= 1 && fraction > _flashInvisibleDur[0] && 
            fraction < _flashInvisibleDur[1]) 
            countdownText.gameObject.SetActive(false);
        else
        {
            countdownText.gameObject.SetActive(true);
        }
    }

    public void RestartLevel ()
    {
        Debug.Log("Hello this is NextLevel()");
        
        foreach (GameBehaviour gb in FindObjectsOfType<GameBehaviour>())
        {
            if (gb.isLocalPlayer)
            {
                gb.CmdRestartLevel();
            }
        }
    }
}
