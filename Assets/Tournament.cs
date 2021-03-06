﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Tournament : Frame {

    private float headerHeight = 75;
    private TournamentBracket content;
	private TournamentNextup nextup;
	private TournamentQuadrants quadrants;
	private string tournName = "SIGN UP @ ROCKAGE.TK";

    protected override void Awake()
    {
        base.Awake();
        content = new GameObject(App.noName).AddComponent<TournamentBracket>();
        content.transform.parent = this.transform;

		nextup = new GameObject("Nextup").AddComponent<TournamentNextup>();
		nextup.transform.parent = this.transform;

		quadrants = new GameObject("Quadrants").AddComponent<TournamentQuadrants>();
		quadrants.transform.parent = this.transform;
    }

    string json = "{'update_time':'now','final_bracket':{'name':'','height':'3','num_descendants':'5','prev_brackets':[{'name':'','height':'2','num_descendants':'5','prev_brackets':[{'name':'','height':'1','num_descendants':'2','prev_brackets':[{'name':'Fox'},{'name':'Yoshi'}]},{'name':'','height':'1','num_descendants':'3','prev_brackets':[{'name':'Victor'},{'name':'Jode'},{'name':'Jake'}]}]}]}}";

    protected override void Start()
    {
        base.Start();
        //UpdateWithJson(json);
    }

    protected override void Update()
    {
        base.Update();
        float headerHeightPerc = headerHeight / rect.height;
        content.bounds.Set(0, headerHeightPerc, 1, 1 - headerHeightPerc);
		nextup.bounds.Set(0, headerHeightPerc, 1, 1 - headerHeightPerc);
		quadrants.bounds.Set(0, headerHeightPerc, 1, 1 - headerHeightPerc);

		if (Input.GetKey (KeyCode.F)) {
			Screen.fullScreen ^= true;
		}
    }

	GUIStyle style;
	GUIStyle styleO;
    void OnGUI() {
        //if (GUI.Button(rect, ""))
        //{
        //    UpdateWithJson(json);
        //}
        
		Rect r = new Rect (0, 0, rect.width, headerHeight);
		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.fontStyle = FontStyle.BoldAndItalic;
		style.normal.textColor = Color.red;
		style.fontSize = (int)(r.height/1.5f);
		
		styleO = new GUIStyle();
		styleO.alignment = TextAnchor.MiddleCenter;
		styleO.fontStyle = FontStyle.Bold;
		styleO.normal.textColor = Color.white;
		styleO.fontSize = (int)(r.height/1.5f);
		GUI.Label(r, tournName, styleO);
		GUI.Label(r, tournName, style);
    }

    public override void UpdateWithJson(string json)
    {
        print("JSON INCOMING! " + json);
		var all = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
		print (all);

		if (content != null)
			content.gameObject.SetActive(false);
		nextup.gameObject.SetActive(false);
		quadrants.gameObject.SetActive(false);
		if (all.ContainsKey ("brackets")) {

			var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(all["brackets"].ToString ());

			tournName = dict["name"].ToString ();

			if (content != null) {
				content.name = "Deleted";
				Destroy(content.gameObject);
			}

			if (dict["bracket"] != null) {
			
				var final = JsonConvert.DeserializeObject<Dictionary<string, object>>(dict["bracket"].ToString());

				content = new GameObject(App.noName).AddComponent<TournamentBracket>();
				content.transform.parent = this.transform;

				content.gameObject.SetActive(true);
				
				if (final["user"] != null)
				{
					var user = JsonConvert.DeserializeObject<Dictionary<string, string>>(final["user"].ToString());
					content.name = user["username"];
				}
				else
				{
					content.name = App.noName;
				}
				content.height = int.Parse(final["round_number"].ToString());
				content.numDescendants = int.Parse(final["num_descendants"].ToString());
				content.bounds.width = 1;
				
				content.UpdateWithJson(final["brackets"].ToString());

			}

		} else if (all.ContainsKey ("nextup")) {

			nextup.gameObject.SetActive(true);

			var list = JsonConvert.DeserializeObject<List<object>>(all["nextup"].ToString ());

			tournName = list[0].ToString ();

			var bList = JsonConvert.DeserializeObject<List<object>>(list[1].ToString ());
			var bracket = JsonConvert.DeserializeObject<Dictionary<string, object>>(bList[0].ToString ());
			print (bracket["brackets"].ToString ());
			nextup.UpdateWithJson(bracket["brackets"].ToString ());

		} else {

			tournName = "SIGN UP @ ROCKAGE.TK";

			print ("LETS GO");
			quadrants.gameObject.SetActive(true);

			quadrants.UpdateWithJson(all["quadrants"].ToString ());

		}
    }
}
