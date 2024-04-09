namespace SerializationDeserialization;

[Serializable]
public class Person
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public int Born { get; set; }
}
