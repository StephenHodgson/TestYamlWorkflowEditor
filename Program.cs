// Overview: This console app is designed to read a GitHub Actions Workflow YAML file
// and edit the file depending on the user's inputs.

Console.WriteLine("Enter the path to the YAML file:");
var inputPath = Console.ReadLine();

// check that path is valid and file exists
if (!File.Exists(inputPath))
{
    Console.WriteLine("File does not exist");
    return;
}
// open the file and read the contents as utf8 bytes
var yamlUtf8Bytes = await File.ReadAllBytesAsync(inputPath);
var yamlObject = YamlSerializer.Deserialize<dynamic>(yamlUtf8Bytes);
Console.WriteLine("YAML file loaded successfully");

var json = JsonSerializer.Serialize(yamlObject, new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
});
Console.WriteLine(json);
// modify the yamlObject workflow name property to TEST
yamlObject["name"] = "TEST";

// After modifying the yamlObject, serialize and write it back to the same input file
ReadOnlyMemory<byte> modifiedYamlUtf8Bytes = YamlSerializer.Serialize(yamlObject);
await File.WriteAllBytesAsync(inputPath, modifiedYamlUtf8Bytes.ToArray());
Console.WriteLine("YAML file modified successfully");
