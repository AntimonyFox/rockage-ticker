using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TournamentQuadrant : Frame {

	public string tournName = "";
	string maxNumEntries = "";
	string spacesLeft = "";

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

		if (tournName != "") {
		
				style = new GUIStyle ();
				style.alignment = TextAnchor.MiddleCenter;
				style.fontStyle = FontStyle.BoldAndItalic;
				style.normal.textColor = Color.red;
				style.fontSize = (int)(rect.height / 8f);
	
				styleO = new GUIStyle ();
				styleO.alignment = TextAnchor.MiddleCenter;
				styleO.fontStyle = FontStyle.Bold;
				styleO.normal.textColor = Color.white;
				styleO.fontSize = (int)(rect.height / 8f);
	
				Rect r = new Rect ();
				r.width = rect.width;
				r.height = rect.height / 8;
				r.x = rect.x + rect.width / 2 - r.width / 2;
				r.y = rect.y + rect.height / 6;
				GUI.Label (r, tournName, styleO);
				GUI.Label (r, tournName, style);

			style.fontSize = (int)(rect.height / 8f);
			styleO.fontSize = (int)(rect.height / 8f);

				string label = "Tournament size: " + maxNumEntries;
				style.normal.textColor = Color.blue;
				r.y += rect.height / 4;

				GUI.Label (r, label, styleO);
				GUI.Label (r, label, style);

			label = "Spaces left: " + spacesLeft;
			style.normal.textColor = Color.green;
			r.y += rect.height / 4;

			GUI.Label (r, label, styleO);
			GUI.Label (r, label, style);

		}
		
	}
	
	public override void UpdateWithJson(string json)
	{
		print ("GETTING AT JUST ONE" + json);
		
		var dict = JsonConvert.DeserializeObject<Dictionary<string,object>> (json);
		tournName = dict ["name"].ToString ();
		maxNumEntries = dict ["max_num_entries"].ToString ();
		spacesLeft = dict ["spaces_available"].ToString ();
		
	}
}
