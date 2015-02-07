using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

public class ScheduleStage : Frame {

    private float headerHeight = 25;
    private Frame content;
    public int timePerBlock { get; set; }
    public int blocksPerScreen { get; set; }

    void Awake()
    {
        timePerBlock = 15;
        blocksPerScreen = 16;

        content = new GameObject("Content").AddComponent<ScheduleStageContent>();
        content.transform.parent = this.transform;
    }

    protected override void Update()
    {
        base.Update();
		headerHeight = rect.height / 12;
        float headerHeightPerc = headerHeight / rect.height;
        content.bounds.Set(0, headerHeightPerc, 1, 1 - headerHeightPerc);

        float screenDuration = timePerBlock * blocksPerScreen;
        DateTime now = DateTime.Now;
        for (int i = 0; i < content.transform.childCount; i++)
        {
            var child = content.transform.GetChild(i).GetComponent<ScheduleStageEvent>();
            float minuteOffset = (float)(child.when - now).TotalMinutes;
            child.bounds.y = minuteOffset / screenDuration;
        }
    }

    public override void UpdateWithJson(string json)
    {
        var events = JsonConvert.DeserializeObject<List<object>>(json);

        foreach (Transform child in transform.FindChild("Content"))
        {
            Destroy(child.gameObject);
        }

        float screenDuration = timePerBlock * blocksPerScreen;
        DateTime now = DateTime.Now;
        for (int i = 0; i < events.Count; i++)
        {
            var child = new GameObject().AddComponent<ScheduleStageEvent>();
            child.UpdateWithJson(events[i].ToString());
            float screenPerc = (float)(child.duration.TotalMinutes / screenDuration);
            float minuteOffset = (float)(child.when - now).TotalMinutes;
            child.bounds.Set(0, minuteOffset / screenDuration, 1, screenPerc);
            child.transform.parent = content.transform;
        }
    }

    protected override void OnGUI()
    {
        base.OnGUI();

		var style = new GUIStyle ();

		var r = new Rect (rect.x, rect.y, rect.width, headerHeight);

		style.alignment = TextAnchor.MiddleCenter;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;
		style.fontSize = (int)(r.height / 2f);

        GUI.depth = 5;
        GUI.Box(r, name, style);
    }
}
