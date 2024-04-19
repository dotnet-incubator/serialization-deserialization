using System.Text;

namespace SerializationDeserialization;

class Program
{
    static void Main()
    {
        var personProperties = ReadDataFile("alex_great.txt");
        var person = new Person();
        SetClassProperties(person, personProperties);

        SerializeClassProperties(person, "alex_great_new.txt");
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

    static void SetClassProperties<T>(T objectToDeserialize, Dictionary<string, string> data) where T : class
    {
        var propertyInfos = objectToDeserialize.GetType().GetProperties();

        foreach (var propertyInfo in propertyInfos)
        {
            if (data.ContainsKey(propertyInfo.Name))
            {
                try
                {
                    var propertyType = propertyInfo.PropertyType;
                    var convertedValue = Convert.ChangeType(data[propertyInfo.Name], propertyType);
                    propertyInfo.SetValue(objectToDeserialize, convertedValue, null);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    static void SerializeClassProperties<T>(T objectToSerialize, string filePath) where T : class
    {
        var properties = objectToSerialize.GetType().GetProperties();
        var stringBuilder = new StringBuilder();

        foreach (var property in properties)
        {
            stringBuilder.AppendLine($"{property.Name}:{property.GetValue(objectToSerialize, null)}");
        }

        File.AppendAllText(filePath, stringBuilder.ToString());
    }
}
