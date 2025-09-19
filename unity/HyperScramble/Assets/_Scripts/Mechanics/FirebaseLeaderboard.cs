using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MiniJSON;
using System;

public class FirebaseLeaderboard : MonoBehaviour
{
    string databaseURL = "https://hyperscramble-leaderboard-default-rtdb.europe-west1.firebasedatabase.app/scores.json";

    [Serializable]
    public class PlayerScore
    {
        public string nick;
        public int score;
        public int hits;
        public PlayerScore(string nick, int score, int hits)
        {
            this.nick = nick;
            this.score = score;
            this.hits = hits;
        }
    }

    [Serializable]
    public class ScoreData
    {
        public Dictionary<string, PlayerScore> scores;
    }

    public IEnumerator GetTopScores(int limit, System.Action<List<PlayerScore>> onResult)
    {
        string url = $"{databaseURL}?orderBy=\"score\"&limitToLast={limit}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;

                // Deserializar con MiniJSON
                var dict = Json.Deserialize(json) as Dictionary<string, object>;

                List<PlayerScore> scores = new List<PlayerScore>();

                foreach (var entry in dict)
                {
                    var data = entry.Value as Dictionary<string, object>;

                    PlayerScore ps = new PlayerScore(
                        data.ContainsKey("nick") ? data["nick"].ToString() : "???",
                        data.ContainsKey("score") ? System.Convert.ToInt32(data["score"]) : 0,
                        data.ContainsKey("hits") ? System.Convert.ToInt32(data["hits"]) : 0
                    );

                    scores.Add(ps);
                }

                // Ordenar descendente por score
                scores.Sort((a, b) => b.score.CompareTo(a.score));

                // Devolver al callback
                onResult?.Invoke(scores);
            }
            else
            {
                Debug.LogError($"Error Firebase: {www.error}");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //StartCoroutine(GetTopScores(10, ShowScores));
            //StartCoroutine(GetTopScores(10, CheckScores));
            StartCoroutine(LeaderBoardService.Instance.GetTopScores(10, ShowScores));
        }
    }

    void ShowScores(List<ScoreEntry> scores)
    {
        Debug.Log("=== Leaderboard ===");
        for (int i = 0; i < scores.Count; i++)
        {
            Debug.Log($"{i + 1}. {scores[i].nick} - {scores[i].score}");
        }
    }
}
