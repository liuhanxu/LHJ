/*
 * _UITools.cs
 * Fast3
 * Created by DefaultCompany on 12/18/2015 08:45:03.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using LitJson;

public class _UITools : Editor 
{
	/// <summary>
	/// Records the object info.
	/// </summary>
	[MenuItem("GameObject/RecordUITreeInfo",false,13)]
	public static void RecordObjInfo()
	{
		string path = Application.dataPath + "/StreamingAssets/mainuilayout.txt";

		if (Selection.activeObject == null)
			return;
		
		Transform acTrans = Selection.activeGameObject.transform;

		JsonData jd = LoopChildren (acTrans);
		Debug.Log ("=="+jd.ToJson());

		File.WriteAllText (path, jd.ToJson());
	}

	/// <summary>
	/// Loops the children.
	/// </summary>
	/// <returns>The children.</returns>
	/// <param name="t">T.</param>
	private static JsonData LoopChildren(Transform t)
	{
		RectTransform rt = (RectTransform)t;

		JsonData jd = new JsonData ();
		jd ["name"] = t.name;
		JsonData trJD = new JsonData ();
		trJD ["position"] = rt.anchoredPosition.VectorToJson ();
		trJD ["sizeDelta"] = rt.sizeDelta.VectorToJson ();
		trJD ["anchorMin"] = rt.anchorMin.VectorToJson ();
		trJD ["anchorMax"] = rt.anchorMax.VectorToJson ();
		trJD ["pivot"] = rt.pivot.VectorToJson ();
		trJD ["rotation"] = rt.rotation.eulerAngles.VectorToJson ();
		trJD ["scale"] = rt.localScale.VectorToJson ();
		jd ["transform"] = trJD;

		int len = t.childCount;
		JsonData ch = new JsonData ();
		for (int i = 0; i < len; ++i) {
			ch.Add(LoopChildren (t.GetChild (i)));
		}
		string jsStr = JsonMapper.ToJson (ch);
		jd ["children"] = string.IsNullOrEmpty(jsStr)?"[]":ch;

		return jd;
	}

	/// <summary>
	/// Autos the layout.
	/// 
	/// </summary>
	[MenuItem("GameObject/AutoLayout",false,14)]
	public static void AutoLayout()
	{
		string path = Application.dataPath + "/StreamingAssets/mainuilayout.txt";

		if (Selection.activeObject == null) {
			Debug.LogError("Select a GameObject First");
			return;
		}

		if (!File.Exists (path)) {
			Debug.LogError ("File " + path + " isn't exist");
			return;
		}

		Transform acTrans = Selection.activeGameObject.transform;

		string jsonStr = File.ReadAllText (path);
		JsonData js = JsonMapper.ToObject (jsonStr);

		if (js ["name"].ToString () == acTrans.name) {
			SetTransform (acTrans, js);
		}
	}

	/// <summary>
	/// Sets the transform.
	/// </summary>
	/// <param name="t">T.</param>
	/// <param name="jd">Jd.</param>
	private static void SetTransform(Transform t,JsonData jd)
	{
		RectTransform rt = (RectTransform)t;

		JsonData trJD = jd ["transform"];
		rt.anchoredPosition = trJD ["position"].JsonToVector ();
		rt.sizeDelta = trJD ["sizeDelta"].JsonToVector ();
		rt.anchorMin=trJD ["anchorMin"].JsonToVector ();
		rt.anchorMax = trJD ["anchorMax"] .JsonToVector ();
		rt.pivot = trJD ["pivot"] .JsonToVector();
		rt.rotation = Quaternion.Euler (trJD ["rotation"].JsonToVector ());
		rt.localScale = trJD ["scale"].JsonToVector ();

		JsonData chJD = jd ["children"];
		if (chJD.IsArray && chJD.Count > 0) {
			for (int i = 0; i < chJD.Count; ++i) {
				JsonData cd = chJD [i];
				Transform cht = t.FindChild (cd ["name"].ToString());
				if (cht != null) {
					SetTransform (cht, cd);
				}
			}
		}
	}
}

public static class _Tools
{
	/// <summary>
	/// Vector2 转 json.
	/// </summary>
	/// <returns>The to json.</returns>
	/// <param name="v">V.</param>
	public static JsonData VectorToJson(this Vector2 v)
	{
		JsonData d = new JsonData ();
		d ["x"] = v.x;
		d ["y"] = v.y;
		return d;
	}

	/// <summary>
	/// Vector3 转 json.
	/// </summary>
	/// <returns>The to json.</returns>
	/// <param name="v">V.</param>
	public static JsonData VectorToJson(this Vector3 v)
	{
		JsonData d = new JsonData ();
		d ["x"] = v.x;
		d ["y"] = v.y;
		d ["z"] = v.z;
		return d;
	}

	/// <summary>
	/// Json转Vector
	/// </summary>
	/// <returns>Vector3</returns>
	/// <param name="j">J.</param>
	public static Vector3 JsonToVector(this JsonData j)
	{
		float x = 0, y = 0, z = 0;
		float.TryParse (j ["x"].ToJson (), out x);
		float.TryParse (j ["y"].ToJson (), out y);
		if (j.Count==3)
			float.TryParse (j ["z"].ToJson (), out z);

		return new Vector3 (x, y, z);
	}
}
