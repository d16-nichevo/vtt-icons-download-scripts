<Query Kind="Statements">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Json</Namespace>
  <Namespace>System.Text.Json</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

/*
This is a LINQPad script for downloading skill icons from Guild Wars 2.

Be sure to set the "saveFolder" below!

Please see the readme.md here for more information:
https://github.com/d16-nichevo/vtt-icons-download-scripts/blob/main/README.md

This script uses the Guild Wars 2 API:
https://wiki.guildwars2.com/wiki/API:2
*/

// Change these variables if you need to.
// Where to save the files:
var saveFolder = @"C:\temp\gw2_icons";

// The Guild Wars 2 API is rate limited.
// https://wiki.guildwars2.com/wiki/API:Best_practices#Rate_Limit
// These values below are configured to stay under that limit.
var waitTimeSeconds = 1;
var options = new ParallelOptions { MaxDegreeOfParallelism = 5 };

var httpClient = new HttpClient();

// Let's download the full list of skill IDs from the GW2 API:
List<int> skillIds = await httpClient.GetFromJsonAsync<List<int>>("https://api.guildwars2.com/v2/skills/");
// The below line should be commented out when used for real.
// When debugging, this can be useful to cut down on the number
// of skills to parse.
// skillIds = skillIds.Take(25).ToList();

// We have a full list of skills. Let's get the icon URL and name from each.
// This dictionary <key, value> is <url, name>:
var iconUrlsDict = new ConcurrentDictionary<string, string>(8, skillIds.Count());
await Parallel.ForEachAsync(skillIds, options, async (skillId, token) =>
{
	// Download the API info on this particular skill:
	var json = await httpClient.GetStringAsync($"https://api.guildwars2.com/v2/skills/{skillId}");
	using var doc = JsonDocument.Parse(json);	
	// Get the skill's name and its icon:
	var iconUrl = doc.RootElement.GetProperty("icon").GetString();
	var name = doc.RootElement.GetProperty("name").GetString();
	// Add this to our dictionary. The dictionary does not accept duplicates
	// for its key, which is good, because we don't want to download the same
	// URL many times:
	if(iconUrlsDict.TryAdd(iconUrl, name))
	{
		Console.WriteLine($"Parsed skill ID {skillId}. Found \"{name}\". Added new icon \"{iconUrl}\"");
	}
	else
	{
		Console.WriteLine($"Parsed skill ID {skillId}. Found \"{name}\". Dropped already-existing icon \"{iconUrl}\"");
	}
	await Task.Delay(TimeSpan.FromSeconds(waitTimeSeconds));
});

// All URL are fetched:
Console.WriteLine();
Console.WriteLine($"Found {iconUrlsDict.Count()} unique icons across {skillIds.Count()} skills.");
Console.WriteLine();

// Now we have a list of URLs. Let's save them to disk:
// Create the place where the files are to be saved:
Directory.CreateDirectory(saveFolder);
await Parallel.ForEachAsync(iconUrlsDict, options, async (kvp, ct) =>
{
	// The file name is based on the associated skill name:
	var fileName = kvp.Value;
	// But we strip out certain characters:
	fileName = fileName.Replace(" ", "-");
	fileName = Regex.Replace(fileName, "[^a-zA-Z0-9-]", string.Empty);
	// These are all pngs, so we manually add the PNG extension:
	fileName += ".png";
	var filePath = Path.Combine(saveFolder, fileName);
	
	// Download the icon:
	byte[] fileBytes = await httpClient.GetByteArrayAsync(kvp.Key);

	// Save to disk:
	File.WriteAllBytes(filePath, fileBytes);
	Console.WriteLine($"Fetching icon for \"{kvp.Value}\"... Downloaded \"{fileName}\".");
	
	await Task.Delay(TimeSpan.FromSeconds(waitTimeSeconds));
});
