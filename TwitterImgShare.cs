using UnityEngine;
using System.Collections;
using System.IO;

public class TwitterImgShare : MonoBehaviour {
	public GoogleAnalyticsV4 googleAnalytics;
	public GameObject GameOver;


	private float mySize;
	public bool size;
	public bool sizeMe;

	private int score;

	private bool isProcessing = false;

	private string Text;
	private string gameLink = "Join the race here: "+"www.mesmeracer.com\n";
	private string subject = "\n";



	void OnEnable(){
		mySize = 1.1f;
		StartCoroutine (shouldSize ());

	}

	void OnMouseDown()
	{

		googleAnalytics.LogEvent ("Twitter", "Shared", "Amount", 1);


		PlayerPrefs.SetInt ("A18", 1);	
		score = PlayerPrefs.GetInt ("score");
		int rnd = Random.Range (1, 4);
		if(rnd==1)
			Text  = score.ToString() + " points! Catch me if you can!\n";
		if(rnd==2)
			Text  = "Pedal to the metal! Try to beat my "+ score.ToString() +" score!\n";
		if(rnd==3)
			Text  = "Pedal to the me...smeracer! Try to beat "+ score.ToString() +"\n";
		Debug.Log ("OnMouse");

		byte[] dataToSave = Resources.Load<TextAsset>("image").bytes;

		string destination = Path.Combine(Application.persistentDataPath,System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
		Debug.Log(destination);
		File.WriteAllBytes(destination, dataToSave);


		Share(Text + subject + gameLink,destination,"");
	}


	public void Share(string shareText, string imagePath, string url, string subject = "")
	{
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagePath);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
		intentObject.Call<AndroidJavaObject>("setType", "image/png");

		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText);

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

		AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, subject);
		currentActivity.Call("startActivity", jChooser);

	}

	void Update(){
		reSize ();
	}


	void reSize()
	{
		if (GameOver.GetComponent<GameOverScript> ().newHigh == true) {

			if (sizeMe == true) {
				if (mySize <= 1.1f) {
					size = true;
				} else if (mySize >= 1.3f) {
					size = false;
				}
				if (size == true) {
					this.gameObject.transform.localScale = new Vector3 (mySize, mySize, mySize);
					mySize = mySize + 0.02f;

				} else if (size == false) {
					this.gameObject.transform.localScale = new Vector3 (mySize, mySize, mySize);
					mySize = mySize - 0.02f;
				}
			} else
				this.gameObject.transform.localScale = new Vector3 (mySize, mySize, mySize);
		}


	}

	IEnumerator shouldSize()
	{
		yield return new WaitForSeconds (0.5f);
		sizeMe = true;
		yield return new WaitForSeconds (1f);
		sizeMe = false;
		yield return new WaitForSeconds (1f);

		StartCoroutine (shouldSize ());




	}



}