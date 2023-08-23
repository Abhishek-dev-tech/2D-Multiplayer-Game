using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField]
    private TMP_Text countText;

    [SerializeField]
    private TMP_Text[] playersNameText, playersKillText;

    [SerializeField, Space(10)]
    private Image healthBar;

    [SerializeField]
    private GameObject killPanel;

    [SerializeField, Space(10)]
    private float maxCountTime;

    private float countTimer;

    private int kills;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    private void Start()
    {
        SetKillPanelName();
    }

    private void Update()
    {
        if(GameManager.instance.GameState == GameState.Preparing)
            CountTime();

        if((Input.GetKeyDown(KeyCode.E) || Input.GetKeyUp(KeyCode.E)) && GameManager.instance.GameState == GameState.Playing)
            ToogleKillPanel();
    }

    private void CountTime()
    {
        if(countTimer > maxCountTime)
        {
            GameManager.instance.GameState = GameState.Playing;
            countText.gameObject.SetActive(false);
            healthBar.transform.parent.gameObject.SetActive(true);
            countTimer = 0;
        }

        countTimer += Time.deltaTime;

        countText.text = "Starting in ...." + (5 - countTimer).ToString("0");
    }

    public void UpdateHealthBar(float value)
    {
        healthBar.fillAmount = value / 100;
    }

    public void AddKill()
    {
        kills++;

        playersKillText[0].text = kills.ToString();
    }

    private void ToogleKillPanel()
    {
        killPanel.SetActive(!killPanel.activeInHierarchy);
    }

    private void SetKillPanelName()
    {
        playersNameText[0].text = PhotonNetwork.PlayerList[0].NickName;
        playersNameText[1].text = PhotonNetwork.PlayerList[1].NickName;
    }

    public void UpdateKills(int value)
    {
        playersKillText[0].text = kills.ToString();
        playersKillText[1].text = value.ToString();
    }
}
