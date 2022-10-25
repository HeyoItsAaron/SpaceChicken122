//Found by Aaron Williams
//10/24/2022
//Code written by Minions Art

//ADD THIS SCRIPT TO PLAYER CHARACTER

//use this on empty object for grass shader 
//link p1: https://www.patreon.com/posts/grass-geometry-1-40090373
//link p2: https://www.patreon.com/posts/grass-geometry-2-40077798

using UnityEngine;

public class ShaderInteractor : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalVector("_PositionMoving", transform.position);
    }
}