using UnityEngine;
using System.Collections;
using System;

public class ScheduleStageContent : Frame
{
    protected override void OnGUI()
    {
        GUI.BeginGroup(rect);

        var parent = transform.parent.GetComponent<ScheduleStage>();
        int blocksPerScreen = parent.blocksPerScreen;
        int timePerBlock = parent.timePerBlock;
        int screenDuration = blocksPerScreen * timePerBlock;
        float blockHeight = rect.height / blocksPerScreen;

        DateTime now = DateTime.Now;
        DateTime blockFromNow = now.AddMinutes(timePerBlock);
        double excessMinutes = blockFromNow.Minute % timePerBlock;
        blockFromNow = blockFromNow.AddMinutes(-excessMinutes).AddSeconds(-blockFromNow.Second);
        double blockTime = (blockFromNow - now).TotalMinutes;
        double screenPerc = blockTime / screenDuration;
        float firstY = (float)(rect.height * screenPerc);
        Texture color = ((blockFromNow.Minute / timePerBlock) % 2 == 0) ? App.color1 : App.color2;

        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = new Color(1, 1, 1, 0.25f);
        style.fontStyle = FontStyle.Bold;
		style.fontSize = (int)(blockHeight / 2f);

        blockFromNow = blockFromNow.AddMinutes(-timePerBlock);
        for (int i = -1; i < blocksPerScreen; i++)
        {
            color = (color == App.color1) ? App.color2 : App.color1;
            float y = firstY + i * blockHeight;
            Rect r = new Rect(0, y, rect.width, blockHeight);
            GUI.DrawTexture(r, color);
            GUI.Label(r, blockFromNow.ToString("hh:mm tt"), style);
            blockFromNow = blockFromNow.AddMinutes(timePerBlock);
        }

        GUI.EndGroup();
    }
}