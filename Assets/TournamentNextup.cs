using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TournamentNextup : Frame {

	private List<string> names = new List<string>();

	GUIStyle style;
	void Awake()
	{
		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.fontStyle = FontStyle.Bold;
	}
	
	protected override void OnGUI()
	{
		base.OnGUI();
		
		GUI.Box (rect, "");
	}
	
	public override void UpdateWithJson(string json)
	{
		print ("GETTING " + json);

		names.Clear ();

		var list = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
		foreach (var bracket in list) {

			var user = JsonConvert.DeserializeObject<Dictionary<string, object>>(bracket["user"].ToString ());
			names.Add (user["username"].ToString ());

		}


	}
}
