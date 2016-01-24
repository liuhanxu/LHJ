using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioSource bg_source, clip_source, extra_scource;
   
    //同一source
    [SerializeField]
    AudioClip[] btntap_clips;
    [SerializeField]
    AudioClip[] runstart_clips;
    [SerializeField]
    AudioClip[] runmid_clips;
    [SerializeField]
    AudioClip[] runend_clips;

    [SerializeField]
    AudioClip[] prizebg_clips;
    [SerializeField]
    AudioClip[] smallprize_clips;

    [SerializeField]
    AudioClip moveback_clip,bigprizerev_clip,boom_clip;

    [SerializeField]
    //不同一source//大三元等
    AudioClip[] bigprize_clips;


    // Use this for initialization
    void Start()
    {
        RegistHandlers();
    }

    void OnDestroy()
    {
        UnregistHndlers();
    }


    public void BtnTap(int id=0)
    {
        if (id >= btntap_clips.Length)
            id = 0;

        bg_source.clip = btntap_clips[id];
        bg_source.loop = false;
        bg_source.Play();
    }

    public float StartRun(int id=0)
    {
        if (id >= runstart_clips.Length)
            id = 0;
        clip_source.clip = runstart_clips[id];
        clip_source.loop = false;
        clip_source.Play();
        return runstart_clips[id].length;
    }

    public void MidRun(float loopTime,float delay,int id=0)
    {
        if (id >= runmid_clips.Length)
            id = 0;
        StartCoroutine(run(loopTime,delay,id));
    }

    IEnumerator run(float loopTime, float delay,int id)
    { 
        yield return new WaitForSeconds(delay);
        clip_source.clip = runmid_clips[id];
        clip_source.loop = true;
        clip_source.Play();
        yield return new WaitForSeconds(loopTime);
    }

    public void EndRun(float delay,int id =0)
    {
        if (id >= runend_clips.Length)
            id = 0;
        StartCoroutine(end(delay, id));
    }

    IEnumerator end(float delay, int id)
    {
        yield return new WaitForSeconds(delay);
        clip_source.clip = runend_clips[id];
        clip_source.loop = false;
        clip_source.Play();
    }

    public void Prize(int id)
    {
        if (id >= prizebg_clips.Length)
            id = 0;
        clip_source.clip = prizebg_clips[id];
        clip_source.loop = false;
        clip_source.Play();
    }

    public void MoveBackTick(float delay)
    {
        StartCoroutine(moveback(delay));
    }

    IEnumerator moveback(float delay)
    {
        yield return new WaitForSeconds(delay);
        clip_source.clip = moveback_clip;
        clip_source.loop = true;
        clip_source.Play();
    }

    public void BigPrizeReverse(float looptime)
    {
        clip_source.clip = bigprizerev_clip;
        clip_source.loop = true;
        clip_source.Play();
    }

    IEnumerator rev(float looptime)
    {
        clip_source.clip = bigprizerev_clip;
        clip_source.loop = true;
        clip_source.Play();
        yield return new WaitForSeconds(looptime);
       // clip_source.Stop();
    }

    public void Boom()
    {
        extra_scource.clip = boom_clip;
        extra_scource.loop = false;
        extra_scource.Play(); 
    }

    /// <summary>
    /// 具体水果
    /// </summary>
    /// <param name="id"></param>
    public void SmallPrize(int id)
    {
        if (id >= smallprize_clips.Length)
            id = 0;
        extra_scource.clip = smallprize_clips[id];
        extra_scource.loop = false;
        extra_scource.Play();
    }

    /// <summary>
    /// 大三元等
    /// </summary>
    /// <param name="id"></param>
    public void BigPrize(int id)
    {
        if (id >= bigprize_clips.Length)
            id = 0;
        extra_scource.clip = bigprize_clips[id];
        extra_scource.loop = false;
        extra_scource.Play();
    }





   

    void onSoundSett(object para)
    {
        clip_source.volume = (float)para;
        bg_source.volume = (float)para;
        extra_scource.volume = (float)para;
    }

    void RegistHandlers()
    {
        EventSystem.Instance.RegistEvent(EventCode.SoundSetting, onSoundSett);
    }

    void UnregistHndlers()
    {
        EventSystem.Instance.UnregistEvent(EventCode.SoundSetting, onSoundSett);
    }
}
