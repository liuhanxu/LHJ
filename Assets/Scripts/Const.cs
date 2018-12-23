using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Const
{
	/// 调试模式-用于内部测试
	public static bool DebugMode = false;
	/// 调试模式
	public static bool UpdateMode = false;
	/// 使用测试桩.
	public static bool UseMock = true;
	/// The timer interval.
	public static int TimerInterval = 1;
	/// FPS
	public static int GameFrameRate = 30;
	/// The use pbc.
	public static bool UsePbc = true;
	/// The use lpeg.
	public static bool UseLpeg = true;
	/// Protobuff-lua-gen
	public static bool UsePbLua = true;
	/// CJson
	public static bool UseCJson = true;
	/// 使用LUA编码
	public static bool LuaEncode = false;
	/// 用户ID
	public static string UserId = string.Empty;
	/// 应用程序名称
	public static string AppName = "Q3";
	/// 应用程序前缀
	public static string AppPrefix = AppName + "_";
	/// 素材扩展名
	public static string ExtName = ".unity3d";
	/// 素材目录
	public static string AssetDirname = "StreamingAssets";
	/// 服务器地址
	public static string WebUrl = "http://10.10.52.141:8080";
	/// Socket服务器端口
	public static int SocketPort = 0;
	/// Socket服务器地址
	public static string SocketAddress = string.Empty;

	public static string PopViewRoot = "Canvas/PopViews";
	public static string CDN = "";
	public static string NextLevel = "Start";

	public static Color blueColor = new Color (0.016f,0.37f,0.66f);
	public static Color greenColor = new Color (0.6f, 0.35f, 0.03f);
	public static Color goldColor = new Color (1, 0, 0);

	public static int MAXTIME = 99;

	public static string NoNetworkInfo = "无法连接到网络，请检查网络连接！";

    public static int[] PRIZE = { 10, 20, 100,200 ,10,5,20,40,20,0,10,15,30,20,40,80,10,10,20,60,30,0,10,40};

 
//0-苹果 10
//1-铃铛 20
//2-小BAR 100
//3-BAR 200
//4-苹果 10
//5-小苹果 5
//6-橘子 20
//7-西瓜
//8-小西瓜
//9-幸运位置
//10-苹果
//11-小柠檬
//12-柠檬
//13-铃铛
//14-小77
//15-77
//16-苹果
//17-小橘子
//18-橘子
//19-双星
//20-小双星
//21-幸运位置
//22-苹果
//23-铃铛"
}