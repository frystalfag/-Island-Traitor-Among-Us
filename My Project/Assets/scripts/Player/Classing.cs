using UnityEngine;

public enum PlayerType
{
    Survivor,
    Saboteur
}

public class PlayerRole : MonoBehaviour
{
    public PlayerType playerType = PlayerType.Survivor;
}
