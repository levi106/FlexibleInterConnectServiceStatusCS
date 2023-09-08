using System.Xml.Linq;
using System.Text;

string ItemToString(XElement item)
{
    var title = item?.Element("title")?.Value;
    var link = item?.Element("link")?.Value;
    var description = item?.Element("description")?.Value;
    var pubDate = item?.Element("pubDate")?.Value;
    return $"| [{title}]({link}) | {description} | {pubDate} |";
}

HttpClient client = new()
{
    BaseAddress = new Uri("https://status.sdpf.ntt.com")
};
using HttpResponseMessage response = await client.GetAsync("/rss/ja/fic/japan-east/");
response.EnsureSuccessStatusCode();
var data = await response.Content.ReadAsStringAsync();
var doc = XDocument.Parse(data);
StringBuilder sb = new();
sb.AppendLine("| Title | Description | PubDate |");
sb.AppendLine("|:-----|:-----------|:-------|");
var cs = (from e in doc.Descendants("item") select ItemToString(e));
foreach (var c in cs) {
    sb.AppendLine(c);
}
Console.WriteLine(sb.ToString());
