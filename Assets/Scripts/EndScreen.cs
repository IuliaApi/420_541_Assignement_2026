using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    public TMP_Text finalScoreText;

    private void Start()
    {
        finalScoreText.text = "Final score: " + GameManager.Instance.Score;
    }
}
