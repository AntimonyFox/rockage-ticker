using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ScheduleStageEvent : Frame {

    private new string name;
    private enum Status { active, upcoming }
    private Status status;
    public DateTime when { get; private set; }
    public TimeSpan duration { get; private set; }

    public override void UpdateWithJson(string json)
    {
        var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        name = dict["name"];
        //status = (Status)Enum.Parse(typeof(Status), dict["status"]);
        when = DateTime.Parse(dict["when"]).ToUniversalTime();
        print(name + " : " + when);
        duration = TimeSpan.FromMinutes(int.Parse(dict["duration"]));
    }

    private Texture solid_active = Resources.Load<Texture>("solid_active");
    private Texture solid_upcoming = Resources.Load<Texture>("solid_upcoming");
    protected override void OnGUI()
    {
        Rect parentRect = transform.parent.GetComponent<Frame>().rect;
        GUI.BeginGroup(parentRect);

        GUI.depth = 0;
        if (when + duration >= DateTime.Now)
        {
            Rect r = new Rect(0, rect.y - parentRect.y, rect.width, rect.height);
            Texture color = (status == Status.active) ? solid_active : solid_upcoming;
            GUI.DrawTexture(r, color);
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.BoldAndItalic;
            style.normal.textColor = Color.black;
            GUI.Label(r, name, style);
        }

        GUI.EndGroup();
    }
}
