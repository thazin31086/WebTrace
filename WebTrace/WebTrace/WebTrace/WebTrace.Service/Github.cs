
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebTrace.Model;

namespace WebTrace.Services
{
    //Github classes
    public class Github
    {
        private const string _accesstoken = "88e04e9f8c53344e05c400db1634187641a9a669";
        private const string _owner = "thazin31086";
        private const string _name = "webtrace";
        //Get all files from a repo
        public static async Task<Directory> getRepo()
        {
            HttpClient client = new HttpClient();
            Directory root = await readDirectory("root", client, String.Format("https://api.github.com/repos/{0}/{1}/contents/", _owner, _name), _accesstoken);
            client.Dispose();
            return root;
        }

        //recursively get the contents of all files and subdirectories within a directory 
        private static async Task<Directory> readDirectory(String name, HttpClient client, string uri, string access_token)
        {
            //get the directory contents
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
            request.Headers.Add("Authorization",
                "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", access_token, "x-oauth-basic"))));
            request.Headers.Add("User-Agent", "lk-github-client");

            //parse result
            HttpResponseMessage response = await client.SendAsync(request);
            String jsonStr = await response.Content.ReadAsStringAsync(); ;
            response.Dispose();
            FileInfo[] dirContents = JsonConvert.DeserializeObject<FileInfo[]>(jsonStr);

            //read in data
            Directory result;
            result.name = name;
            result.subDirs = new List<Directory>();
            result.files = new List<FileData>();

            foreach (FileInfo file in dirContents)
            {
                if (file._links.ToString().Contains("contents/Datasets") || file.name.ToUpper() == "DATASETS")
                {
                    if (file.type == "dir")
                    { //read in the subdirectory
                        Directory sub = await readDirectory(file.name, client, file._links.self, access_token);
                        result.subDirs.Add(sub);
                    }
                    else
                    { //get the file contents;
                        HttpRequestMessage downLoadUrl = new HttpRequestMessage(HttpMethod.Get, file.download_url);
                        downLoadUrl.Headers.Add("Authorization",
                            "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", access_token, "x-oauth-basic"))));
                        request.Headers.Add("User-Agent", "lk-github-client");

                        HttpResponseMessage contentResponse = await client.SendAsync(downLoadUrl);
                        String content = await contentResponse.Content.ReadAsStringAsync();
                        contentResponse.Dispose();

                        FileData data;
                        data.name = file.name;
                        data.contents = content;

                        result.files.Add(data);
                    }
                }
            }
            return result;
        }
    }
}
