using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

public class Viewport : Frame {

    protected override void Start()
    {
        base.Start();
        //var json = "{'update_time':'12/27/2014 10:21:00 PM','schedule':{'update_time':'12/27/2014 10:21:00 PM','stage_a':{'update_time':'12/27/2014 10:21:00 PM','events':[{'update_time':'12/27/2014 10:21:00 PM','name':'Petriform','status':'active','when':'12/28/2014 9:00:00 PM','duration':'0:45:00'},{'update_time':'12/27/2014 10:21:00 PM','name':'crashfaster','status':'upcoming','when':'12/28/2014 10:00:00 PM','duration':'0:45:00'},{'update_time':'12/27/2014 10:21:00 PM','name':'Super Soul Brothers','status':'upcoming','when':'12/29/2014 12:00:00 AM','duration':'0:45:00'}]},'stage_b':{'update_time':'12/27/2014 10:21:00 PM','events':[{'update_time':'12/27/2014 10:21:00 PM','name':'Curious Quail','status':'upcoming','when':'12/28/2014 11:00:00 PM','duration':'0:45:00'}]},'stage_c':{'update_time':'12/27/2014 10:21:00 PM','events':[{'update_time':'12/27/2014 10:21:00 PM','name':'Mega Ran','status':'upcoming','when':'12/29/2014 8:30:00 AM','duration':'0:45:00'}]}}}";
        //var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        //GetSubFrame("Schedule").UpdateWithJson(dict["schedule"].ToString());

        StartCoroutine("CheckForUpdates");
    }

    private IEnumerator CheckForUpdates()
    {
        while (true)
        {
            WWW www = new WWW(App.checkUrl);
            yield return www;
            print(www.text);
            var list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(www.text);
            foreach (var update in list)
            {
                String module = update["name"];
                DateTime dt = DateTime.Parse(update["updated_at"]);

                module = module.Substring(0, 1).ToUpper() + module.Substring(1, module.Length - 1);
                var m = transform.FindChild(module).GetComponent<Frame>();
                if (m != null)
                {
                    if (m.UpdateTime(dt))
                    {
                        StartCoroutine("UpdateModule", m);
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator UpdateModule(Frame module)
    {
        WWW www = new WWW(App.baseUrl + module.name.ToLower());
        yield return www;
        print(www.text);
        module.UpdateWithJson(www.text);
    }
}
