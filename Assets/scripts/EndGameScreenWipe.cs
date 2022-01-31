using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreenWipe : MonoBehaviour {
    
    public Image WhiteCover;
    public Image Logo;
    
    [Header("Game Loss")]
    public float LossTime;
    public Gradient LossGradient;
    public Gradient LossLogoGradient;

    [Header("GameWin")]
    public float WinTime;
    public Gradient WinGradient;

    private void Update() {

        if (!PlayerStatus.PlayerHasLost && !PlayerStatus.PlayerHasWon) {
            if (WorldStatus.GameTimeLeft <= 0f) {
                PlayerStatus.PlayerHasLost = true;
            }
        }

        if (PlayerStatus.PlayerHasLost) {
            WhiteCover.color = LossGradient.Evaluate(WorldStatus.TimeSinceGameEnd / LossTime);
            Logo.color = LossLogoGradient.Evaluate(WorldStatus.TimeSinceGameEnd / LossTime);
            if (WorldStatus.TimeSinceGameEnd > LossTime) {
                Application.Quit();
            }
        }

        if (PlayerStatus.PlayerHasWon) {
            WhiteCover.color = WinGradient.Evaluate(WorldStatus.TimeSinceGameEnd / WinTime);
            if (WorldStatus.TimeSinceGameEnd > 1f) {
                // TODO: Scene Load.
            }
        }
        
    }
    
}
