using UnityEngine;
using System.Collections;

public class fade : MonoBehaviour {
	
	private AnimationCurve easeInOut;
	private float duration=0f ;
	private float time=0f;
	
	void SetAlpha(float alpha)
	{
		Color c = GetComponent<GUITexture> ().color;
		c.a = alpha;
		GetComponent<GUITexture> ().color = c;
	}
	void Update()
	{
		if (duration == 0f || Time.deltaTime > 0.08f)
			return; // missed a frame
		float alpha = easeInOut.Evaluate(time); 
		SetAlpha (alpha);
		Debug.Log ("time = " + time + " delta Time = " + Time.deltaTime + " alpha = " + alpha);
		// GetComponent<GUITexture> ().color.a = alpha; Can't do this
		if (time > duration  ) 
		{
			if (alpha <= 0.01f)
			{
				gameObject.SetActive(false);
				Debug.Log ("FADE IN SCRIPT DONE @ t=" + Time.time + " elapsed = " + time);
			}
			else
				Debug.Log ("FADE OUT SCRIPT DONE @ t=" + Time.time + " elapsed = " + time);
			transform.parent.gameObject.GetComponent<GameController>().FadeDone();
			duration = 0f;
			time = 0f;
		}
		else
		 	time += Time.deltaTime;
	}
	void Activate(float fadeTime)
	{
		gameObject.SetActive(true);
		time = 0.0f;
		duration = fadeTime;
		GetComponent<GUITexture>().pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
	}
	public void FadeIn(float fadeTime)
	{
		easeInOut = AnimationCurve.EaseInOut (0.0f, 1.0f, fadeTime, 0.0f);
		SetAlpha (1.0f);
		Activate (fadeTime);
		Debug.Log ("FADE IN STARTED @ t=" + Time.time + " duration = " + fadeTime);
	}
	public void FadeOut(float fadeTime)	
	{
		easeInOut = AnimationCurve.EaseInOut (0.0f, 0.0f, fadeTime, 1.0f);
		SetAlpha (0.0f);
		Activate (fadeTime);
		Debug.Log ("FADE OUT STARTED @ t=" + Time.time + " duration = " + fadeTime);
	}


}
