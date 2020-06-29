
public class GameEventController : Singleton<GameEventController> {
  public GameEventHandler GameEvent;

  private void Awake() {
    GameEvent = new GameEventHandler();
  }

  public void NotifyEvent(GameEventArgs event_args) {
    GameEvent.Invoke(event_args);
  }
}
