using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class DefaultRoom
{
    public string Name;
    public int sceneIndex;
    public int maxPlayer;
}

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public List<DefaultRoom> defaultRooms;
    public GameObject roomUI;

    //1. Connect to photon server
    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        //Debug.Log("Trying to connect to server...");
    }

    //2. join the lobby
    public override void OnConnectedToMaster()
    {
        //Debug.Log("Connected to server");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    //3. load he lobby scene
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        //Debug.Log("Lobby joined successfully.");
        roomUI.SetActive(true);
    }

    public void InitiliazeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //LOAD SCENE
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);

        //CREATE THE ROOM
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom(roomSettings.Name, roomOptions, TypedLobby.Default);
    }

    public void JoinNextRoom()
    {
        PhotonNetwork.ConnectUsingSettings();
        //Debug.Log("Game joined successfully.");
    }

    public override void OnJoinedRoom()
    {
        //Debug.Log("Room joined successfully.");

            //PhotonNetwork.LoadLevel("Game");      // ?????????????? SHOULD WE ???????????
        base.OnJoinedRoom();
    }

    /*public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player joined the room");
        base.OnPlayerEnteredRoom(newPlayer);
    }*/
}
