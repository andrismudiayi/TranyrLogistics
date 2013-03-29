using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TranyrLogistics.Controllers.Utility
{
    public class FileContent
    {
        private static Dictionary<string, string> contentTypeConfig = null;

        public static Dictionary<string, string> ContentTypeConfig
        {
            get
            {
                if (contentTypeConfig == null)
                {
                    contentTypeConfig = new Dictionary<string, string>();
                    // Images
                    contentTypeConfig.Add(".bmp", "image/bmp");
                    contentTypeConfig.Add(".gif", "image/gif");
                    contentTypeConfig.Add(".jpeg", "image/jpeg");
                    contentTypeConfig.Add(".jpg", "image/jpeg");
                    contentTypeConfig.Add(".png", "image/png");
                    contentTypeConfig.Add(".tif", "image/tiff");
                    contentTypeConfig.Add(".tiff", "image/tiff");
                    // Documents
                    contentTypeConfig.Add(".doc", "application/msword");
                    contentTypeConfig.Add(".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                    contentTypeConfig.Add(".pdf", "application/pdf");
                    // Slideshows
                    contentTypeConfig.Add(".ppt", "application/vnd.ms-powerpoint");
                    contentTypeConfig.Add(".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
                    // Data
                    contentTypeConfig.Add(".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    contentTypeConfig.Add(".xls", "application/vnd.ms-excel");
                    contentTypeConfig.Add(".csv", "text/csv");
                    contentTypeConfig.Add(".xml", "text/xml");
                    contentTypeConfig.Add(".txt", "text/plain");
                    // Compressed Folders
                    contentTypeConfig.Add(".zip", "application/zip");
                    // Audio
                    contentTypeConfig.Add(".ogg", "application/ogg");
                    contentTypeConfig.Add(".mp3", "audio/mpeg");
                    contentTypeConfig.Add(".wma", "audio/x-ms-wma");
                    contentTypeConfig.Add(".wav", "audio/x-wav");
                    // Video
                    contentTypeConfig.Add(".wmv", "audio/x-ms-wmv");
                    contentTypeConfig.Add(".swf", "application/x-shockwave-flash");
                    contentTypeConfig.Add(".avi", "video/avi");
                    contentTypeConfig.Add(".mp4", "video/mp4");
                    contentTypeConfig.Add(".mpeg", "video/mpeg");
                    contentTypeConfig.Add(".mpg", "video/mpeg");
                    contentTypeConfig.Add(".qt", "video/quicktime");
                }

                return contentTypeConfig;
            }
        }

        public static string GetType(string fileExtension)
        {
            if (!ContentTypeConfig.ContainsKey(fileExtension))
            {
                throw new ArgumentException("Unsupported content type or unknown content type specified.");
            }

            return ContentTypeConfig[fileExtension];
        }
    }
}