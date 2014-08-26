using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Toyota.Tridion.EventSystem
{
    public class MaxFilesizeDef
    {
        long _maxImageSize = 4194304;
        public long maxImageSize
        {
            get
            {
                return _maxImageSize;
            }    

            set
            {
                _maxImageSize = value;
            }
        }

        long _maxPdfSize = 20971520;
        public long maxPdfSize
        {
            get
            {
                return _maxPdfSize;
            }

            set
            {
                _maxPdfSize = value;
            }
        }

        long _maxMovieSize = 41943040;
        public long maxMovieSize
        {
            get
            {
                return _maxMovieSize;
            }

            set
            {
                _maxMovieSize = value;
            }
        }

        public void LoadSizesFromConfig(ConfigurationManager config)
        {
            if (config.Configurations.Count < 1)
                return;

            if(config.Configurations.ContainsKey("maxMovieSize"))
            {
                _maxMovieSize = Int64.Parse(config.Configurations["maxMovieSize"]);
            }

            if (config.Configurations.ContainsKey("maxPdfSize"))
            {
                _maxPdfSize = Int64.Parse(config.Configurations["maxPdfSize"]);
            }

            if (config.Configurations.ContainsKey("maxImageSize"))
            {
                _maxImageSize = Int64.Parse(config.Configurations["maxImageSize"]);
            }
        }
    }
}
