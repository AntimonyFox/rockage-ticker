using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Tournament : Frame {

    private float headerHeight = 25;
    private TournamentBracket content;

    protected override void Awake()
    {
        base.Awake();
        content = new GameObject(App.noName).AddComponent<TournamentBracket>();
        content.transform.parent = this.transform;
    }

    string json = "{'update_time':'now','final_bracket':{'name':'','height':'3','num_descendants':'5','prev_brackets':[{'name':'','height':'2','num_descendants':'5','prev_brackets':[{'name':'','height':'1','num_descendants':'2','prev_brackets':[{'name':'Fox'},{'name':'Yoshi'}]},{'name':'','height':'1','num_descendants':'3','prev_brackets':[{'name':'Victor'},{'name':'Jode'},{'name':'Jake'}]}]}]}}";

    protected override void Start()
    {
        base.Start();
        UpdateWithJson(json);
    }

    protected override void Update()
    {
        base.Update();
        float headerHeightPerc = headerHeight / rect.height;
        content.bounds.Set(0, headerHeightPerc, 1, 1 - headerHeightPerc);
    }

    void OnGUI() {
        //if (GUI.Button(rect, ""))
        //{
        //    UpdateWithJson(json);
        //}
        
    }

    public override void UpdateWithJson(string json)
    {
        var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        var final = JsonConvert.DeserializeObject<Dictionary<string, object>>(dict["final_bracket"].ToString());
        content.name = "Deleted";
        Destroy(content.gameObject);
        content = new GameObject(App.noName).AddComponent<TournamentBracket>();
        content.transform.parent = this.transform;

        string name = final["name"].ToString();
        content.name = (name != "") ? name : App.noName;
        content.height = int.Parse(final["height"].ToString());
        content.numDescendants = int.Parse(final["num_descendants"].ToString());
        content.bounds.width = 1;
        
        content.UpdateWithJson(final["prev_brackets"].ToString());
    }
}
