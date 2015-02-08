using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TournamentNextup : Frame {

	private List<string> names = new List<string>();

	GUIStyle style;
	GUIStyle styleO;
	void Awake()
	{
		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.fontStyle = FontStyle.Bold;
	}
	
	protected override void OnGUI()
	{
		base.OnGUI();

		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.fontStyle = FontStyle.BoldAndItalic;
		style.normal.textColor = Color.red;
		style.fontSize = (int)(rect.height/8f);
		
		styleO = new GUIStyle();
		styleO.alignment = TextAnchor.MiddleCenter;
		styleO.fontStyle = FontStyle.Bold;
		styleO.normal.textColor = Color.white;
		styleO.fontSize = (int)(rect.height/8f);

		Rect r = new Rect ();
		r.width = rect.width;
		r.height = rect.height / 8;
		r.x = rect.width / 2 - r.width / 2;
		r.y = rect.y;
		GUI.Label(r, name, styleO);
		GUI.Label(r, name, style);

		style.normal.textColor = Color.blue;
		float howMuchLeft = rect.height - r.height;
		float per = howMuchLeft / (names.Count * 2);

		r.y += per;
		if (names.Count > 0) {
				GUI.Label (r, names [0], styleO);
				GUI.Label (r, names [0], style);
				for (int i = 1; i < names.Count; i++) {
						r.y += per;
						GUI.Label (r, "vs", styleO);
						GUI.Label (r, "vs", style);

						r.y += per;
						GUI.Label (r, names [i], styleO);
						GUI.Label (r, names [i], style);
				}
		}
	}
	
	public override void UpdateWithJson(string json)
	{
		print ("GETTING " + json);

		names.Clear ();

		var list = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
		foreach (var bracket in list) {

			if (bracket["user"] != null) {
				var user = JsonConvert.DeserializeObject<Dictionary<string, object>>(bracket["user"].ToString ());
				names.Add (user["username"].ToString ());
			}

		}


	}
}
