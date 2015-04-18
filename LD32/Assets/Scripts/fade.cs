using UnityEngine;
using System.Collections;

public class fade : MonoBehaviour {
	
	private AnimationCurve easeInOut;
	private float duration ;
	private float time;
	
	void SetAlpha(float alpha)
	{
		Color c = GetComponent<GUITexture> ().color;
		c.a = alpha;
		GetComponent<GUITexture> ().color = c;
	}
	void Update()
	{
		if (Time.deltaTime > 0.08f)
			return; // missed a frame
		float alpha = easeInOut.Evaluate(time); 
		SetAlpha (alpha);
		// GetComponent<GUITexture> ().color.a = alpha; Can't do this
		Debug.Log ("time = " + time + " delta Time = " + Time.deltaTime + " alpha = " + alpha);
		if (time > duration  ) {
			if (alpha <= 0.01f)
			{
				Debug.Log ("FADE SCRIPT DONE @ t=" + Time.time + " elapsed = " + time);
				Destroy (gameObject);
			}
			else
			{
				// finished fade out....
				//FadeIn(duration);
			}
		}
		else
		 	time += Time.deltaTime;
	}
	void Activate(float fadeTime)
	{
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
