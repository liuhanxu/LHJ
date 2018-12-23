using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using LitJson;

public class MainUIView : MonoBehaviour
{
    [SerializeField]
    SoundManager soundManager;

    [SerializeField]
    Transform itemRoot, historyRoot;

    [SerializeField]
    GameObject hisItemPrefab;

    const int LEN = 24;
    Vector3[] itemPos = new Vector3[LEN];
    FruitItem[] fruitItems = new FruitItem[LEN];

    [SerializeField]
    Text obtain_txt, total_txt, show_txt;

    [SerializeField]
    Button start_btn, change_btn, da_btn, xiao_btn, auto_btn, clear_btn, setting_btn, quit_btn, modPwd_btn;
    [SerializeField]
    Dropdown dd;

    [SerializeField]
    GameObject light;

    Text[] bet_txts = new Text[8];
    Button[] bet_btns = new Button[8];

    int lastPrizeNo = 0;
    bool isRunning = false, isAuto = false,isGuessed = true;
    int curTimes = 5;
    int curTotalBet = 0;
    int[] curBets = new int[8];
    int obtain = 0;
    int control = 0;

    // Use this for initialization
    void Start()
    {
        InitUI();
        InitData();
    }

    void InitUI()
    {
        for (int i = 0; i < LEN; ++i)
        {
            GameObject go = Util.Child(itemRoot, (i + 1).ToString());
            if (go)
            {
                itemPos[i] = go.transform.localPosition;
                fruitItems[i] = Util.Get<FruitItem>(go, "");
            }
        }

        for (int i = 0; i < 8; i++)
        {
            string id = (i + 1).ToString();
            bet_txts[i] = Util.Get<Text>(transform, "fruit_Texts/" + id + "/bet_Text");
            bet_btns[i] = Util.Get<Button>(transform, "fruit_Buttons/" + id);

            int index = i;
            bet_btns[i].onClick.AddListener(() =>
                {
                    onBet(index);
                });
        }

        int l = historyRoot.childCount;
        for (int i = 0; i < l; i++)
        {
            Destroy(historyRoot.GetChild(i).gameObject);
        }

        float sound = PlayerPrefs.GetFloat("SOUND",1);
        EventSystem.Instance.FireEvent(EventCode.SoundSetting, sound);
        setting_btn.GetComponentInChildren<Text>().text = sound > 0 ? "声音开" : "声音关";


        light.SetActive(false);

        start_btn.onClick.AddListener(onStart);
        change_btn.onClick.AddListener(onChange);
        da_btn.onClick.AddListener(onDa);
        xiao_btn.onClick.AddListener(onXiao);
        auto_btn.onClick.AddListener(onAuto);
        clear_btn.onClick.AddListener(onClear);
        setting_btn.onClick.AddListener(onSetting);
        quit_btn.onClick.AddListener(onQuit);
        modPwd_btn.onClick.AddListener(onModPwd);
        dd.onValueChanged.AddListener(onDD);
    }

    void InitData()
    {
        for (int i = 0; i < 8; i++)
        {
            curBets[i] = 0;
            bet_txts[i].text = curBets[i].ToString();
        }


        obtain = 0;
        obtain_txt.text = "0";
        total_txt.text = GlobalData.playerTotal.ToString();

        curTimes = GlobalData.times[GlobalData.curTimeIndex];
        change_btn.GetComponentInChildren<Text>().text = curTimes.ToString();

        UpdateUIState();
    }

    void UpdateUIState()
    {
        start_btn.interactable = !isRunning && curTotalBet > 0;
        total_txt.text = GlobalData.playerTotal.ToString();
        obtain_txt.text = obtain.ToString();
        da_btn.interactable = obtain > 0 && !isRunning && !isGuessed;
        xiao_btn.interactable = obtain > 0 && !isRunning && !isGuessed;
        clear_btn.interactable = !isAuto && !isRunning;
        change_btn.interactable = !isAuto && !isRunning;
    }

    void onBet(int index)
    {
        soundManager.BtnTap(index);
        Debug.Log(index);

        if ( curTotalBet > GlobalData.playerTotal)
        {
            Debug.Log("余额不足，请您先充值吧");
            EventSystem.Instance.FireEvent(EventCode.ShowTips, "余额不足，请您先充值吧！");
            return;
        }

        if (curBets[index] + curTimes > 9999)
            return;
        curBets[index] += curTimes;
        bet_txts[index].text = curBets[index].ToString();
        curTotalBet += curTimes;
        //GlobalData.playerTotal -= curTimes;
        UpdateUIState();
    }

    void onChange()
    {
        soundManager.BtnTap(8);

        GlobalData.curTimeIndex++;
        if (GlobalData.curTimeIndex >= GlobalData.times.Length)
        {
            GlobalData.curTimeIndex = 0;
        }
        curTimes = GlobalData.times[GlobalData.curTimeIndex];

        EventSystem.Instance.FireEvent(EventCode.ShowTips, "当前分档为一次" + GlobalData.times[GlobalData.curTimeIndex] + "分");

        change_btn.GetComponentInChildren<Text>().text = curTimes.ToString();
    }

    void onClear()
    {
        soundManager.BtnTap(8);

        for (int i = 0; i < 8; i++)
        {
            curBets[i] = 0;
            bet_txts[i].text = curBets[i].ToString();
        }
        GlobalData.playerTotal += curTotalBet;
        curTotalBet = 0;
        UpdateUIState();
    }
    void onDa()
    {
        soundManager.BtnTap(8);
        if (obtain > 0)
        {
            GameManager.Instance.httpClient.JGuessBig(GlobalData.userId, "big", (res) => {
                isGuessed = true;
                obtain *= 2;

            });
        }
        else
        {
            EventSystem.Instance.FireEvent(EventCode.ShowTips, "你已经输光啦，请赢点钱再来吧！");
        }
        UpdateUIState();
    }

    void onXiao()
    {
        soundManager.BtnTap(8);
        if (obtain > 0)
        {
            GameManager.Instance.httpClient.JGuessBig(GlobalData.userId, "small", (res) =>
            {
                isGuessed = true;
                obtain *= 2;

            });
        }
        else
        {
            EventSystem.Instance.FireEvent(EventCode.ShowTips, "你已经输光啦，请赢点钱再来吧！");
        }

        UpdateUIState();
    }

    void onAuto()
    {
        soundManager.BtnTap(8);
        isAuto = !isAuto;
        Util.Get<Text>(auto_btn, "Text").text = isAuto ? "取消托管" : "托管";
    }

    void onSetting()
    {
        soundManager.BtnTap(8);
        float sound = PlayerPrefs.GetFloat("SOUND", 1);
        sound = sound > 0 ? 0f : 1f;
        EventSystem.Instance.FireEvent(EventCode.SoundSetting, sound);
        PlayerPrefs.SetFloat("SOUND", sound);
        setting_btn.GetComponentInChildren<Text>().text = sound > 0 ? "声音开" : "声音关";
    }
    void onModPwd()
    {
        soundManager.BtnTap(8);
    }

    void onQuit()
    {
        soundManager.BtnTap(8);
        Application.Quit();
    }

    void Score()
    {
        GlobalData.playerTotal += obtain;
        obtain = 0;
        UpdateUIState();
    }

    void onDD(int index){
        control = index;
    }

    void ResetEffect()
    {
        for (int i = 0; i <LEN; i++)
        {
            fruitItems[i].PlayLight(2);
        }
    }
    void onStart()
    {
        if (!Util.NetAvailable)
        {
            EventSystem.Instance.FireEvent(EventCode.ShowTips, Const.NoNetworkInfo);
            return;
        }
        if (curTotalBet > GlobalData.playerTotal)
        {
            EventSystem.Instance.FireEvent(EventCode.ShowTips, "余额不足，请您先充值吧！");
            return;
        }

        obtain = 0;
        show_txt.text = obtain.ToString();
        UpdateUIState();

        string betStr = "";

        GameManager.Instance.httpClient.JStartGame(GlobalData.userId, betStr, (res) => {

            JsonData jd = JsonMapper.ToObject(res.ToString());
            if (jd["errorcode"].ToString() == "0")
            {
                GlobalData.playerTotal -= curTotalBet;
                //if (isAnimate)
                //    isBreak = true;
                ResetEffect();
                isRunning = true;
                isGuessed = false;
                UpdateUIState();

                int des = Random.Range(0, 24);//模拟
                switch (control)
                {
                    case 0:
                        break;
                    case 1:
                        des = Random.Range(0, 20) < 10 ? 4 : 16;
                        break;
                    case 2:
                        des = Random.Range(0, 20) < 10 ?1:13;
                        break;
                    case 3:
                        des = 15;
                        break;
                    case 4:
                        des = 3;
                        break;
                }


                Debug.Log("Last=" + lastPrizeNo + "   result=" + des);

                if /*(des > 1)// */(des == 9 || des == 21)
                {
                    int len = Random.Range(3, 6);
                    int[] results = new int[len];
                    results[0] = Random.Range(0, 9);
                    int ob = 0;
                    for (int i = 1; i < len;i++ )
                    {
                        if (results[i - 1] + 4 > 23)
                        {
                            results[i] = results[i - 1] + 4 - 24;
                        }
                        else
                        {
                            results[i] = results[i - 1] + 4;
                        }
                        Debug.Log("res=" + results[i]);
                        ob += Const.PRIZE[results[i]];
                    }
                    obtain = ob;
                    StartCoroutine(yieldRun(lastPrizeNo, 21, results[0], results));
                }
                else
                {
                    obtain = Const.PRIZE[des];
                    StartCoroutine(yieldRun(lastPrizeNo, des));//, 15,19));
                }
                lastPrizeNo = des;
            }
            else
            {
                EventSystem.Instance.FireEvent(EventCode.ShowTips, "开始错误");
            }
        });
    }

    /// <summary>
    /// 小奖
    /// </summary>
    /// <param name="st">起始位置</param>
    /// <param name="des">目标中奖位置</param>
    /// <param name="startDelay">初始延迟</param>
    /// <returns></returns>
    IEnumerator yieldRun(int st, int des, float startDelay = 0.1f)
    {
        int count = des % 24;
        count += Random.Range(4, 6) * 24 + 1;//转4-5圈 ;
        count -= st;

        light.transform.localPosition = itemPos[st];
        light.SetActive(true);
        yield return new WaitForSeconds(startDelay);//初始等待

        float loopTime = (count - 16) / 24.0f * 0.365f + 0.0f;
        Debug.Log("looptime=" + loopTime + " count=" + count);

        float deto = soundManager.StartRun(0);
        soundManager.MidRun(loopTime, deto, 0);
        soundManager.EndRun(deto + loopTime, 0);

        Debug.Log("1------------------------------------------" + Time.time);
        float delay = 0.01f;

        for (int i = 1; i < count; ++i)
        {
            //Debug.Log("i=" + i + "   index=" + i % LEN);
            if (i < 9)//前9步跑第一段音乐
            {
                delay = 0.1744f;//1.567
            }
            else if (i > count - 7)//最后一段音乐
            {
                delay = 0.197f;//1.384
            }
            else//中间循环
            {
                delay = 0.002f;
            }
            yield return new WaitForSeconds(delay);
            int index = (st + i) % LEN;
            light.transform.localPosition = itemPos[index];
        }

        Debug.Log("2------------------------------------------" + Time.time);
        isRunning = false;
        light.SetActive(false);

        //soundManager.PlaySingle(des);
        soundManager.Prize(des%9);

        //obtain = 30;
        show_txt.text = obtain.ToString();

        fruitItems[des].PlayLight(0);
        Prize(des);
        GlobalData.playerTotal += obtain;
        UpdateUIState();

        if (isAuto)
        {
            yield return new WaitForSeconds(1);
            onStart();
        }
    }


    /// <summary>
    /// 多奖
    /// </summary>
    /// <param name="st">初始位置</param>
    /// <param name="des1">gododluck位置，11和21</param>
    /// <param name="des2">往回走到达位置，也是第一个开奖位置</param>
    /// <param name="des3"></param>
    /// <param name="startDelay"></param>
    /// <returns></returns>
    IEnumerator yieldRun(int st, int des1, int des2, int[] des3, float startDelay = 0.1f)
    {
        int count = des1 % 24;//11或22
        count += Random.Range(3, 6) * 24 + 1;//转3-5圈 ;
        count -= st;

        light.transform.localPosition = itemPos[st];
        light.SetActive(true);
        yield return new WaitForSeconds(startDelay);

        float loopTime = (count - 16) / 24.0f * 0.365f + 0.0f;
        Debug.Log("looptime=" + loopTime);

        float deto = soundManager.StartRun(0);
        soundManager.MidRun(loopTime, deto, 0);
        soundManager.EndRun(deto+loopTime,0);

        float delay = 0.01f;
        int id1 = 1;
        int last1 = id1;
        for (; id1 < count; id1++)
        {
            //Debug.Log("i=" + i + "   index=" + i % LEN);
            if (id1 <  9)
            {
                delay = 0.1744f;//1.567
            }
            else if (id1 > count - 7)
            {
                delay = 0.1977f;//1.384
            }
            else
            {
                delay = 0.01f;
            }
            yield return new WaitForSeconds(delay);
            int index = (id1 + st) % LEN;
            light.transform.localPosition = itemPos[index];
            last1 = index;
        }

        fruitItems[last1].PlayLight(1);

        yield return new WaitForSeconds(0.1f);
        //int count2 =id1 % 24;
        int last2 = last1;
        soundManager.EndRun(0);
        soundManager.MoveBackTick(1);

        for (int i = 1; i < des1- des2; i++)
        {
            delay = 0.1f;
            yield return new WaitForSeconds(delay);
            //soundManager.PlayShortMid(1);

            int index = (last1 - i) % 24;
            if (index < 0)
            {
                index = LEN + index;
            }

            light.transform.localPosition = itemPos[index];
            last2 = index;
        }

        fruitItems[last2].PlayLight(1);

        yield return new WaitForSeconds(0.1f);
        int count3 = des3[des3.Length-1] % 24;
        if (count3 < last2)
            count3 += 24;
        count3 -= last2;
        count3 += 1;

        float lpt = (count3 - 1) * 0.35f;

        soundManager.BigPrizeReverse(lpt);

        //soundManager.PlayBigPrizeMid((count3 - 1) * 0.35f);
        //soundManager.PlayBig(0);
        for (int id3 = 1; id3 < count3; id3++)
        {
            delay = 0.35f;
            yield return new WaitForSeconds(delay);
            int index = (id3 + last2) % LEN;
            light.transform.localPosition = itemPos[index];

            for (int t = 0; t < des3.Length; t++)
            {
                if (index == des3[t])
                {
                    fruitItems[index].PlayLight(1);
                    soundManager.Boom();
                    break;
                }
                else
                {
                    //soundManager.PlayShortMid(0);
                }
            }
        }

        isRunning = false;
        light.SetActive(false);

        //soundManager.BigPrize(des3.Length-1);
        soundManager.BigPrize(des3.Length - 1);
        soundManager.Prize(0);

        fruitItems[des3[des3.Length - 1]].PlayLight(0);
        Prize(last1);
        GlobalData.playerTotal += obtain;
        UpdateUIState();

        if (isAuto)
        {
            yield return new WaitForSeconds(1);
            onStart();
        }
    }

    void Prize(int index)
    {
        if (historyRoot.childCount >= 8)
        {
            Destroy(historyRoot.GetChild(7).gameObject);
        }
        GameObject go = Instantiate(hisItemPrefab) as GameObject;
        go.name = index.ToString();
        go.transform.SetParent(historyRoot);
        go.transform.SetAsFirstSibling();
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        Util.Get<Image>(go, "Icon").sprite = fruitItems[index].GetSprite();
    }

    #region Events

    void RegistHandlers()
    {
    }

    void UnregistHndlers()
    {

    }
    #endregion
}