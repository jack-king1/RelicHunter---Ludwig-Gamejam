using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GameEnd"))
        {
            Debug.Log("Game End Collided");
            //Disable player move/ability controls
            ServiceLocator.Instance.gameManager.SetGameEnd(true);
            ServiceLocator.Instance.gameManager.SetGameEndHighScore();
            ServiceLocator.Instance.playerManager.GetPlayerAbilityController().abilityIndicator.SetActive(false);
            ServiceLocator.Instance.playerManager.GetPlayer().GetComponent<Animator>().SetBool("Walking", false);
            ServiceLocator.Instance.playerManager.GetPlayer().GetComponent<CandleController>().candleReduceSpeed = 4f;
            ServiceLocator.Instance.playerManager.GetPlayer().GetComponent<CandleController>().minCandleRadius = 0f;
            ServiceLocator.Instance.playerManager.GetPlayer().GetComponentInChildren<ParticleSystem>().Stop();
            ServiceLocator.Instance.uiManager.DisableAllGameUI();
            ServiceLocator.Instance.uiManager.FadeInEndScreen();


            //Display win screen
            //Display highscore
            //start credits
            //Reset player current pos to start
            //save data
            ServiceLocator.Instance.gameManager.ApplyGameData();
        }
    }
}
