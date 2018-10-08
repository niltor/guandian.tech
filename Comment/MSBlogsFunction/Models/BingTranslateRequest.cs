using System;
using System.Collections.Generic;
using System.Text;

namespace MSBlogsFunction.Models
{
    class BingTranslateRequest
    {

        public string Text { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
    }
}
