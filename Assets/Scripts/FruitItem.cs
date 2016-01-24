using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class FruitItem : MonoBehaviour {

    Image icon,light;
    int index;

	// Use this for initialization
	void Start () {
        index = int.Parse(gameObject.name);
        icon = Util.Get<Image>(transform, "Icon_Image");
        light = Util.Get<Image>(transform, "Light");

        InitUI();
	}

    void InitUI()
    {

    }

    public void PlayLight(int st=0)
    {
        CancelInvoke("SetLight");
        if (st==0)//��
        {
            InvokeRepeating("SetLight", 0, 0.8f);
        }
        else if (st == 1)//����ͣ
        {
            light.gameObject.SetActive(true);
        }
        else//��
        {
            light.gameObject.SetActive(false);
        }
    }

    public Sprite GetSprite()
    {
        return icon.sprite;
    }

    bool act = false;
    void SetLight()
    {
        act = !act;
        light.gameObject.SetActive(act);
    }
}
