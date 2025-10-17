using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButton : MonoBehaviour
{
   
    public SceneStateManager.SceneState targetScene;

    public void OnButtonPressed(){
        SceneStateManager.Instance.CurrentScene = targetScene;
    }
}
