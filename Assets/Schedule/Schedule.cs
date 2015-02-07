using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Schedule : Frame {

	public override void UpdateWithJson(string json) 
    {
        var dict = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
        foreach (var venue in dict)
        {
            print(venue["name"]);
			var s = GetSubFrame(venue["name"].ToString());
			if (s.gameObject.activeSelf)
            	s.UpdateWithJson(venue["events"].ToString());
        }
        //GetSubFrame("Stage A").UpdateWithJson(dict["stage_a"].ToString());
        //GetSubFrame("Stage B").UpdateWithJson(dict["stage_b"].ToString());
        //GetSubFrame("Stage C").UpdateWithJson(dict["stage_c"].ToString());
    }
}
