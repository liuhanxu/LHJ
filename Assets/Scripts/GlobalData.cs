using UnityEngine;
using System.Collections;

public class GlobalData
{
    public static string NextLevel = "Login";


    public static string userId = "";
    public static int playerTotal = 1000000;
    public static int curObtain = 0;
    public static int[] times = new int[] { 5, 10, 50, 100, 500 };
    public static int curTimeIndex = 0;

    /// <summary>
    /// 中奖类型
    /// 0-不中奖
    /// 1-小奖
    /// 2-多奖【小三元、大三元、大四喜、大满贯】
    /// </summary>
    public static int prizeType = 0;

}