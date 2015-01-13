using UnityEngine;
using System.Collections;

public class App {

	public static readonly Texture color1 = Resources.Load<Texture>("solid_blue");
    public static readonly Texture color2 = Resources.Load<Texture>("solid_black");
    public static readonly Texture colorTournOn = Resources.Load<Texture>("solid_tournon");
    public static readonly Texture colorTournOff = Resources.Load<Texture>("solid_tournoff");
    public static readonly string noName = "";

    public static readonly string baseUrl = "http://192.168.0.14:3000/api/";
    //public static readonly string baseUrl = "rockage.herokuapp.com/api/";
    public static readonly string checkUrl = baseUrl + "check/";
}
