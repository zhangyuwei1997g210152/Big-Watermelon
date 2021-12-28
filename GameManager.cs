using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 默认Ready；
/// 点击开始到鼠标点击控制水果位置，StandBy；
/// 松开鼠标水果跌落，InProgress
/// 水果跌落碰撞到地板或者其它水果之后，回滚StandBy
/// 水果超出边界，GameOver
/// 游戏结束之后，延迟0.5s，计算分数 CalculateScore
/// </summary>
public enum GameState
{
    Ready = 0,
    StandBy = 1,//StandBy~Inprogress
    InProgress = 2,//Inprogress~StandBy
	GameOver = 3,
    CalculateScore = 4,
}

public class GameManager : MonoBehaviour {
	public GameObject[] fruitList;
	public GameObject bornFruitPosition;

	public GameObject startBtn;

	public static GameManager gameManagerInstance;//静态的实例，可以直接在别的类中使用

	public GameState gameState = GameState.Ready;

	public Vector3 combineScale = new Vector3(0,0,0);

	public float TotalScore = 0f;
	public Text totalScore;
	public Text highestScoreText;

	public AudioSource combineSource;
	public AudioSource hitSource;

	//在游戏对象启用之前，调用一次
	void Awake()
    {
		gameManagerInstance = this;
	}
	// Use this for initialization
	//在游戏对象启用的时候，调用一次
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void StartGame()
    {
		//游戏开始
		Debug.Log("start");

		float highestScore = PlayerPrefs.GetFloat("HighestScore");
		highestScoreText.text = "历史最高：" + highestScore;

		CreateFruit();
		gameState = GameState.StandBy;//step1
		startBtn.SetActive(false);
	}
	public void InvokeCreateFruit(float invokeTime)
    {
		Invoke("CreateFruit", invokeTime);
    }
	//待命水果无重力
	public void CreateFruit()
	{
		int index = Random.Range(0, 5);//0,1,2,3,4
		if (fruitList.Length >= index && fruitList[index] != null)
		{
			GameObject fruitObj = fruitList[index];
			var currentFruit = Instantiate(fruitObj, bornFruitPosition.transform.position, fruitObj.transform.rotation);
			currentFruit.GetComponent<Fruit>().fruitState = FruitState.StandBy;//Fruit Step1
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="currentFruitType">当前碰撞的水果类型</param>
	/// <param name="currentPos">当前碰撞的水果位置</param>
	/// <param name="collisionPos">碰撞的对方位置</param>
	/// 合成的水果需要有重力
	public void CombineNewFruit(FruitType currentFruitType,Vector3 currentPos,Vector3 collisionPos)
    {
		Vector3 centerPos = (currentPos + collisionPos) / 2;
		int index = (int)currentFruitType + 1;
		GameObject combineFruitObj = fruitList[index];
		var combineFruit = Instantiate(combineFruitObj, centerPos, combineFruitObj.transform.rotation);
		combineFruit.GetComponent<Rigidbody2D>().gravityScale = 1f;
		combineFruit.GetComponent<Fruit>().fruitState = FruitState.Collision;
		combineFruit.transform.localScale = combineScale;

		combineSource.Play();
	}


}
