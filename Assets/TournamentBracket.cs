using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TournamentBracket : Frame {

    public int height = 0;
    public int numDescendants = 1;
    public bool isWinner { get; private set; }

    GUIStyle style;
	GUIStyle styleO;
    void Awake()
    {
        
    }

    Vector2 boxDim = new Vector2(10,15);
    protected override void OnGUI()
    {
        base.OnGUI();

		boxDim.y = (int)((rect.height/2.5f)/(height+1));

		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter;
		style.fontStyle = FontStyle.BoldAndItalic;
		style.normal.textColor = Color.red;
		style.fontSize = (int)((rect.height/2.5f)/(height+1));
		
		styleO = new GUIStyle();
		styleO.alignment = TextAnchor.MiddleCenter;
		styleO.fontStyle = FontStyle.Bold;
		styleO.normal.textColor = Color.white;
		styleO.fontSize = (int)((rect.height/2.5f)/(height+1));

        var parent = transform.parent.GetComponent<TournamentBracket>();
        isWinner = false;
        if (name != App.noName)
        {
            if (parent == null || (parent.name == this.name && parent.isWinner) || parent.name == App.noName)
            {
                isWinner = true;
            }
        }
        var color = (!isWinner) ? App.colorTournOff : App.colorTournOn;
        //var color = (name == "<entry>" || (parent != null && parent.name != this.name)) ? App.color1 : App.color4;

        Rect r = new Rect();
        float sliceWidth = rect.width / (height + 1);
        r.width = rect.width / (height + 1) - boxDim.x * 2;
        r.height = boxDim.y;
        r.x = sliceWidth * height + boxDim.x;
        r.y = rect.y + rect.height / 2 - 12.5f;
        //GUI.color = new Color(1, 1, 1, 0.2f);
        GUI.DrawTexture(r, color);
        GUI.Label(r, name, styleO);
		GUI.Label(r, name, style);

        if (transform.childCount > 0)
        {
            Rect lineH = new Rect();
            lineH.x = r.x - boxDim.x - 2;
            lineH.y = r.y + r.height / 2 - 1;
            lineH.width = r.x - lineH.x;
            lineH.height = 2;
            GUI.DrawTexture(lineH, color);
        }

        if (parent != null)
        {
            var color2 = (isWinner && parent.name != App.noName) ? App.colorTournOn : App.colorTournOff;

            Rect lineH = new Rect();
            lineH.x = r.x + r.width;
            lineH.y = r.y + r.height / 2 - 1;
            lineH.width = rect.x + rect.width - lineH.x;
            lineH.height = 2;
            GUI.DrawTexture(lineH, color2);

            Rect lineV = new Rect();
            lineV.x = lineH.x + lineH.width - 2;
            lineV.y = lineH.y;
            lineV.width = 2;
            var pRect = parent.rect;
            lineV.height = pRect.y + pRect.height / 2 - lineH.y;
            GUI.DrawTexture(lineV, color2);
        }

        //GUI.Box(rect, name, style);
    }

    public override void UpdateWithJson(string json)
    {
        var list = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
        float y = 0;
        int maxHeight = Mathf.RoundToInt((1 / bounds.width) * (height + 1));
        float width = height / (float)(height + 1);
        foreach (var prevBracket in list)
        {
            var bracket = new GameObject(App.noName).AddComponent<TournamentBracket>();
            bracket.transform.parent = this.transform;

            if (prevBracket["user"] != null)
            {
                var user = JsonConvert.DeserializeObject<Dictionary<string, string>>(prevBracket["user"].ToString());
                bracket.name = user["username"];
            }
            else
            {
                bracket.name = App.noName;
            }
            
            if (prevBracket.ContainsKey("brackets"))
            {
                bracket.height = int.Parse(prevBracket["round_number"].ToString());
                bracket.numDescendants = int.Parse(prevBracket["num_descendants"].ToString());
            }
            float h = bracket.numDescendants / (float)numDescendants;
            bracket.bounds.Set(0, y, width, h);
            y += h;
            if (prevBracket.ContainsKey("brackets"))
            {
                bracket.UpdateWithJson(prevBracket["brackets"].ToString());
            }
        }
    }
}
