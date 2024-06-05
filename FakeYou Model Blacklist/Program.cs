using System.Text.RegularExpressions;

string mpath = args[0];
string allmodels = Path.Combine(mpath, "all.txt");
string modeltoken = args[1];

try
{
    File.AppendText(Path.Combine(mpath, "blacklist.txt"))
        .WriteLine(modeltoken);
    
    var tempFile = Path.GetTempFileName();
    var linesToKeep = File.ReadLines(allmodels)
        .Where(l => !Regex.IsMatch(l,"^" + modeltoken));

    File.WriteAllLines(tempFile, linesToKeep);

    File.Delete(allmodels);
    File.Move(tempFile, allmodels);
}
catch (Exception e) { Console.WriteLine(e); }