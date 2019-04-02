using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SequenceGenerator{

	public GameController GameController;
	public GameObject Soul;

	public GameObject BadSoul;

	protected float ArenaWidth = 25;
	protected float Interval = 0.5f;
	public float CountdownTimer = 0;

	protected void GenerateSoul(float x, float speed, float pBad){
		bool isBad = Random.value < pBad;
		//TODO: implement this (copy code from gamecontroller).
		GameObject soul;
		if(isBad)
		{
			soul = GameObject.Instantiate(BadSoul, new Vector3(x, 20, 0), Quaternion.identity);
		}
		else
		{
			soul = GameObject.Instantiate(Soul, new Vector3(x, 20, 0), Quaternion.identity);
		}
		
		soul.GetComponent<SoulController>().GameController = this.GameController;
	}
	public void Update(){
		CountdownTimer -= Time.deltaTime;
		if(CountdownTimer < 0){
			NextSoul();
			CountdownTimer += Interval;
		}
	}

	protected abstract void NextSoul();
}

public class SinusoidSequence : SequenceGenerator{
	float w = 3 + Random.value * 6;
	protected override void NextSoul(){
		float x = ArenaWidth * 0.5f * Mathf.Sin(Time.time * w);
		GenerateSoul(x, 5, 0);
	}
}

public class TriangleSequence : SequenceGenerator{
	float w = 3 + Random.value * 6;
	protected override void NextSoul(){
		float m = Time.time * w;
		int mI = (int)m;
		int ms = (mI % 2 == 0) ? -1 : 1;
		float r = m - mI - 0.5f;
		float x = r * ms * ArenaWidth * 0.5f;
		GenerateSoul(x, 5, 0);
	}
}

public class LinearSequence : SequenceGenerator{
	int L = (int) (3 + Random.value * 6);
	int l = 0;
	float x = 0;
	protected override void NextSoul(){
		l--;
		if(l <= 0) {
			l = L;
			x = ArenaWidth * (Random.value - 0.5f);
			Interval = 0.7f;
		}else{
			GenerateSoul(x, 5, 0);
			Interval = 0.25f;
		}
	}
}

public class DichotomySequence : SequenceGenerator{
	public DichotomySequence(){
		Interval = 1;
	}
	protected override void NextSoul(){
		int goodEvil = Mathf.RoundToInt(Random.value);
		float separation = 5 + Random.value * (ArenaWidth - 10) / 2;
		GenerateSoul(separation, 5, goodEvil);
		GenerateSoul(-separation, 5, 1 - goodEvil);
	}
}
public class RandomSequence : SequenceGenerator{
	public RandomSequence(){
		Interval = 0.8f;
	}
	protected override void NextSoul(){
		float x = ArenaWidth * (Random.value - 0.5f);
		GenerateSoul(x, 5, 0.1f);
	}
}
public class NothingSequence : SequenceGenerator{
	protected override void NextSoul(){}
}

//TODO: new sequencegenerator where you have to dodge holes of bad souls.

/* 
Create a new one of these and use it to control the game.
*/
public class LevelController{

	private GameController GameController;

	GameObject Soul;

	GameObject BadSoul;

	public int Hp
	{
		get { return hp;}
		set { hp = value;}
	}
	
	int soulsSaved = 0;
	int soulsThisLevel = 0;
	int soulsRequiredThisLevel = 50;
	long score = 0;
	int level = 0;
	int hp = 5;
	List<System.Type> allGeneratorTypes = null;

	SequenceGenerator currentSequence = null;
	float currentSequenceTimer = 0;

	public LevelController(GameController GameController, GameObject Soul, GameObject BadSoul)
	{
		this.GameController = GameController;
		this.Soul = Soul;
		this.BadSoul = BadSoul;
		LevelUp();
	}

	public void Update(){
		currentSequenceTimer -= Time.deltaTime;
		if(currentSequenceTimer <= 0){
			currentSequenceTimer += 3 + Random.value * 10;
			System.Type T = allGeneratorTypes[Mathf.FloorToInt(allGeneratorTypes.Count * (Random.value * 0.999f))];
			currentSequence = (SequenceGenerator)System.Activator.CreateInstance(T);
			currentSequence.GameController = this.GameController;
			currentSequence.Soul = this.Soul;
			currentSequence.BadSoul = this.BadSoul;
		}

		if(currentSequence != null){
			currentSequence.Update();
		}
	}

	public void LevelUp(){
		level++;
		if(level > 1) GameController.Notify("Level " + level);
		soulsThisLevel = 0;
		soulsRequiredThisLevel = 50 + (level - 1) * 10;

		allGeneratorTypes = new List<System.Type>();
		allGeneratorTypes.Add(typeof(RandomSequence));
		if(level > 2) allGeneratorTypes.Add(typeof(SinusoidSequence));
		if(level > 4) allGeneratorTypes.Add(typeof(TriangleSequence));
		if(level > 10) allGeneratorTypes.Add(typeof(LinearSequence));
		if(level > 15) allGeneratorTypes.Add(typeof(DichotomySequence));
		currentSequence = new NothingSequence();
		currentSequence.CountdownTimer = 2;
	}

	public void GivePoint(){
		soulsSaved++;
		soulsThisLevel++;
		score += level;

		if(soulsThisLevel >= soulsRequiredThisLevel){
			LevelUp();
		}
	}

	public void GiveHp(){
		hp++;
	}
	public void TakeHp(){
		hp--;
	}
}