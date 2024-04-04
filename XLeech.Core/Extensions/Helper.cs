using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XLeech.Core.Extensions
{
    public static class Helper
    {

        public static T GetRandomElement<T>(List<T> values)
        {
            if (values != null && values.Count > 0)
            {
                Random rand = new Random();
                int index = rand.Next(values.Count);
                return values[index];
            }

            return default(T);
        }

        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            string normalizedString = txt.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GetRandomProxyLive(List<string> proxys)
        {
            var result = string.Empty;
            while (true && proxys != null && proxys.Count > 0)
            {
                var proxy = GetRandomElement<string>(proxys);
                if (!string.IsNullOrEmpty(proxy) && CheckStatusProxyLive(proxy))
                {
                    result = proxy;
                    break;
                }
                else
                {
                    proxys.Remove(proxy);
                }
            }

            return result;
        }

        public static bool CheckStatusProxyLive(string proxyAddress)
        {
            try
            {
                WebRequest request = WebRequest.Create("http://www.google.com");
                request.Proxy = GetWebProxy(proxyAddress);
                WebResponse response = request.GetResponse();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static WebProxy GetRandomProxyLiveOrDefault(List<string> proxys)
        {
            var proxyAddress = GetRandomProxyLive(proxys);
            if (string.IsNullOrEmpty(proxyAddress))
                return new WebProxy();
            else
            {
                return GetWebProxy(proxyAddress);
            }
        }

        public static WebProxy GetWebProxy(string proxyAddress)
        {
            WebProxy proxyString;
            if (proxyAddress.Split(":").Length == 4)
            {
                var proxyIpPort = proxyAddress.Split(":");
                proxyString = new WebProxy(String.Format("{0}:{1}", proxyIpPort[0], proxyIpPort[1]), true);
                proxyString.Credentials = new NetworkCredential(proxyIpPort[2], proxyIpPort[3]);
            }
            else
            {
                proxyString = new WebProxy(proxyAddress, true);
            }

            return proxyString;
        }

        public static string GetAbsolutePath(this string url)
        {
            Uri uri = new Uri(url);
            return uri.AbsolutePath;
        }

        public static bool IsValidURL(string URL)
        {
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(URL);
        }

        public static void DownloadImage(string imageUrl, string savePath, string fileName)
        {
            // Kiểm tra xem thư mục đã tồn tại chưa
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            using (WebClient client = new WebClient())
            {
                client.DownloadFile(imageUrl, Path.Combine(savePath, fileName));
            }
        }

        public static int GetPageNumberFromRegex(string url, string regexPattern)
        {
            var result = 1;
            try
            {
                if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(regexPattern))
                {
                    MatchCollection matches = Regex.Matches(url, regexPattern);
                    if (matches.Count > 0)
                    {
                        foreach (Match match in matches)
                        {
                            var patternNumber = @"\d+";
                            MatchCollection matchesNumber = Regex.Matches(match.Value, patternNumber);
                            if (matchesNumber.Count > 0)
                            {
                                result = Int32.Parse(matchesNumber[0].Value);
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        public static string ObjectToBase64(object obj)
        {
            byte[] objBytes;
            // Serialize đối tượng thành mảng byte
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, obj);
                objBytes = memoryStream.ToArray();
            }

            // Chuyển đổi mảng byte thành chuỗi Base64 và trả về
            return Convert.ToBase64String(objBytes);
        }

        public static string ObjectToString(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static object StringToObject(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
        }

        public static string ConvertStrToCapitalize(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;

            if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            else
                return char.ToUpper(str[0]) + str.Substring(1);
        }

        public static string[] ToListString(this string str)
        {
            if (string.IsNullOrEmpty(str)) return new string[0];
            return str.Split(new string[] { "\n" }, StringSplitOptions.None);
        }
    }
}
