/*
 * HttpClient.cs
 * Fast3
 * Created by com.sinodata on 12/28/2015 11:09:05.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;
using System;
using UnityEngine.EventSystems;

public enum RequestType
{
	POST = 0,
	GET = 1,
}

public class HttpClient : MonoBehaviour
{
	private Queue<KeyValuePair<RequestType, HttpRequest>> requestQueue = new Queue<KeyValuePair<RequestType, HttpRequest>>();
	private KeyValuePair<RequestType, HttpRequest> lastRequest = new KeyValuePair<RequestType, HttpRequest>();

	#region
	/****************************************数据接口****************************************************/
	/// <summary>
	/// 时间同步
	/// </summary>
	/// <param name="callback">Callback.</param>
	public void JSyncTime(Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{\"backCode\":\"0\",\"errorDesc\":\"成功\",\"game\":[{\"serviceDate\":\"20140311232323\"}]}";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jsynctime";
			Dictionary<string,string> data = new Dictionary<string, string>();
			AddRequest(RequestType.GET, new HttpRequest(url, data, callback));
		}
	}
	
	/// <summary>
	/// 登录
	/// </summary>
	/// <param name="userId">Terminal identifier.</param>
	/// <param name="name">Name.</param>
	/// <param name="passWord">Pass word.用户密码以MD5字符串形式</param>
	/// <param name="isNotBing">Is not bing.isNotBing 默认为0绑定 </param>
	/// <param name="callback">Callback.</param>
	public void JLogin(string userId,  string passWord, Action<object> callback)
	{
		if (Const.UseMock) {
            string res = "{'backCode':'0'}";

			if (callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "backCode/jlogin";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "userId",userId },
				{ "passWord", passWord },
			};
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
		}
	}

	/// <summary>
	/// 修改密码
	/// </summary>
	/// <param name="userId">Terminal identifier.</param>
	/// <param name="passWord">Pass word.</param>
	/// <param name="newPassWord">New pass word.</param>
	/// <param name="callback">Callback.</param>
	public void JModPass(string userId, string passWord, string newPassWord, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{'errorcode':'0','msg':'ok'}";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jmodpass";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "userId",userId },
				{ "passWord", Util.MD5(passWord) },
				{ "newPassWord", Util.MD5(newPassWord) },
			};
			AddRequest(RequestType.POST, new HttpRequest(url, data, callback));
		}
	}

	/// <summary>
	/// 登出
	/// </summary>
	/// <param name="terminalId">Terminal identifier.</param>
	/// <param name="callback">Callback.</param>
    public void JLogout(string userId, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{'errorcode':'0','msg':'ok'}";
			if(callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jlogout";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "userId",userId },
			};
			AddRequest(RequestType.GET, new HttpRequest(url, data, callback));
		}
	}

	
    /// <summary>
    /// 请求投注
    /// 返回开奖结果
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="betNums">投注的号码及金额</param>
    /// <param name="callback"></param>
	public void JStartGame(string userId, string betNums,  Action<object> callback)
	{
		if (Const.UseMock) {
			string res = "{'errorcode':'0','msg':'ok'}";
			if (callback != null) {
				callback(res);
			}
		} else {
			string url = Const.WebUrl + "/jbet";
			Dictionary<string,string> data = new Dictionary<string, string>() {
				{ "userId",userId },
				{ "betNums",betNums },
			};
			AddRequest(RequestType.GET, new HttpRequest(url, data, callback));
		}
	}

    public void JGuessBig(string userId, string betBig, Action<object> callback)
    {
        if (Const.UseMock)
        {
            string res = "{'errorcode':'0','msg':'ok'}";
            if (callback != null)
            {
                callback(res);
            }
        }
        else
        {
            string url = Const.WebUrl + "/jguessbig";
            Dictionary<string, string> data = new Dictionary<string, string>() {
				{ "userId",userId },
				{ "betBig",betBig },
			};
            AddRequest(RequestType.GET, new HttpRequest(url, data, callback));
        }
    }

    public void GetUserInfo(string userId, Action<object> callback)
	{
		if(Const.UseMock) {
			string res = "{'id':'123','money':'10000'}";
			if(callback != null) {
				callback(res);
			}
		} else {
            Dictionary<string, string> data = new Dictionary<string, string>() { { "userid", userId } };
			string url = Const.WebUrl + "/getuserinfo";
			AddRequest(RequestType.GET, new HttpRequest(url, data, callback));
		}
	}
	/****************************************数据接口****************************************************/
	#endregion

	/*------------------------------------------------------------------------------------------------*/
	private float progress = 0;
	private bool isloading = false;

	void Start()
	{
		//requestQueue = new Queue<KeyValuePair<RequestType, HttpRequest>>();
	}

	void Update()
	{
		//StartCoroutine(SendRequest());
		if(requestQueue.Count > 0) {
			KeyValuePair<RequestType, HttpRequest> hr = requestQueue.Dequeue();
			//lastRequest = new KeyValuePair<RequestType, HttpRequest>(hr.Key, hr.Value);
			if(hr.Key == RequestType.POST) {
				//POST
				StartCoroutine(POST(hr.Value.url, hr.Value.data, hr.Value.callback));
			} else if(hr.Key == RequestType.GET) {
				//GET
				StartCoroutine(GET(hr.Value.url, hr.Value.data, hr.Value.callback));
			}
		}
	}

	/// <summary>
	/// 加入请求队列
	/// </summary>
	/// <param name="type">Type.</param>
	/// <param name="hr">Hr.</param>
	void AddRequest(RequestType type, HttpRequest hr)
	{
		//Debug.Log("add:>>" + hr.url);
		requestQueue.Enqueue(new KeyValuePair<RequestType, HttpRequest>(type, hr));
	}

	/// <summary>
	/// 重试请求
	/// </summary>
	public void ReTryLastRequest()
	{
		if (lastRequest.Key==null)
			AddRequest(lastRequest.Key, lastRequest.Value);
	}

	IEnumerator SendRequest()
	{
		if(requestQueue.Count > 0) {
			KeyValuePair<RequestType, HttpRequest> hr = requestQueue.Dequeue();
			lastRequest = new KeyValuePair<RequestType, HttpRequest>(hr.Key, hr.Value);
			if(hr.Key == RequestType.POST) {
				//POST
				yield return StartCoroutine(POST(hr.Value.url, hr.Value.data, hr.Value.callback));
			} else if(hr.Key == RequestType.GET) {
				//GET
				yield return StartCoroutine(GET(hr.Value.url, hr.Value.data, hr.Value.callback));
			}
		}
	}

	void ShowLoading(bool st)
	{
		if (st) {
			if (isloading)
				return;
			isloading = true;
			//EventSystem.Instance.FireEvent(EventCode.EnableDialog, new UIDialogParams(UIDialogID.Loading, null, null));
		} else {
			if (!isloading)
				return;
			isloading = false;
			//EventSystem.Instance.FireEvent(EventCode.DisableDialog, new UIDialogParams(UIDialogID.Loading, null, null));
		}
	}

	public float Progress()
	{
		return progress;
	}

	private void Print(string url,Dictionary<string,string> p)
	{
		string info = "";
		info += "Send:>>" + url + "\n";
		foreach (string k in p.Keys) {
			info += string.Format("{0}={1}\n", k, p[k]);
		}
        Debug.Log(info);
	}

	IEnumerator POST(string url, Dictionary<string, string> post, Action<object> callback)
	{
		Print(url, post);
		ShowLoading(true);
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<string, string> post_arg in post) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);

		yield return www;
		progress = www.progress;
		string mContent = "";

		if(www.error != null) {
			//mContent = "error :" + www.error;
			Debug.Log("Recive:<<"+ www.error);
			//NetError网络错误等
			ShowLoading(false);
			yield return 0;
		} else {
			mContent = www.text;
			Debug.Log("Recive:<<" + mContent);
			if(callback != null) {
				callback(mContent);
			}
		}
		ShowLoading(false);
	}

	IEnumerator GET(string url, Dictionary<string, string> get, Action<object> callback)
	{
		Print(url, get);
		ShowLoading(true);
		string Parameters;
		bool first;
		if(get.Count > 0) {
			first = true;
			Parameters = "?";
			foreach(KeyValuePair<string, string> post_arg in get) {
				if(first)
					first = false;
				else
					Parameters += "&";
				Parameters += post_arg.Key + "=" + post_arg.Value;
			}
		} else {
			Parameters = "";
		}

		WWW www = new WWW(url + Parameters);

		yield return www;
		progress = www.progress;
		string mContent = "";
		if(www.error != null) {
			//mContent = "error :" + www.error;
			Debug.Log("Recive:<<"+ www.error);
			//NetError网络错误等
			ShowLoading(false);
			yield return 0;
		} else {
			mContent = www.text;
			Debug.Log("Recive:<<" + mContent);
			if(callback != null) {
				callback(mContent);
			}
		}
		ShowLoading(false);
	}

	IEnumerator GETTexture(string picURL, Action<object> callback)
	{
		WWW wwwTexture = new WWW(picURL);

		yield return wwwTexture;

		Texture2D tex = null;
		if(wwwTexture.error != null) {
			Debug.Log("error :" + wwwTexture.error);
		} else {
			tex = wwwTexture.texture;
		}

		if(callback != null) {
			callback(tex);
		}
	}

	IEnumerator GETTextureByte(string picURL, Action<object> callback)
	{
		WWW www = new WWW(picURL);
		yield return www;

		Texture2D tex = null;
		if(www.error != null) {
			Debug.Log("error :" + www.error);
		} else {
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(new StringReader(www.text));
			//通过索引查找子节点 
			string PicByte = xmlDoc.GetElementsByTagName("base64Binary").Item(0).InnerText;
			tex = Util.ByteToPic(PicByte);
		}

		if(callback != null) {
			callback(tex);
		}
	}
}

