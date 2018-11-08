using UnityEngine;
using System.Collections;


namespace UnitySampleAssets.CrossPlatformInput.PlatformSpecific
{
public class BackgroundScroller : MonoBehaviour {


	
	//CONTROLL THE SCORLLING SPEED
	public float speed = 0.51f;
	
	//AND UPDATE THE OFFSET ACCORDINGLY
	void Update () {
		renderer.material.mainTextureOffset = new Vector2(Time.time*speed,0f);
	}
}
}