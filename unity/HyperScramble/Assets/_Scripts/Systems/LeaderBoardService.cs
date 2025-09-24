using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class ScoreEntry
{
    public string nick;
    public int score;
    public int hits;
    public ScoreEntry(string nick, int score, int hits)
    {
        this.nick = nick;
        this.score = score;
        this.hits = hits;
    }
}  

public class LeaderBoardService : PersistentSingleton<LeaderBoardService>
{
    [SerializeField] string databaseURL = "https://hyperscramble-leaderboard-default-rtdb.europe-west1.firebasedatabase.app/scores.json";

    [SerializeField] int maxEntries = 8;
    public int MaxEntries => maxEntries;

    List<ScoreEntry> _topScores = new List<ScoreEntry>();
    public IReadOnlyList<ScoreEntry> TopScores => _topScores;

    protected override void Awake()
    {
        base.Awake();

        // Cargar las mejores puntuaciones al iniciar
        RefreshTopScores();
    }

    public void RefreshTopScores()
    {
        StartCoroutine(GetTopScores(MaxEntries, (scores) =>
        {
            _topScores = scores;

            // Ordenamos la lista de puntuaciones de forma descendente
            _topScores.Sort((a, b) => b.score.CompareTo(a.score));
        }));
    }

    // Obtiene las N mejores puntuaciones y devuelve lista ordenada desc
    public IEnumerator GetTopScores(int limit, Action<List<ScoreEntry>> onResult)
    {
        // consulta con ordenación descendente y límite de resultados
        string url = $"{databaseURL}?orderBy=\"score\"&limitToLast={limit}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            // Enviamos la solicitud y esperamos la respuesta
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                // obtenemos el texto JSON de la respuesta
                string json = www.downloadHandler.text;

                // Deserializar con MiniJSON a un diccionario donde la clave es el ID generado por Firebase
                // y el valor es otro diccionario con los datos de la puntuación almacenada
                var dict = Json.Deserialize(json) as Dictionary<string, object>;

                // Creamos la lista con la tabla de puntuaciones 
                List<ScoreEntry> scores = new List<ScoreEntry>();

                if (dict != null)
                {
                    foreach (var entry in dict)
                    {
                        var data = entry.Value as Dictionary<string, object>;
                        if (data == null) continue;

                        ScoreEntry ps = new(
                            data.ContainsKey("nick") ? data["nick"].ToString() : "???",
                            data.ContainsKey("score") ? System.Convert.ToInt32(data["score"]) : 0,
                            data.ContainsKey("hits") ? System.Convert.ToInt32(data["hits"]) : 0
                        );

                        scores.Add(ps);
                    }
                }

                // Ordenamos la lista de puntuaciones de forma descendente
                scores.Sort((a, b) => b.score.CompareTo(a.score));

                onResult?.Invoke(scores);
            }
            else
            {
                Debug.LogError($"Error Firebase: {www.error}");

                // En caso de error devolvemos una lista vacía
                onResult?.Invoke(new List<ScoreEntry>());
            }
        }
    }

    // POST para enviar un nuevo score
    public IEnumerator PostScore(ScoreEntry newScore, Action<bool> onResult = null)
    {
        // Creamos un nuevo registro en la base de datos
        string json = Json.Serialize(new Dictionary<string, object>
        {
            { "nick", newScore.nick },
            { "score", newScore.score },
            { "hits", newScore.hits }
        });

        Debug.Log($"Enviando puntuación: {json}");

        // Convertimos el JSON a un array de bytes
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        // Enviamos la solicitud POST a Firebase
        using (UnityWebRequest www = new UnityWebRequest(databaseURL, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // Enviamos la solicitud y esperamos la respuesta
            Debug.Log("Enviando solicitud...");

            yield return www.SendWebRequest();

            Debug.Log("Solicitud enviada. Chequeando resultado...");

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("🎉 Puntuación subida correctamente.");
                onResult?.Invoke(true);
            }
            else
            {
                Debug.LogError($"Error al subir score: {www.error}");
                onResult?.Invoke(false);
            }
        }
    }
}
