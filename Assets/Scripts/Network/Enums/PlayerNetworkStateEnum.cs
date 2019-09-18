/// <summary>
/// Hold the player network states, that can be requested throught a RequestPlayerState type message.
/// 
/// Logged = Player is in lobby
/// WaitingForPlayRequest = Player is waiting for requests
/// Playing = Player is playing and cant be asked for play
/// EndPlay = Playr is ending the game that he was.
/// 
/// </summary>

public enum PlayerNetworkStateEnum
{
    Logged = 0,
    WaitingForPlayRequest = 1,
    Playing = 2,
    EndingPlay = 3,
}