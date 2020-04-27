using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour
{
    [SerializeField] Image fadeIn;
    [SerializeField] Vector3 fadeIn_Rotation;
    [SerializeField] float fadeIn_RotationSpeed;
    GameManager gameMan;
    public Text t_Score;
    public Text t_Text;
    public Text h_Score;
    public GameObject t_Size;
    Vector3 start;
    public float highScore;
    public float l_Time;
    public float l_MTime;
   public bool gameOver = false;
    public GameObject InGameUI;
    P_Movement player;
    // Start is called before the first frame update
    void Start()
    {
        gameMan = FindObjectOfType<GameManager>();
        fadeIn.CrossFadeAlpha(0, 2, false);
        start = t_Size.transform.position;
        gameOver = false;
        player = FindObjectOfType<P_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        fadeIn.transform.Rotate(fadeIn_Rotation * fadeIn_RotationSpeed * Time.deltaTime);
        StartCoroutine(WaitForStart());
        t_Score.text = "Score: " + $"{Score(player.score)}";
        if (Score(Score(player.score)) > PlayerPrefs.GetFloat("Best Score", highScore))
        {
            highScore = Mathf.Round(Score(Score(player.score)));
            PlayerPrefs.SetFloat("Best Score", highScore);
            PlayerPrefs.Save();
        }
        h_Score.text = "HighScore: " + $"{PlayerPrefs.GetFloat("LVL1HSCORE", highScore)}";
    }

    void TimerTextFormat()
    {
        l_Time -= Time.deltaTime;
        if (Mathf.Round(l_Time % 60) >= 10)
        {
            t_Text.text = $"{Mathf.Floor((l_Time / 60))}:{Mathf.Floor(l_Time % 60)}";
        }
        if (Mathf.Floor(l_Time % 60) < 10)
        {
            t_Text.text = $"{Mathf.Round((l_Time / 60))}:0{Mathf.Floor(l_Time % 60)}";
        }
        if (l_Time <= 0)
        {
            t_Text.text = "Times Up!";
            gameOver = true;
        }
        if(gameOver) StartCoroutine(GameOver());
        //if (l_Time <= 30)
        //{
        Hurry();
       // }
    }

    public IEnumerator GameOver()
    {

        gameMan.gameWin = true;
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync(0);
        SceneManager.UnloadSceneAsync(1);
    }

IEnumerator WaitForStart()
{
    yield return new WaitForSeconds(1.5f);
        if (t_Text != null)
        {
            InGameUI.SetActive(true);
            fadeIn.gameObject.SetActive(false);
            fadeIn.transform.Rotate(fadeIn_Rotation * 0);
            TimerTextFormat();
        }
}

public void EraseData()
{
    PlayerPrefs.DeleteAll();
}

void Nullifier()
{
    t_Text = null;
    t_Size = null;
    t_Score = null;
    h_Score = null;
}


public float Score(float r)
{
    return Mathf.Round((r) * 10);
}

void Hurry()
    {
        t_Size.transform.position = Vector3.Lerp(start, new Vector3(t_Size.transform.position.x, t_Size.transform.position.y + 5, t_Size.transform.position.z), Mathf.PingPong(Time.time, .5f));
        t_Text.color = Color.Lerp(Color.green, Color.red, Mathf.PingPong(Time.time, 1f));
    }

}
