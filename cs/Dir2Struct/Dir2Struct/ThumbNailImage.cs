using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
//using System.Web.UI;
using System.Drawing.Imaging;
using System.Reflection;
using System.Drawing;



using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization.Formatters;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Remoting.Contexts;
//using static System.Net.WebRequestMethods;

namespace Tools.Dir
{
    /*class ThumpNailImage : BaseHttpHandler2
    {
        public ThumpNailImage()
        {
        }
        public void WriteThumpNail(string sourcePath, string size, string targetPath)
        {
            HttpContext2 context = new HttpContext2(sourcePath, size, targetPath);
            HandleRequest(context);
        }
    }*/
    class ThumpNailImage
    {
        protected string _mimeText { get; set; }
        protected ThumbnailSizeType _sizeType { get; set; }
        protected ImageFormat _formatType { get; set; }
        protected const int IMG_PARAM = 0;
        protected const int SIZE_PARAM = 1;
        protected const int IMG_PARAM_TARGET = 2;
        protected const string DEFAULT_THUMBNAIL = "";
        /// <summary>
        /// An internal enumeration defining the thumbnail sizes.
        /// </summary>
        internal enum ThumbnailSizeType
        {
            Small = 72,
            Medium = 144,
            Large = 288
        }

        internal enum LoadType
        {
            FromFile,
            FromStream
        }


        /// <summary>
        /// Gets the MIME Type.
        /// </summary>
        public string ContentMimeType
        {
            get { return this._mimeText; }
        }
        public void WriteThumpNail(string sourcePath, string size, string targetPath)
        {
            HttpContext2 context = new HttpContext2(sourcePath, size, targetPath);
            HandleRequest(context);
        }

        /// <summary>
        /// Main interface for reacting to the Thumbnailer request.
        /// </summary>
        protected void HandleRequest(HttpContext2 context)
        {
            if (string.IsNullOrEmpty(context.Request_QueryString[SIZE_PARAM]))
                this._sizeType = ThumbnailSizeType.Small;
            else
                this.SetSize(context.Request_QueryString[SIZE_PARAM]);

            if ((string.IsNullOrEmpty(
                 context.Request_QueryString[IMG_PARAM])) ||
             (!this.IsValidImage(context.Request_QueryString[IMG_PARAM])))
            {
                //Do nothing!
                //this.GetDefaultImage(context);
            }
            else
            {
                string file =
                  context.Request_QueryString[IMG_PARAM].Trim().ToLower().Replace("\\", "/");
                if (file.IndexOf("/") != 0)
                    file = "/" + file;
                if (!File.Exists(context.Server_MapPath("~" + file)))
                    this.GetDefaultImage(context);
                else
                {
                    //TryCreateThumbnailFromStream(context.Request_QueryString[IMG_PARAM].Trim().ToLower().Replace("\\", "/"), context.Request_QueryString[IMG_PARAM_TARGET]);
                    TryCreateThumbnail(context.Request_QueryString[IMG_PARAM].Trim().ToLower().Replace("\\", "/"), context.Request_QueryString[IMG_PARAM_TARGET],ImageFormat.Jpeg, LoadType.FromStream);
                }
            }
        }
        private void CreateThumbnail(HttpContext2 context, string file)
        {
            string filePath = context.Server_MapPath("~" + file);
            if (!File.Exists(filePath))
                this.GetDefaultImage(context);
            else
            {
                using (MemoryStream ms = new MemoryStream())
                using (System.Drawing.Image im = System.Drawing.Image.FromFile(filePath))
                using (System.Drawing.Image tn = this.CreateThumbnail(im))
                {
                    tn.Save(ms, ImageFormat.Jpeg //this._formatType
                        );
                    File.WriteAllBytes(context.Request_QueryString[IMG_PARAM_TARGET], ms.ToArray());
                }
            }
        }
        private void CreateThumbnailByStream(HttpContext2 context, string file)
        {
            string filePath = context.Server_MapPath("~" + file);
            if (!File.Exists(filePath))
                this.GetDefaultImage(context);
            else
            {
                TryCreateThumbnailFromStream(context.Server_MapPath("~" + file), context.Request_QueryString[IMG_PARAM_TARGET]);
            }
        }
        public bool MoveFile(string inFile, string outFile)
        {
            try
            {
                if (File.Exists(inFile) && !File.Exists(outFile))
                {
                    File.Move(inFile, outFile);
                    if (!File.Exists(inFile) && File.Exists(outFile))
                        return true;
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                System.Console.WriteLine("Error:{0} When loading:{1}", errmsg, inFile);
            }
            return false;
        }

        private void TryCreateThumbnailFromStream(string inFile, string outFile)
        {
            try
            {
                if (!File.Exists(outFile))
                {
                    using (MemoryStream ms = new MemoryStream())
                    using (FileStream fs = new FileStream(inFile, FileMode.Open, FileAccess.Read))
                    using (System.Drawing.Image im = System.Drawing.Image.FromStream(fs))
                    using (System.Drawing.Image tn = this.CreateThumbnail(im))
                    {
                        tn.Save(ms, ImageFormat.Jpeg //this._formatType
                            );
                        File.WriteAllBytes(outFile, ms.ToArray());
                    }
                }
            }
            catch(Exception ex)
            {
                string errmsg = ex.Message;
                System.Console.WriteLine("Error:{0} When loading:{1}", errmsg, inFile);
            }
        }
        private void TryCreateThumbnailFromFile(string inFile, string outFile, ImageFormat formatType)
        {
            try
            {
                if (!File.Exists(outFile))
                {
                    using (MemoryStream ms = new MemoryStream())
                    using (System.Drawing.Image im = System.Drawing.Image.FromFile(inFile))
                    using (System.Drawing.Image tn = this.CreateThumbnail(im))
                    {
                        tn.Save(ms, formatType);
                        File.WriteAllBytes(outFile, ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                System.Console.WriteLine("Error:{0} When loading:{1}", errmsg, inFile);
            }
        }
        /*
        private void CreateThumbnailFromStream(string inFile, string outFile, ImageFormat formatType)
        {
            using (MemoryStream ms = new MemoryStream())
            using (FileStream fs = new FileStream(inFile, FileMode.Open, FileAccess.Read))
            using (System.Drawing.Image im = System.Drawing.Image.FromStream(fs))
            using (System.Drawing.Image tn = this.CreateThumbnail(im))
            {
                tn.Save(ms, formatType);
                File.WriteAllBytes(outFile, ms.ToArray());
            }
        }
        */
        #region LoadTypes
        private void TryCreateThumbnail(string inFile, string outFile, ImageFormat formatType, LoadType loadType)
        {
            try
            {
                if (!File.Exists(outFile))
                {
                    CreateThumbnail(inFile, outFile, formatType, loadType);
                }
            }
            catch (Exception ex)
            {
                string errmsg = ex.Message;
                System.Console.WriteLine("Error:{0} When loading:{1}", errmsg, inFile);
            }
        }
        private void CreateThumbnail(string inFile, string outFile, ImageFormat formatType, LoadType loadType)
        {
            using (MemoryStream ms = new MemoryStream())
            using (FileStream fs = LoadFileStream(inFile, loadType))
            using (System.Drawing.Image im = LoadImage(inFile, fs, loadType))
            using (System.Drawing.Image tn = this.CreateThumbnail(im))
            {
                tn.Save(ms, formatType);
                File.WriteAllBytes(outFile, ms.ToArray());
            }
        }

        private FileStream LoadFileStream(string filePath, LoadType loadType)
        {
            switch (loadType)
            {
                case LoadType.FromFile:
                    return null;
                case LoadType.FromStream:
                    return new FileStream(filePath, FileMode.Open, FileAccess.Read);
            }
            return null;
        }
        private System.Drawing.Image LoadImage(string filePath, FileStream fStream, LoadType loadType)
        {
            switch (loadType)
            {
                case LoadType.FromFile:
                    if (filePath != null)
                        return System.Drawing.Image.FromFile(filePath);
                    break;
                case LoadType.FromStream:
                    if (fStream != null)
                        return System.Drawing.Image.FromStream(fStream);
                    break;
            }
            return null;
        }
        #endregion LoadTypes

        /// <summary>
        /// Sets the size of the thumbnail base on the size parameter.
        /// </summary>
        /// <param name="size">The size parameter.</param>
        private void SetSize(string size)
        {
            int sizeVal;
            if (!Int32.TryParse(size.Trim(),
                System.Globalization.NumberStyles.Integer,
                null, out sizeVal))
                sizeVal = (int)ThumbnailSizeType.Small;

            try
            {
                this._sizeType = (ThumbnailSizeType)sizeVal;
            }
            catch
            {
                this._sizeType = ThumbnailSizeType.Small;
            }
        }

        /// <summary>
        /// Determines if the img parameter is a valid image.
        /// </summary>
        /// <param name="fileName">File name from
        ///        the img parameter.</param>
        /// <returns>
        ///   <c>true</c> if valid image, otherwise <c>false</c>
        /// </returns>
        public bool IsValidImage(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            bool isValid = false;
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                    isValid = true;
                    this._mimeText = "image/jpeg";
                    this._formatType = ImageFormat.Jpeg;
                    break;
                case ".gif":
                    isValid = true;
                    this._mimeText = "image/gif";
                    this._formatType = ImageFormat.Gif;
                    //this._formatType = ImageFormat.Jpeg;
                    break;
                case ".png":
                    isValid = true;
                    this._mimeText = "image/png";
                    this._formatType = ImageFormat.Png;
                    //this._formatType = ImageFormat.Jpeg;
                    break;
                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }
        public static bool IsValidImage_(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            bool isValid = false;
            switch (ext)
            {
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".png":
                    isValid = true;
                    break;
                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }

        /// <summary>
        /// Get default image.
        /// </summary>
        /// <remarks>
        /// This method is only invoked when
        /// there is a problem with the parameters.
        /// </remarks>
        /// <param name="context"></param>
        private void GetDefaultImage(HttpContext2 context)
        {
            Assembly a = Assembly.GetAssembly(this.GetType());
            Stream imgStream = null;
            Bitmap bmp = null;
            string file = string.Format("{0}{1}{2}",
                          DEFAULT_THUMBNAIL,
                          (int)this._sizeType, ".gif");

            imgStream = a.GetManifestResourceStream(a.GetName().Name + file);
            if (imgStream != null)
            {
                bmp = (Bitmap.FromStream(imgStream) as Bitmap);
                bmp.Save(context.Response_OutputStream, this._formatType);

                imgStream.Close();
                bmp.Dispose();
            }
        }

        /// <summary>
        /// This method generates the actual thumbnail.
        /// </summary>
        /// <param name="src"></param>
        /// <returns>Thumbnail image</returns>
        private System.Drawing.Image CreateThumbnail(System.Drawing.Image src)
        {
            int maxSize = (int)this._sizeType;

            int w = src.Width;
            int h = src.Height;

            if (w > maxSize)
            {
                h = (h * maxSize) / w;
                w = maxSize;
            }

            if (h > maxSize)
            {
                w = (w * maxSize) / h;
                h = maxSize;
            }

            // The third parameter is required and is
            // of type delegate.  Rather then create a method that
            // does nothing, .NET 2.0 allows for anonymous
            // delegate (similar to anonymous functions in other languages).
            return src.GetThumbnailImage(w, h, delegate () { return false; }, IntPtr.Zero);
        }
    }
    class HttpContext2
    {
        private IList<FileInfo> Repeater1_DataSource { get; set; }
        public string[] Request_QueryString { get; set; }
        public Stream Response_OutputStream { get; set; }
        public HttpContext2(string sourcePath, string size, string targetPath)
        {
            this.Request_QueryString = new string[3] { sourcePath.Trim().ToLower().Replace("\\", "/"), size, targetPath.Trim().Replace("\\", "/") };
            this.Response_OutputStream = new MemoryStream();
        }
        public string Server_MapPath(string path)
        {
            return Request_QueryString[0];
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<FileInfo> files = new List<FileInfo>();
            string filters = "*.jpg;*.png;*.gif";

            foreach (string filter in filters.Split(';'))
            {
                FileInfo[] fit = new DirectoryInfo(
                           this.Server_MapPath("~/im")).GetFiles(filter);
                foreach (FileInfo fi in fit)
                {
                    files.Add(fi);
                }
            }
            this.Repeater1_DataSource = files;
            this.Repeater1_DataBind();
        }
        private void Repeater1_DataBind()
        {

        }
    }
}