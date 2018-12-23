/*
 * Utils.cs
 * RpgFramework
 * Created by com.loccy on 11/19/2015 14:34:34.
 */

using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;

public class Util : MonoBehaviour
{
	public static int Int (object o)
	{
		return Convert.ToInt32 (o);
	}

	public static float Float (object o)
	{
		return (float)Math.Round (Convert.ToSingle (o), 2);
	}

	public static long Long (object o)
	{
		return Convert.ToInt64 (o);
	}

	public static int Random (int min, int max)
	{
		return UnityEngine.Random.Range (min, max);
	}

	public static float Random (float min, float max)
	{
		return UnityEngine.Random.Range (min, max);
	}

	public static string Uid (string uid)
	{
		int position = uid.LastIndexOf ('_');
		return uid.Remove (0, position + 1);
	}

	public static long GetTime ()
	{
		TimeSpan ts = new TimeSpan (DateTime.UtcNow.Ticks - new DateTime (1970, 1, 1, 0, 0, 0).Ticks);
		return (long)ts.TotalMilliseconds;
	}

	/// <summary>
	/// 搜索子物体组件-GameObject版
	/// </summary>
	public static T Get<T> (GameObject go, string subnode) where T : Component
	{
		if (go != null) {
			return Get<T>(go.transform, subnode);
		}
		return null;
	}

	/// <summary>
	/// 搜索子物体组件-Transform版
	/// </summary>
	public static T Get<T> (Transform go, string subnode) where T : Component
	{
		if (go != null) {
			Transform sub = go.Find (subnode);
			if (sub != null)
				return sub.GetComponent<T> ();
		}
		return null;
	}

	/// <summary>
	/// 搜索子物体组件-Component版
	/// </summary>
	public static T Get<T> (Component go, string subnode) where T : Component
	{
		return Get<T>(go.transform, subnode);
	}

	/// <summary>
	/// 添加组件
	/// </summary>
	public static T Add<T> (GameObject go) where T : Component
	{
		if (go != null) {
			T[] ts = go.GetComponents<T> ();
			for (int i = 0; i < ts.Length; i++) {
				if (ts [i] != null)
					Destroy (ts [i]);
			}
			return go.gameObject.AddComponent<T> ();
		}
		return null;
	}

	/// <summary>
	/// 添加组件
	/// </summary>
	public static T Add<T> (Transform go) where T : Component
	{
		return Add<T> (go.gameObject);
	}

	/// <summary>
	/// 查找子对象
	/// </summary>
	public static GameObject Child (GameObject go, string subnode)
	{
		return Child (go.transform, subnode);
	}

	/// <summary>
	/// 查找子对象
	/// </summary>
	public static GameObject Child (Transform go, string subnode)
	{
		Transform tran = go.Find (subnode);
		if (tran == null)
			return null;
		return tran.gameObject;
	}

	/// <summary>
	/// 取平级对象
	/// </summary>
	public static GameObject Peer (GameObject go, string subnode)
	{
		return Peer (go.transform, subnode);
	}

	/// <summary>
	/// 取平级对象
	/// </summary>
	public static GameObject Peer (Transform go, string subnode)
	{
		Transform tran = go.parent.Find (subnode);
		if (tran == null)
			return null;
		return tran.gameObject;
	}
	/// <summary>
	/// Sets the active.
	/// </summary>
	/// <param name="go">Go.</param>
	/// <param name="st">If set to <c>true</c> st.</param>
	public static void SetActive(GameObject go,bool st)
	{
		if (go == null)
			return;
		go.SetActive (st);
	}

	/// <summary>
	/// Sets the active.
	/// </summary>
	/// <param name="tr">Tr.</param>
	/// <param name="st">If set to <c>true</c> st.</param>
	public static void SetActive(Transform tr,bool st)
	{
		if (tr == null || tr.gameObject == null)
			return;
		tr.gameObject.SetActive (st);
	}
	/// <summary>
	/// Sets the active.
	/// </summary>
	/// <param name="co">Co.</param>
	/// <param name="st">If set to <c>true</c> st.</param>
	public static void SetActive(Component co,bool st)
	{
		if (co == null || co.gameObject == null)
			return;
		co.gameObject.SetActive (st);
	}

	static public T FindInParents<T>(GameObject go) where T : Component
	{
		return FindInParents<T>(go.transform);
	}

	static public T FindInParents<T>(Transform go) where T : Component
	{
		if (go == null) return null;
		var comp = go.GetComponent<T>();

		if (comp != null)
			return comp;

		Transform t = go.parent;
		while (t != null && comp == null)
		{
			comp = t.gameObject.GetComponent<T>();
			t = t.parent;
		}
		return comp;
	}

	public static bool DelChild(Transform t,string name)
	{
		GameObject go = t.Find (name).gameObject;
		if (go != null) {
			Destroy (go);
			return true;
		}
		return false;
	}

	/// <summary>
	/// 手机震动
	/// </summary>
	public static void Vibrate ()
	{
		//int canVibrate = PlayerPrefs.GetInt(Const.AppPrefix + "Vibrate", 1);
		//if (canVibrate == 1) iPhoneUtils.Vibrate();
	}

	/// <summary>
	/// Base64编码
	/// </summary>
	public static string Encode (string message)
	{
		byte[] bytes = Encoding.GetEncoding ("utf-8").GetBytes (message);
		return Convert.ToBase64String (bytes);
	}

	/// <summary>
	/// Base64解码
	/// </summary>
	public static string Decode (string message)
	{
		byte[] bytes = Convert.FromBase64String (message);
		return Encoding.GetEncoding ("utf-8").GetString (bytes);
	}

	/// <summary>
	/// 判断数字
	/// </summary>
	public static bool IsNumeric (string str)
	{
		if (str == null || str.Length == 0)
			return false;
		for (int i = 0; i < str.Length; i++) {
			if (!Char.IsNumber (str [i])) {
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// HashToMD5Hex
	/// </summary>
	public static string HashToMD5Hex (string sourceStr)
	{
		byte[] Bytes = Encoding.UTF8.GetBytes (sourceStr);
		using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ()) {
			byte[] result = md5.ComputeHash (Bytes);
			StringBuilder builder = new StringBuilder ();
			for (int i = 0; i < result.Length; i++)
				builder.Append (result [i].ToString ("x2"));
			return builder.ToString ();
		}
	}

	/// <summary>
	/// 计算字符串的MD5值
	/// </summary>
	public static string MD5 (string source)
	{
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ();
		byte[] data = System.Text.Encoding.UTF8.GetBytes (source);
		byte[] md5Data = md5.ComputeHash (data, 0, data.Length);
		md5.Clear ();

		string destString = "";
		for (int i = 0; i < md5Data.Length; i++) {
			destString += System.Convert.ToString (md5Data [i], 16).PadLeft (2, '0');
		}
		destString = destString.PadLeft (32, '0');
		return destString;
	}

	/// <summary>
	/// 计算文件的MD5值
	/// </summary>
	public static string md5file (string file)
	{
		try {
			FileStream fs = new FileStream (file, FileMode.Open);
			System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
			byte[] retVal = md5.ComputeHash (fs);
			fs.Close ();

			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < retVal.Length; i++) {
				sb.Append (retVal [i].ToString ("x2"));
			}
			return sb.ToString ();
		} catch (Exception ex) {
			throw new Exception ("md5file() fail, error:" + ex.Message);
		}
	}



	/// <summary>
	/// 清除所有子节点
	/// </summary>
	public static void ClearChild (Transform go)
	{
		if (go == null)
			return;
		for (int i = go.childCount - 1; i >= 0; i--) {
			Destroy (go.GetChild (i).gameObject);
		}
	}

	
	/// <summary>
	/// 清理内存
	/// </summary>
	public static void ClearMemory ()
	{
		GC.Collect ();
		Resources.UnloadUnusedAssets ();
	}

	/// <summary>
	/// 是否为数字
	/// </summary>
	public static bool IsNumber (string strNumber)
	{
		Regex regex = new Regex ("[^0-9]");
		return !regex.IsMatch (strNumber);
	}



	/// <summary>
	/// 取得行文本
	/// </summary>
	public static string GetFileText (string path)
	{
		return File.ReadAllText (path);
	}

	/// <summary>
	/// 网络可用
	/// </summary>
	public static bool NetAvailable {
		get {
			return Application.internetReachability != NetworkReachability.NotReachable;
		}
	}

	/// <summary>
	/// 是否是无线
	/// </summary>
	public static bool IsWifi {
		get {
			return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
		}
	}

	/// <summary>
	/// 图片与byte[]互转
	/// </summary>
	/// <param name="pic">Pic.</param>
	public static Texture2D convertPNG(Texture2D pic)
	{
		byte[] data = pic.EncodeToPNG();
		//Debug.Log("data = " + data.Length + "|" + data[0]);
		Texture2D mConvertPNG = new Texture2D(200, 200);
		mConvertPNG.LoadImage(data);

		return mConvertPNG;
	}

	/// <summary>
	/// byte[]与base64互转
	/// </summary>
	/// <returns>The to pic.</returns>
	/// <param name="base64">Base64.</param>
	public static Texture2D ByteToPic(string base64)
	{ 
		Texture2D pic = new Texture2D(200, 200);
		//将base64转码为byte[] 
		byte[] data = System.Convert.FromBase64String(base64);
		//加载byte[]图片
		pic.LoadImage(data);

		string base64str = System.Convert.ToBase64String(data);

		return pic;
	}
}

public static class ExtendClass
{
	public static List<T> AddList<T>(this List<T> list, List<T> list2)
	{
		int c = list2.Count;
		for (int i = 0; i < c; ++i)
		{
			list.Add(list2[i]);
		}
		return list;
	}

	/// <summary>
	/// 剩余时间
	/// </summary>
	/// <param name="obj"></param>
	/// <returns>00d00h00m00s</returns>
	public static string ToLeftTime(this object obj)
	{
		string result = "";
		int time = 0;
		if (!int.TryParse(obj.ToString(), out time))
		{
			time = 0;
			return obj.ToString();
		}

		System.TimeSpan ts = new System.TimeSpan(0, 0, time);//Debug.Log("00-" + time);


		if (ts.Days > 0)
		{
			result = result + ts.Days + "d";
		}

		if (ts.Hours > 0)
		{
			result = result + ts.Hours + "h";
		}
		if (ts.Minutes > 0)
		{
			result = result + ts.Minutes + "m";
		}

		if (ts.Seconds > 0)
		{
			result = result + ts.Seconds + "s";
		}
		if (result == "")
		{
			result = "0";
		}
		return result;
	}

	/// <summary>
	/// 转换成当地时间
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static System.DateTime ToLocalTime(this object obj)
	{
		long time = (long)obj;
		//Debug.LogError(time);
		System.DateTime dt = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
		dt = dt.AddMilliseconds((double)time).ToLocalTime();
		return dt;
	}

	/// <summary>
	/// 返回字典的键列表LIST
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="V"></typeparam>
	/// <param name="dic"></param>
	/// <returns></returns>
	public static List<T> DicKeysToList<T, V>(this Dictionary<T, V> dic)
	{
		List<T> tempList = new List<T>();

		foreach (T t in dic.Keys)
		{
			tempList.Add(t);
		}
		return tempList;
	}

	/// <summary>
	/// 返回字典的值列表LIST
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="V"></typeparam>
	/// <param name="dic"></param>
	/// <returns></returns>
	public static List<V> DicValuesToList<T, V>(this Dictionary<T, V> dic)
	{
		List<V> tempList = new List<V>();

		foreach (V v in dic.Values)
		{
			tempList.Add(v);
		}
		return tempList;
	}

	/// <summary>
	/// 序列化深度拷贝
	/// </summary>
	public static T DepthClone<T>(T obj) where T : class
	{
		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, obj);
		stream.Position = 0;
		return formatter.Deserialize(stream) as T;
	}
}
