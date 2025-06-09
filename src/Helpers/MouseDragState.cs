using Microsoft.Xna.Framework;

public class MouseDragState
{
    public bool IsDragging { get; set; }
    public bool DragStarted { get; set; }
    public bool DragEnded { get; set; }
    public Point StartPosition { get; set; }
    public Point CurrentPosition { get; set; }
    public Point Delta => CurrentPosition - StartPosition;
}
