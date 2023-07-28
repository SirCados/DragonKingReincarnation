using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerCharacterController Player;
    public Canvas HUD;
    public Canvas LevelMenu;
    public Grid Map;

    void CheckIfGameOver()
    {
        if (Player.IsDead)
        {
            //ded x_x
        }
    }
}
