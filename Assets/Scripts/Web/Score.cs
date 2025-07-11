using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Score : MonoBehaviour
{
    public IEnumerator SubmitScore(string name, float time)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("time", time.ToString("F2"));

        using (UnityWebRequest www = UnityWebRequest.Post("http://192.168.68.104/submit_score.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.Log("Error: " + www.error);
            else
                Debug.Log("Score submitted!");
        }
    }
}
