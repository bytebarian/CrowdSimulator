public class PedestrianPosition
{
    private int id;
    private decimal time;
    private float x;
    private float y;
    public PedestrianPosition(int id, decimal time, float x, float y)
    {
        this.id = id;
        this.time = time;
        this.x = x;
        this.y = y;
    }

    public int getID() { return this.id; }
    public decimal getTime() { return this.time; }
    public float getX() { return this.x; }
    public float getY() { return this.y; }
}
