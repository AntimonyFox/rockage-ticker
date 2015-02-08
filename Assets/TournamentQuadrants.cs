using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TournamentQuadrants : Frame {

	private List<TournamentQuadrant> quadrants = new List<TournamentQuadrant>();

	GUIStyle style;
	GUIStyle styleO;
	void Awake()
	{
		base.Awake ();
		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.fontStyle = FontStyle.Bold;

		var q1 = new GameObject("Quad0").AddComponent<TournamentQuadrant>();
		q1.bounds = new Rect (0,0,0.5f,0.5f);
		q1.transform.parent = this.transform;
		quadrants.Add (q1);

		var q2 = new GameObject("Quad1").AddComponent<TournamentQuadrant>();
		q2.bounds = new Rect (0.5f,0,0.5f,0.5f);
		q2.transform.parent = this.transform;
		quadrants.Add (q2);

		var q3 = new GameObject("Quad2").AddComponent<TournamentQuadrant>();
		q3.bounds = new Rect (0,0.5f,0.5f,0.5f);
		q3.transform.parent = this.transform;
		quadrants.Add (q3);

		var q4 = new GameObject("Quad3").AddComponent<TournamentQuadrant>();
		q4.bounds = new Rect (0.5f,0.5f,0.5f,0.5f);
		q4.transform.parent = this.transform;
		quadrants.Add (q4);
	}
	
	protected override void OnGUI()
	{
		base.OnGUI();

	}
	
	public override void UpdateWithJson(string json)
	{
		print ("GETTING AT QUADS " + json);

		var list = JsonConvert.DeserializeObject<List<object>> (json);

		for (int i = 0; i < quadrants.Count; i++) {
			var q = quadrants[i];
			if (list[i] != null)
				q.UpdateWithJson (list [i].ToString ());
			else
				q.UpdateWithJson ("");
		}
		
	}
}
