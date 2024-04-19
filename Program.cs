using System.Text;

namespace SerializationDeserialization;

class Program
{
    static void Main()
    {
        var person = new Person { Name = "Alex", Surname = "Great", Born = 356 };

        SerializePersonProperties(person, "alex_great2.txt");
    }

    static Dictionary<string, string> ReadDataFile(string path)
    {
        var data = File.ReadAllLines(path);
        var properties = new Dictionary<string, string>();

        foreach (var line in data)
        {
            var splittedData = line.Split(':');
            properties.Add(splittedData[0], splittedData[1]);
        }

        return properties;
    }

    static void SetPersonProperties(Person person, Dictionary<string, string> data)
    {
        var propertyInfos = person.GetType().GetProperties();

        foreach (var propertyInfo in propertyInfos)
        {
            if (data.ContainsKey(propertyInfo.Name))
            {
                try
                {
                    var propertyType = propertyInfo.PropertyType;
                    var convertedValue = Convert.ChangeType(data[propertyInfo.Name], propertyType);
                    propertyInfo.SetValue(person, convertedValue, null);
                }
                catch (Exception ex)
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
