Console.WriteLine("Enter the path to the YAML file:");
var inputPath = Console.ReadLine();

// check that path is valid and file exists
if (!File.Exists(inputPath))
{
    Console.WriteLine("File does not exist");
    return;
}

// open the file and read the contents as a string
var yamlContent = await File.ReadAllTextAsync(inputPath);
using var stringReader = new StringReader(yamlContent);
var yamlStream = new YamlStream();

try
{
    yamlStream.Load(stringReader);
}
finally
{
    stringReader.Close();
}

var yamlDocument = yamlStream.Documents[0];
var rootNode = (YamlMappingNode)yamlDocument.RootNode;
var yamlObject = rootNode.Children;
Console.WriteLine("YAML file loaded successfully");

// modify the yamlDocument to set the workflow name property to TEST
// check if "name" exists then change the value to TEST
if (yamlObject.ContainsKey("name"))
{
    yamlObject["name"] = new YamlScalarNode("TEST");
}
else
{
    var node = new YamlScalarNode("TEST")
    {
        Style = ScalarStyle.SingleQuoted
    };
    yamlObject.Add("name", node);
}

// insert a new Scalar Node named "new" and set its value to "Test Value 2"
var newNode = new YamlScalarNode("Test Value 2")
{
    Style = ScalarStyle.SingleQuoted
};
yamlObject.Add("test-node", newNode);

// After modifying the yamlObject, serialize and write it back to the same input file
var serializer = new SerializerBuilder()
    .WithIndentedSequences()
    .Build();
await using var output = new StringWriter();

try
{
    serializer.Serialize(output, rootNode);
    var modifiedYamlContent = output.ToString();
    await File.WriteAllTextAsync(inputPath, modifiedYamlContent);
    Console.WriteLine("YAML file modified successfully");
}
finally
{
    output.Close();
}
