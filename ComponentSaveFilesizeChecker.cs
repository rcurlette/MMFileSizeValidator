using System;
using System.Drawing;
using System.IO;
using System.Linq;
using Tridion.ContentManager;
using Tridion.ContentManager.ContentManagement;
using Tridion.ContentManager.Extensibility;
using Tridion.ContentManager.Extensibility.Events;
using ValidateMultimediaFilesize;

namespace Toyota.Tridion.EventSystem
{

    
    // Based on example code from https://code.google.com/p/tridion-practice/source/browse/#git%2FImageSizeChecker
    
    [TcmExtension("ComponentSave")]
    public class ComponentSave : TcmExtension
    {

        public ComponentSave() 
        {
            EventSystem.Subscribe<Component, SaveEventArgs>( ComponentSaveInitiatedHandler, EventPhases.Initiated);
        }

        private void ComponentSaveInitiatedHandler(Component component, SaveEventArgs args, EventPhases phase)
        {
            ValidateMultimediaFilesize(component);
        }

        private void ValidateMultimediaFilesize(Component component)
        {
            string[] interestingMimeTypes = new string[] { 
                "image/jpeg", "image/gif", "image/png", "image/x-bmp" ,
                "application/pdf",
                "video/mp4", "video/mpeg"
            };

            if(component.BinaryContent == null)
            { 
                return; 
            }

            string mmMimeType = component.BinaryContent.MultimediaType.MimeType;
            if (!interestingMimeTypes.Contains(mmMimeType))
            { 
                return;
            }
                
            if (component.BinaryContent == null)
            {
                return;   
            }
            
            using (MemoryStream mem = new MemoryStream())
            {
                component.BinaryContent.WriteToStream(mem);
                Bitmap bitmap = null;
                try
                {
                    bitmap = new Bitmap(mem);
                    Repository publication = component.ContextRepository;
                    if (mem.Length > GetAllowedSize(mmMimeType, component))
                    {
                        throw new WrongFileSizeException(
                            string.Format("Sorry, file exceeds the allowed size for {0} files.  The allowed size is {1}"
                            , mmMimeType, GetSize(GetAllowedSize(mmMimeType, component))));
                    }
                }
                catch (System.ArgumentException)
                {
                    if (mem.Length > 1)
                    {
                        throw new WrongFileSizeException("Unable to process this image, probably because it is too large, or not in a recognised image format.");
                    }
                    else throw;
                }
                finally
                {
                    if (bitmap != null) { bitmap.Dispose(); }
                }
            }
        }

        private long GetAllowedSize(string mmMimeType, Component component)
        {
            // http://www.whatsabyte.com/P1/byteconverter.htm
            // Defaults
            // - PDF	-> 20MB
            // - Movie	-> 40MB
            // - Image	-> 4MB

            long allowedSize = 41943040;  // 40 MB
            ConfigurationManager configManager = ConfigurationManager.GetInstance(component);

            //long allowedImageSize = 4194304;   // 4MB
            //long allowedPDFSize = 31457280;    // 30MB
            //long allowedMovieSize = 41943040;  // 40MB
            MaxFilesizeDef maxFileSizes = new MaxFilesizeDef();
            maxFileSizes.LoadSizesFromConfig(configManager);

            // ***MaxFilesizeDef maxSizes = Config.GetMaxFilesizes(publication);
            switch(mmMimeType) {
                // Images
                case "image/jpeg":
                case "image/gif": 
                case "image/png":
                case "image/x-bmp" :
                    allowedSize = maxFileSizes.maxImageSize;
                    break;

                // PDF
                case "application/pdf" :
                    allowedSize = maxFileSizes.maxPdfSize;
                    break;

                // Movie
                case "video/mp4" :
                case "video/mpeg" :
                    allowedSize = maxFileSizes.maxMovieSize;
                    break;
            }

            return allowedSize;
        }

        private string GetSize(double streamLength)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (streamLength >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                streamLength = streamLength / 1024;
            }
            
            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            string result = String.Format("{0:0.##} {1}", streamLength, sizes[order]);
            return result;
        }

         [Serializable]
        public class WrongFileSizeException : Exception
        {
            public WrongFileSizeException() { }
            public WrongFileSizeException(string message) : base(message) { }
            public WrongFileSizeException(string message, Exception inner) : base(message, inner) { }
            protected WrongFileSizeException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
}
