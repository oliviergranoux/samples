namespace Worker.Settings
{
  public abstract class BaseBackgroundService
  {
    public string Name;
    public bool IsEnable { get; set; }
    public int DelayInSeconds { get; set; }
  }
}