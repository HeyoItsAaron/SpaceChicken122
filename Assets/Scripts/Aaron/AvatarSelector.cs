//Aaron Williams
//10/24/2022
//from tutorial at https://www.youtube.com/watch?v=3qlRgICRoeA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarSelector : MonoBehaviour
{
    //class variables

    public GameObject[] avatars;    //make sure this is same order as in Network Player prefab
    public int avatarIndex = 0;
    public int currentScene;

    //methods

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }

    //Displays the next avatar in the array created above
    public void NextAvatar()
    {
        avatars[avatarIndex].SetActive(false);
        avatarIndex = (avatarIndex + 1) % avatars.Length;
        avatars[avatarIndex].SetActive(true);
    }

    //Displays the previous avatar in the array created above, if the index is below zero it starts back at the
        // highest index that exists
    public void PreviousAvatar()
    {
        avatars[avatarIndex].SetActive(false);
        avatarIndex--;
        if (avatarIndex < 0)
        {
            avatarIndex += avatars.Length;
        }
        avatars[avatarIndex].SetActive(true);
    }

    //Selects this avatar to load into the game
    public void SetAvatarID() // aka Select
    {
        PlayerPrefs.SetInt("AvatarID", avatarIndex);

    }
}
