﻿using InterviewTask.CrawlerLogic.Crawlers;
using InterviewTask.CrawlerLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewTask.CrawlerLogic.Services
{
    public class WebsiteCrawler
    {
        private readonly HtmlCrawler _parserHtml;
        private readonly SitemapCrawler _parserSitemap;

        public WebsiteCrawler(HtmlCrawler parseHtml, SitemapCrawler parseSitemap)
        {
            _parserHtml = parseHtml;
            _parserSitemap = parseSitemap;
        }

        public async Task<IEnumerable<Link>> StartAsync(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var listLinksHtml = await _parserHtml.StartParseAsync(inputLink);

            var listLinksSitemap = await _parserSitemap.ParseAsync(inputLink);

            var listAllLinks = ConcatLists(listLinksHtml, listLinksSitemap);

            return listAllLinks.ToList();
        }

        private IEnumerable<Link> ConcatLists(IEnumerable<Uri> listLinksHtml, IEnumerable<Uri> listLinksSitemap)
        {
            var intersectLinks = listLinksHtml.Intersect(listLinksSitemap);

            var listUniqueLinks = intersectLinks.Select(s => new Link()
            {
                Url = s,
                IsLinkFromHtml = true,
                IsLinkFromSitemap = true
            });

            var onlyLinksHtml = listLinksHtml.Except(intersectLinks)
                .Select(s => new Link()
                {
                    Url = s,
                    IsLinkFromHtml = true,
                    IsLinkFromSitemap = false
                });

            var onlyLinksSitemap = listLinksSitemap.Except(intersectLinks)
                .Select(s => new Link()
                {
                    Url = s,
                    IsLinkFromHtml = false,
                    IsLinkFromSitemap = true
                });

            var listAllLinks = new List<Link>(listUniqueLinks);
            listAllLinks.AddRange(onlyLinksHtml);
            listAllLinks.AddRange(onlyLinksSitemap);

            return listAllLinks;
        }
    }
}