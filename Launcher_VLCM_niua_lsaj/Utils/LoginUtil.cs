using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Launcher_VLCM_niua_lsaj.Utils
{
    public class LoginUtil
    {
        /// <summary>
        /// Encrypts the input string using the MD5 hash algorithm.
        /// </summary>
        /// <param name="text">The string to be encrypted.</param>
        /// <returns>The MD5 hash of the input string in hexadecimal form, converted to lowercase.</returns>
        public static string md5_encrypt(string text)
        {
            // Instantiate the MD5 service provider
            using (MD5 md5 = MD5.Create())
            {
                // Convert the input string to a byte array
                byte[] data = Encoding.UTF8.GetBytes(text);

                // Compute the hash of the data
                byte[] result = md5.ComputeHash(data);

                // Use a StringBuilder to collect the bytes and create a string.
                StringBuilder string_builder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                foreach (byte b in result)
                {
                    string_builder.Append(b.ToString("X2"));
                }

                // Return the hexadecimal string in lowercase.
                return string_builder.ToString().ToLower();
            }
        }

        /// <summary>
        /// Searches for a specified pattern in a given string and returns the matched substring.
        /// </summary>
        /// <param name="input">The string to search within.</param>
        /// <param name="pattern">The pattern to be found in the input string. This pattern should be in regular expression form.</param>
        /// <returns>The substring matching the provided pattern if found; otherwise, an empty string.</returns>
        public static string find_string(string input, string pattern)
        {
            // Try to match the pattern in the input string
            Match match = Regex.Match(input, pattern);

            // Check if the match is successful
            // If yes, return the matched value; otherwise, return an empty string
            return match.Success ? match.Value : "";
        }

        /// <summary>
        /// Removes all white spaces from the given string.
        /// </summary>
        /// <param name="text">The string from which white spaces are to be removed.</param>
        /// <returns>A string with all white spaces removed.</returns>
        public static string remove_white_space(string text)
        {
            // Use regular expression to replace all white spaces ("\\s") with an empty string ("")
            return Regex.Replace(text, "\\s", "");
        }

        /// <summary>
        /// Converts the input string into a query string format.
        /// </summary>
        /// <param name="text">The string to be converted, each line should be in the form of 'key:"value"'</param>
        /// <returns>A string in the form of a query string where each key-value pair is separated by '=', and pairs are separated by '&'.</returns>
        public static string parse_to_query_string(string text)
        {
            // Initialize an empty result string
            string result = "";

            // Split the input text into separate lines
            string[] text_split = Regex.Split(text, "\\n");

            // Iterate over each line of the text
            for (int i = 0; i < text_split.Length; i++)
            {
                // Remove white spaces from the line
                text_split[i] = remove_white_space(text_split[i]);

                // If the line is empty, skip to the next line
                if (text_split[i] == "")
                {
                    continue;
                }

                // Remove any trailing comma from the line
                text_split[i] = Regex.Replace(text_split[i], ",$", "");

                // Split the line into key and value parts at the first colon ':'
                string[] text_split_split = Regex.Split(text_split[i], "(?<=^[^:]*?):");

                // Remove leading and trailing double quote from the value part
                text_split_split[1] = Regex.Replace(text_split_split[1], "^\"", "");
                text_split_split[1] = Regex.Replace(text_split_split[1], "\"$", "");

                // Append the key-value pair to the result in the form 'key=value&'
                result += string.Format("{0}={1}&", text_split_split[0], text_split_split[1]);
            }

            // Remove the trailing ampersand '&' from the result
            result = Regex.Replace(result, "&$", "");

            // Return the result string
            return result;
        }
    }
}
