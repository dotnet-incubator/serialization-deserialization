using System.Text;

namespace SerializationDeserialization;

internal class Program
{
    static void Main()
    {
        Person person = new Person() { Name = "Alex", Surname = "Great", Born = 356 };

        SerializePersonProperties(person, "alex_great2.txt");
    }

    static Dictionary<string, string> ReadDataFile(string path)
    {
        string[] data = File.ReadAllLines(path);
        Dictionary<string, string> properties = new Dictionary<string, string>();

        for (int i = 0; i < data.Length; i++)
        {
            string[] splittedData = data[i].Split(':');
            properties.Add(splittedData[0], splittedData[1]);
        }

        return properties;
    }

    static void SetPersonProperties(Person person, Dictionary<string, string> data)
    {
        var propertyInfos = person.GetType().GetProperties();

        for (int i = 0; i < propertyInfos.Length; i++)
        {
            if (data.ContainsKey(propertyInfos[i].Name))
            {
                try
                {
                    Type propertyType = propertyInfos[i].PropertyType;
                    var convertedValue = Convert.ChangeType(data[propertyInfos[i].Name], propertyType);
                    propertyInfos[i].SetValue(person, convertedValue, null);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    static void SerializePersonProperties(Person person, string filePath)
    {
        var properties = person.GetType().GetProperties();
        var stringBuilder = new StringBuilder();

        foreach (var property in properties)
        {
            stringBuilder.AppendLine($"{property.Name}:{property.GetValue(person, null)}");
        }

        File.AppendAllText(filePath, stringBuilder.ToString());
    }
}
