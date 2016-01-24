/*******************************************************************
* Summary: 
* Author : 
* Date   : 
*******************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Tools
{
	public static List<List<int>> Composition(int[] source,int m)
	{
		int len = source.Length;
		if (len < m)
			return null;
		
		List<List<int>> result = new List<List<int>> ();
		List<int> temp = new List<int> ();

		bool[] comp = new bool[len];
		int i = 0;
		for (i = 0; i < len; i++)
			comp [i] = i < m;

		string s = "";
		int c = 0;
		for (int index = 0; index < source.Length; index++) {
			if (comp [index] == true) {
				s += source [index] + " ";
				temp.Add (source [index]);
			}
		}
		s+="\n";
		Debug.Log (s);
		c++;

		while (true) {
			for (i = 0; i < len - 1; i++) {
				if (comp [i] == true && comp [i + 1] == false)
					break;
			}

			if (i == len - 1)
				return result;
			comp [i] = false;
			comp [i + 1] = true;

			int p = 0;
			while (p < i) {
				while (comp [p] == true)
					p++;
				while (i >= 0 && comp [i] == false)
					i--;
				if (p < i) {
					comp [p] = true;
					comp [i] = false;
				}
			}
			temp = new List<int> ();s = "";
			for (int index = 0; index < source.Length; index++) {
				if (comp [index] == true) {
					s += source [index] + " ";
					temp.Add (source [index]);
				}
			}
			s+="\n";
			Debug.Log (s);
			c++;
		}
	}

	public static List<List<int>> Composition(List<int> source,int m)
	{
		int len = source.Count;
		if (len < m)
			return null;
		List<List<int>> result = new List<List<int>> ();
		List<int> temp = new List<int> ();
		bool[] comp = new bool[len];
		int i = 0;
		for (i = 0; i < len; i++)
			comp [i] = i < m;
		string s = "";
		int c = 0;
		for (int index = 0; index < source.Count; index++) {
			if (comp [index] == true) {
				s += source [index] + " ";
				temp.Add (source [index]);
			}
		}
		result.Add (temp);
		c++;
		s+="\n";
		//Debug.Log (s);

		while (true) {
			for (i = 0; i < len - 1; i++) {
				if (comp [i] == true && comp [i + 1] == false)
					break;
			}

			if (i == len - 1)
				return result;
			comp [i] = false;
			comp [i + 1] = true;

			int p = 0;
			while (p < i) {
				while (comp [p] == true)
					p++;
				while (i >= 0 && comp [i] == false)
					i--;
				if (p < i) {
					comp [p] = true;
					comp [i] = false;
				}
			}
			temp = new List<int> ();s = "";
			for (int index = 0; index < source.Count; index++) {
				if (comp [index] == true) {
					s += source [index] + " ";
					temp.Add (source [index]);
				}
			}
			result.Add (temp);
			s+="\n";
			Debug.Log (s);
			c++;
		}
		Debug.Log ("c=" + c);



		return result;
	}
}
