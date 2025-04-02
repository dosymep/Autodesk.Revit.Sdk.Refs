using System.IO;

string version = "2026";
string oldVersion = "2025";

string target = @"lib";
string source = @"lib\" + oldVersion;
string originals = @"C:\Program Files\Autodesk\Revit " + version;

foreach(string enumerateFile in Directory.EnumerateFiles(source)) {
    string fileName = Path.GetFileName(enumerateFile);
    string targetFile = Path.Combine(target, version, fileName);
    string originalFile = Path.Combine(originals, version, fileName);

    Directory.CreateDirectory(Path.Combine(target, version));
    File.Copy(originalFile, targetFile);
}
