using System;
using UnityEngine;
using System.Collections;

public abstract class SequenceGenerator{
	protected float ArenaWidth = 25;
	protected float Interval = 0.5f;
	public float CountdownTimer = 0;

	protected void GenerateSoul(float x, float speed, float pBad){
		bool isBad = Random.value < pBad;
		//TODO: implement this (copy code from gamecontroller).
		throw new NotImplementedException();
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
	protected void NextSoul(){
		float x = ArenaWidth * 0.5f * Mathf.Sin(Time.time * w);
		GenerateSoul(x, 5, 0);
	}
}

public class TriangleSequence : SequenceGenerator{
	float w = 3 + Random.value * 6;
	protected void NextSoul(){
		float m = Time.time * w;
		int mI = (int)m;
		int ms = (mI % 2 == 0) ? -1 : 1;
		float r = m - mI - 0.5;
		float x = r * ms * ArenaWidth * 0.5f;
		GenerateSoul(x, 5, 0);
	}
}

public class LinearSequence : SequenceGenerator{
	int L = (int) (3 + Random.value * 6);
	int l = 0;
	float x = 0;
	protected void NextSoul(){
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
	protected void NextSoul(){
		int goodEvil = Mathf.RoundToInt(Random.value);
		float separation = 5 + Random.value * (ArenaWidth - 10) / 2;
		GenerateSoul(separation, 5, goodEvil);
		GenerateSoul(-separation, 5, 1 - goodEvil);
	}
}
public class RandomSequence : SequenceGenerator{
	public DichotomySequence(){
		Interval = 0.8;
	}
	protected void NextSoul(){
		float x = ArenaWidth * (Random.value - 0.5f);
		GenerateSoul(separation, 5, 0.1);
	}
}
public class NothingSequence : SequenceGenerator{
	protected void NextSoul(){}
}

//TODO: new sequencegenerator where you have to dodge holes of bad souls.

/* 
Create a new one of these and use it to control the game.
*/
public class LevelController{
	public int Hp
	{
		get { return hp;}
		set { hp = value;}
	}
	
	int soulsSaved = 0;
	int soulsThisLevel = 0;
	int soulsRequiredThisLevel = 50;
	long score = 0;
	int level = 1;
	int hp = 5;
	List<Type> allGeneratorTypes = null;

	SequenceGenerator currentSequence = null;
	float currentSequenceTimer = 0;

	public void Update(){
		currentSequenceTimer -= Time.deltaTime;
		if(currentSequenceTimer <= 0){
			currentSequenceTimer += 3 + Random.value * 10;
			currentSequence = new allGeneratorTypes[Mathf.Floor(allGeneratorTypes.Count * (Random.value * 0.999f))];
		}

		if(currentSequence != null){
			currentSequence.update();
		}
	}

	public void LevelUp(){
		level++;
		soulsThisLevel = 0;
		soulsRequiredThisLevel = 50 + (level - 1) * 10;

		allGeneratorTypes = new List<Type>();
		allGeneratorTypes.Add(RandomSequence);
		if(level > 2) allGeneratorTypes.Add(SinusoidSequence);
		if(level > 4) allGeneratorTypes.Add(TriangleSequence);
		if(level > 10) allGeneratorTypes.Add(LinearSequence);
		if(level > 15) allGeneratorTypes.Add(DichotomySequence);
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



