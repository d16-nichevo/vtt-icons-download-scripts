<Query Kind="Statements" />

using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Concurrent;

/*
This is a LINQPad script for downloading skill icons from the Pathfinder: Kingmaker Wiki.

Expect about 1,544 icons.

Be sure to set the "saveFolder" below!

Please see the readme.md here for more information:
https://github.com/d16-nichevo/vtt-icons-download-scripts/blob/main/README.md

This script downloads images from the Pathfinder: Kingmaker Wiki:
https://pathfinderkingmaker.fandom.com/wiki/Category:Spells_and_abilities_images
*/

// Where to save the files.
// Change this variables if you need to.
// You shouldn't need to change anything below.
var saveFolder = @"C:\temp\pfkm_icons";

// Create a new ConcurrentDictionary. 2000 icons should suffice (we expect ~1544).
var iconUrlsDict = new ConcurrentDictionary<string, string>(8, 2000);
var httpClient = new HttpClient();

//////////////////////////////////////////
//	PART ONE
//	Parse the category pages, moving page-by-page
//  and collecting URLs.
/////////////////////////////////////////
var oldIconUrlCount = -1;
var nextUrl = "https://pathfinderkingmaker.fandom.com/wiki/Category:Spells_and_abilities_images";
// Keep looping until we find no more icons. Then we know to stop.
while(iconUrlsDict.Count() > oldIconUrlCount)
{
	oldIconUrlCount = iconUrlsDict.Count();
	
	// Download a single page from the category.
	// This should contain ~200 icons.
	// Retry a few times in case of error:
	Console.WriteLine($"Downloading page: {nextUrl}");
	string html = await httpClient.GetStringAsync(nextUrl);
	
	// Pull out image matches from that page:
	var matches = Regex.Matches(html, @"src=""https://static\.wikia\.nocookie\.net/pathfinderkingmaker_gamepedia_en/images/[^\s""]+\?cb=\d+", RegexOptions.IgnoreCase);

	// Go through each match:
	foreach (Match match in matches)
	{
		var fileName = Regex.Match(match.Value, @"[^/]+\.png").Value;
		var url = match.Value.Remove(0,5);
		
		// Ignore empty strings and ignore "site-logo" images:
		if(!String.IsNullOrWhiteSpace(fileName) && !fileName.Contains("Site-logo"))
		{
			// Add to dictionary, if it's not there already:
			if(iconUrlsDict.TryAdd(fileName, url))
			{
				Console.WriteLine($"Found \"{fileName}\".");
			}
			else
			{
				Console.WriteLine($"Dropped already-existing \"{fileName}\".");
			}
		}
	}
	
	// Move to next page:
	Console.WriteLine($"Found {iconUrlsDict.Count() - oldIconUrlCount} icons.");
	nextUrl = "https://pathfinderkingmaker.fandom.com/wiki/Category:Spells_and_abilities_images?filefrom=" + iconUrlsDict.OrderBy(x => x.Key).Last().Key;
}

//////////////////////////////////////////
//	PART TWO
//	Save the icons we found to the disk.
/////////////////////////////////////////
Directory.CreateDirectory(saveFolder);
var options = new ParallelOptions { MaxDegreeOfParallelism = 20 };
iconUrlsDict.Dump();
await Parallel.ForEachAsync(iconUrlsDict, options, async (iconUrl, token) =>
{
	// Wikia pages like to convert everything to WEBP
	// even though the filename may be PNG. So to we accurate
	// we save with WEBP extension:
	var fileName = iconUrl.Key.Replace(".png", ".webp");
	var filePath = Path.Combine(saveFolder, fileName);

	// Download the icon:
	byte[] fileBytes = await httpClient.GetByteArrayAsync(iconUrl.Value);

	// Save to disk:
	File.WriteAllBytes(filePath, fileBytes);
	Console.WriteLine($"Fetching \"{iconUrl.Value}\"... Downloaded \"{iconUrl.Key}\".");
});
