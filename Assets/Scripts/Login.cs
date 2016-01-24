using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif
public class Login : MonoBehaviour {

    [SerializeField]
    InputField account_input,passwd_input;
    [SerializeField]
    Button login_btn;
    [SerializeField]
    Text info_txt;


	// Use this for initialization
	void Start () {
        InitUI();
        InitData();
	}
	

	
    void InitUI()
    {
        account_input.onValueChanged.AddListener((str) => { 
            if(str.Length>0)
                info_txt.text = "";
        });
        passwd_input.onValueChanged.AddListener((str) =>
        {
            if (str.Length > 0)
                info_txt.text = "";
        });
        login_btn.onClick.AddListener(onLogin);
    }

    void InitData()
    {
        if (!Util.NetAvailable)
        {
            Debug.Log("无网络连接");
            EventSystem.Instance.FireEvent(EventCode.ShowTips, Const.NoNetworkInfo);
        }
    }

    void onLogin()
    {
        if (account_input.text != "" && passwd_input.text != "")
        {
            string userid = account_input.text.Trim();
            string pwd = passwd_input.text;
            GameManager.Instance.httpClient.JLogin(userid, pwd, (res) => {

                info_txt.text = "登录成功";
                GoToGame();

            });


            
        }
        else
        {
            Debug.Log("账号和密码不能为空");
            info_txt.text = "账号和密码不能为空";
        }
    }



    void GoToGame()
    {
        GlobalData.NextLevel = "Game";
#if UNITY_5_3
        SceneManager.LoadScene("Loading");
#else
         Application.LoadLevel(GlobalData.NextLevel);
#endif
    }
}
