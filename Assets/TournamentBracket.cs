﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class TournamentBracket : Frame {

    public int height = 0;
    public int numDescendants = 1;
    public bool isWinner { get; private set; }

    GUIStyle style;
    void Awake()
    {
        style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
    }

    Vector2 boxDim = new Vector2(10,25);
    protected override void OnGUI()
    {
        base.OnGUI();

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

            string n = prevBracket["name"].ToString();
            bracket.name = (n != "") ? n : App.noName;
            if (prevBracket.ContainsKey("prev_brackets"))
            {
                bracket.height = int.Parse(prevBracket["height"].ToString());
                bracket.numDescendants = int.Parse(prevBracket["num_descendants"].ToString());
            }
            float h = bracket.numDescendants / (float)numDescendants;
            bracket.bounds.Set(0, y, width, h);
            y += h;
            if (prevBracket.ContainsKey("prev_brackets"))
            {
                bracket.UpdateWithJson(prevBracket["prev_brackets"].ToString());
            }
        }
    }
}