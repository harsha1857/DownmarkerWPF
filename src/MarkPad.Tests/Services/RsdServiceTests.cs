﻿using MarkPad.DocumentSources.MetaWeblog.Service.Rsd;
using Xunit;

namespace MarkPad.Tests.Services
{
    public class RsdServiceTests
    {
        readonly TestWebRequestFactory webRequestFactory;
        readonly RsdService rsdService;

        public RsdServiceTests()
        {
            webRequestFactory = new TestWebRequestFactory();
            rsdService = new RsdService(webRequestFactory);
        }

        [Fact]
        public void discovers_codeplex_metaweblog_api()
        {
            // arrange
            webRequestFactory.RegisterResultForUri("http://project.codeplex.com", CodeplexProjectPage);
            webRequestFactory.RegisterResultForUri("http://project.codeplex.com/rsd", CodeplexProjectRsdFile);

            // act
            var result = rsdService.DiscoverAddress("http://project.codeplex.com").Result;

            // assert
            Assert.True(result.Success);
            Assert.Equal("https://www.codeplex.com/site/metaweblog", result.MetaWebLogApiLink);
        }

        [Fact]
        public void discovers_site_with_rsdxml_file_in_root()
        {
            // arrange
            webRequestFactory.RegisterResultForUri("http://funnelweblog.net/rsd.xml", FunnelWebRsdFile);

            // act
            var result = rsdService.DiscoverAddress("http://funnelweblog.net").Result;

            // assert
            Assert.True(result.Success);
            Assert.Equal("http://funnelweblog.net/blogapi", result.MetaWebLogApiLink);
        }

        private const string FunnelWebRsdFile = @"<rsd version=""1.0"" xmlns=""http://archipelago.phrasewise.com/rsd"">
  <service>
    <engineName>FunnelWeblog</engineName>
    <engineLink>http://www.funnelweblog.com/</engineLink>
    <homePageLink>http://funnelweblog.net/</homePageLink>
    <apis>
      <api name=""MetaWeblog"" preferred=""true"" apiLink=""http://funnelweblog.net/blogapi"" blogId=""something"" />
    </apis>
  </service>
</rsd>";

        private const string CodeplexProjectPage = @"<!DOCTYPE html>
<head>
<link rel=""EditURI"" type=""application/rsd+xml"" title=""RSD"" href='http://project.codeplex.com/rsd' />

        private const string CodeplexProjectRsdFile = @"<rsd version=""1.0"">
<service>
<engineName>CodePlex</engineName>
<engineLink>http://www.codeplex.com</engineLink>
<homePageLink>http://phoenixframework.codeplex.com/</homePageLink>
<apis>
<api name=""MetaWeblog"" blogID=""project"" preferred=""true"" apiLink=""https://www.codeplex.com/site/metaweblog""/>
</apis>
</service>
</rsd>";
    }
}