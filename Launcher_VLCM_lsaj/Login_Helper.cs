using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace Launcher_VLCM_niua_lsaj
{
    public class Login_Helper
    {
        /**
         * Helper method:
         * Retrieve the captcha image from the server and display it in the form.
         */
        public static Image get_random_captcha(CookieContainer cookieContainer)
        {
            // get the captcha based on the cookies
            byte[] captcha_data =
                Web_Request.Web_Request.send_request("http://www.niua.com/seccode.php",
                                                    "GET",
                                                    null,
                                                    cookieContainer);
            if (captcha_data == null)
                return null;

            // put the captcha in picture box control in the login form
            using (MemoryStream memory_stream = new MemoryStream(captcha_data))
            {
                return Image.FromStream(memory_stream);
            }
        }
        
        /**
         * Helper method:
         * Encrypt the password using MD5 since the server does not accept plain text password.
         */
        public static string md5_encrypt(string text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(text);
                byte[] result = md5.ComputeHash(data);
                StringBuilder string_builder = new StringBuilder();
                foreach (byte b in result)
                {
                    string_builder.Append(b.ToString("X2"));
                }
                return string_builder.ToString().ToLower();
            }
        }

        /**
         * Helper method:
         * Find the string given a pattern and a string.
         */
        public static string find_string(string input, string pattern) // hàm tìm kiếm chuỗi và trả về kết quả
        {
            Match match = Regex.Match(input, pattern);

            return match.Success ? match.Value : "";
        }

        /**
         * Helper method:
         * Replace all of the white spaces in a string.
         */
        public static string remove_white_space(string text)
        {
            return Regex.Replace(text, "\\s", "");
        }

        /**
         * Helper method:
         * Clean the input string to make it ready to be used in the query string.
         */
        public static string parse_to_query_string(string text)
        {
            string result = "";
            string[] text_split = Regex.Split(text, "\\n");
            for (int i = 0; i < text_split.Length; i++)
            {
                text_split[i] = remove_white_space(text_split[i]);
                if (text_split[i] == "")
                    continue;

                text_split[i] = Regex.Replace(text_split[i], ",$", "");
                string[] text_split_split = Regex.Split(text_split[i], "(?<=^[^:]*?):");
                text_split_split[1] = Regex.Replace(text_split_split[1], "^\"", "");
                text_split_split[1] = Regex.Replace(text_split_split[1], "\"$", "");
                result += string.Format("{0}={1}&", text_split_split[0], text_split_split[1]);
            }
            result = Regex.Replace(result, "&$", "");
            return result;
        }

        /**
         * Helper method:
         * Get the maximum server number
         */
        public static int get_max_server()
        {
            // send a get request to the link to get the HTML page
            byte[] html = Web_Request.Web_Request.send_request("http://www.niua.com/server/code/lsaj/",
                                                                "GET",
                                                                null,
                                                                null);
            if (html == null)
                return 0;
            // convert the html to string
            string html_string = Encoding.UTF8.GetString(html);
            // a regex to find the maximum server number
            Regex regex = new Regex("var qServer = {\"([0-9]{0,9})\":\\[");
            // temp value to hold the extracted values (3 parts)
            var temp = regex.Match(html_string);
            // retrieve the maximum server number from the second part of the temp value
            string server = temp.Groups[1].ToString();
            return int.Parse(server);
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }
    }
}
