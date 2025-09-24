using TMPro;
using UnityEngine;

public class LoadTopScores : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nicksText;
    [SerializeField] TextMeshProUGUI scoresText;

    void Start()
    {
        LoadScores();
    }

    void LoadScores()
    {
        var topScores = LeaderBoardService.Instance.TopScores;

        nicksText.text = "";
        scoresText.text = "";

        foreach (var scoreEntry in topScores)
        {
            nicksText.text += $"{scoreEntry.nick}\n";
            scoresText.text += $"{scoreEntry.score}\n";
        }
    }
}
