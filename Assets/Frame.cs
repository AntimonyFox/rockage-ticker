using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Frame : MonoBehaviour {

    public Rect bounds;
    [HideInInspector]
    public Rect rect;
    protected DateTime update_time;

    protected virtual void Awake()
    {

    }

    protected virtual void Start () {
        
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        var parent = gameObject.transform.parent;
        var parentRect = (parent) ? parent.GetComponent<Frame>().rect : new Rect(0, 0, Screen.width, Screen.height);
        rect.Set(
            parentRect.x + bounds.x * parentRect.width,
            parentRect.y + bounds.y * parentRect.height,
            bounds.width * parentRect.width,
            bounds.height * parentRect.height
        );
	}

    protected virtual void OnGUI()
    {
        //GUI.Box(rect, "");
    }

    public virtual void UpdateWithJson(string json) 
    {
        
    }

    protected Frame GetSubFrame(string name)
    {
        return transform.FindChild(name).GetComponent<Frame>();
    }

    public bool UpdateTime(DateTime newTime)
    {
        DateTime oldTime = update_time;
        update_time = newTime;
        return newTime > oldTime;
    }
}
