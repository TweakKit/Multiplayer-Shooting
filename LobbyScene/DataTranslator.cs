using UnityEngine;

public class DataTranslator : MonoBehaviour {

    private const string KILLS_SYMBOL = "[KILLS]";
    private const string DEATHS_SYMBOL = "[DEATHS]";

    // convert from values to data to save in database
    public static string ValuesToData(int kills,int deaths)
    {
        Debug.Log("saved Again: " + KILLS_SYMBOL + kills + "/" + DEATHS_SYMBOL + deaths);
        return KILLS_SYMBOL + kills + "/" + DEATHS_SYMBOL + deaths;
    }

    // get the value of kills from database
    public static int DataToKills(string data)
    {
        return int.Parse(DataToValues(data, KILLS_SYMBOL));
    }

    // get the value of deaths from database
    public static int DataToDeath(string data)
    {
        return int.Parse(DataToValues(data, DEATHS_SYMBOL));
    }

    // convert method
    private static string DataToValues(string data,string symbol)
    {
        string[] pieces = data.Split('/'); // i.e : KILLS10/DEATHS5
        foreach (string piece in pieces)
        {
            if(piece.StartsWith(symbol))
            {
                return piece.Substring(symbol.Length);
            }
        }

        return "";
    }
	 
}
