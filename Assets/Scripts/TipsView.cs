using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class TipsView : MonoBehaviour {

    [SerializeField]
    Text info_txt;

    Tween tw;

	// Use this for initialization
	void Start () {
        RegistHandlers();
	}

    void OnDestroy()
    {
        UnregistHndlers();
    }

    IEnumerator HideView()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    #region Events

    void onTips(object para)
    {
        gameObject.SetActive(true);
        info_txt.text = para.ToString();
        transform.localPosition = new Vector3(0, -800, 0);

        if (tw != null)
            tw.Kill();
        tw = transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.InOutCubic);


        StartCoroutine(HideView());
    }

    void RegistHandlers()
    {
        EventSystem.Instance.RegistEvent(EventCode.ShowTips, onTips);
    }

    void UnregistHndlers()
    {
        EventSystem.Instance.UnregistEvent(EventCode.ShowTips, onTips);
    }
    #endregion
}
