using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public HttpClient httpClient;


    void Awake()
    {
        Instance = this;
        httpClient = transform.GetComponent<HttpClient>();
    }


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
