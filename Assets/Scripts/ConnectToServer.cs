using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField userName;
    public TMP_InputField createRoomInputField;
    public TMP_InputField joinRoomInputField;

    [Space(10)]
    public Button playButton;

    [Space(10)]
    public TMP_Text connectButtonText;
    public TMP_Text[] userNames;

    [Space(10)]
    public GameObject enterName;
    public GameObject createRoom;
    public GameObject joinRoom;
    public GameObject room;


    public void Connect()
    {
        if (userName.text.Length <= 0)
            return;

        PhotonNetwork.NickName = userName.text;

        connectButtonText.text = "Connecting..";

        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnConnectedToMaster()
    {
        enterName.SetActive(false);
        createRoom.SetActive(true);
        joinRoom.SetActive(true);
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(createRoomInputField.text, roomOptions);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInputField.text);
    }

    public override void OnJoinedRoom()
    {
        createRoom.SetActive(false);
        joinRoom.SetActive(false);
        room.SetActive(true);

        playButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);


        UpdatePlayerNames();
    }

    public void LeftRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        room.SetActive(false);
        createRoom.SetActive(true);
        joinRoom.SetActive(true);
    }

    public void Play()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    private void UpdatePlayerNames()
    {
        userNames[0].gameObject.SetActive(true);
        userNames[0].text = PhotonNetwork.PlayerList[0].NickName;

        if(PhotonNetwork.PlayerList.Length == 2)
        {
            userNames[1].gameObject.SetActive(true);
            userNames[1].text = PhotonNetwork.PlayerList[1].NickName;
        }
        else
        {
            userNames[1].gameObject.SetActive(false);
        }

        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            playButton.enabled = false;
            playButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
        }
        else
        {
            playButton.enabled = true;
            playButton.GetComponent<Image>().color = Color.white;
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerNames();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerNames();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        LeftRoom();
    }
}
