using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Tournament : Frame {

    private float headerHeight = 25;
    private TournamentBracket content;
	private TournamentNextup nextup;

    protected override void Awake()
    {
        base.Awake();
        content = new GameObject(App.noName).AddComponent<TournamentBracket>();
        content.transform.parent = this.transform;

		nextup = new GameObject("Nextup").AddComponent<TournamentNextup>();
		nextup.transform.parent = this.transform;
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
		nextup.bounds.Set(0, 0, 1, 1);

		if (Input.GetKey (KeyCode.F)) {
			Screen.fullScreen ^= true;
		}
    }

    void OnGUI() {
        //if (GUI.Button(rect, ""))
        //{
        //    UpdateWithJson(json);
        //}
        
    }

    public override void UpdateWithJson(string json)
    {
        print("JSON INCOMING! " + json);
		var all = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
		print (all);

		content.enabled = false;
		nextup.enabled = false;
		if (all.ContainsKey ("brackets")) {

			content.enabled = true;

			var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(all["brackets"].ToString ());
			
			var final = JsonConvert.DeserializeObject<Dictionary<string, object>>(dict["bracket"].ToString());
			content.name = "Deleted";
			Destroy(content.gameObject);
			content = new GameObject(App.noName).AddComponent<TournamentBracket>();
			content.transform.parent = this.transform;
			
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

		} else if (all.ContainsKey ("nextup")) {

			nextup.enabled = true;

			var list = JsonConvert.DeserializeObject<List<object>>(all["nextup"].ToString ());

			var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(list[0].ToString ());

			nextup.UpdateWithJson(dict["brackets"].ToString ());

		} else {

		}
    }
}
