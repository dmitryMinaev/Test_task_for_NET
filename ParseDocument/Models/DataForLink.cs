﻿using System;

namespace InterviewTask.Logic.Models
{
    public class DataForLink
    {
        public Uri Link { get; set; }
        public int ResponseTime { get; set; }
        public bool IsLinkFromHtml { get; set; }
        public bool IsLinkFromSitemap { get; set; }
    }
}
