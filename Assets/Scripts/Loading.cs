using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

public class Loading : MonoBehaviour {

    [SerializeField]
    Slider slider;

    private AsyncOperation async;

	// Use this for initialization
	void Start () {
#if UNITY_5_3
        async =  SceneManager.LoadSceneAsync(GlobalData.NextLevel);
#else
         async =Application.LoadLevelAsync(GlobalData.NextLevel);
#endif
	}
	
	// Update is called once per frame
	void Update () {

        if (slider && async.progress <= 1)
        {
            slider.value = async.progress;
        }
	}
}
