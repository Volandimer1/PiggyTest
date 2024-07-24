public struct ArrowData
{
    public ArrowType ArrowType { get; private set; }
    public int Damage {  get; private set; }


    public ArrowData(ArrowType arrowType, int gamage)
    {
        ArrowType = arrowType;
        Damage = gamage;
    }
}