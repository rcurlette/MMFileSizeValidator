using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toyota.Tridion.EventSystem;
using Tridion.ContentManager;
using Tridion.ContentManager.CommunicationManagement;
using Tridion.ContentManager.ContentManagement;

namespace ValidateMultimediaFilesize
{
    public static class Config
    {
        private static Dictionary<string, MaxFilesizeDef> MultimediaMaxFileSizes = 
            new Dictionary<string, MaxFilesizeDef>();
        public static MaxFilesizeDef GetMaxFilesizes(Repository publication)
        {
            if(!MultimediaMaxFileSizes.ContainsKey(publication.Id.ToString()))
            {
                // create a new maxsizes for this publication and add it to dict
                LoadDictionary(publication);
            }
            
            return MultimediaMaxFileSizes[publication.Id.ToString()]; ;
        }

        private static void LoadDictionary(Repository publication)
        {
            Publication pub = (Publication)publication;
            if (pub.MetadataSchema == null)
            {
                // set default maxSizes for this pub
                MaxFilesizeDef maxSizesDef = new MaxFilesizeDef();
                MultimediaMaxFileSizes[publication.Id.ToString()] = maxSizesDef;
            }
            else
            {
                // get the sizes from the pub and use them
                
            }
        }
    }
}
