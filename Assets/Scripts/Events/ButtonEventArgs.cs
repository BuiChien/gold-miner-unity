public class ButtonEventArgs : GameEventArgs {
  public string ButtonName { get; private set; }
  public ButtonEventArgs(string btnName)
    : base(EventNames.BUTTON_CLICK) {
    ButtonName = btnName;
  }
}