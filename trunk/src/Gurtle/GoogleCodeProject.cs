#region License, Terms and Author(s)
//
// Gurtle - IBugTraqProvider for Google Code
// Copyright (c) 2008 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// This library is free software; you can redistribute it and/or modify it 
// under the terms of the New BSD License, a copy of which should have 
// been delivered along with this distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
#endregion

namespace Gurtle
{
    #region Imports

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text.RegularExpressions;

    #endregion

    internal class GoogleCodeProject
    {
        public static readonly Uri HostingUrl = new Uri("http://code.google.com/hosting/");

        public GoogleCodeProject(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (!IsValidProjectName(name)) throw new ArgumentException(null, "name");

            Debug.Assert(name != null);
            Debug.Assert(IsValidProjectName(name));

            Name = name;
            Url = FormatUrl(null);
        }

        public string Name { get; private set; }

        public Uri Url { get; private set; }

        public Uri DnsUrl()
        {
            return new Uri("http://" + Name + ".googlecode.com/");
        }

        public Uri IssueDetailUrl(int id)
        {
            return FormatUrl("issues/detail?id={0}", id);
        }

        public Uri RevisionDetailUrl(int revision)
        {
            return FormatUrl("source/detail?r={0}", revision);
        }

        public Uri DownloadsListUrl()
        {
            return FormatUrl("downloads/list");
        }

        public Uri IssueOptionsFeedUrl()
        {
            return FormatUrl("feeds/issueOptions");
        }        

        public Uri IssuesCsvUrl(int start)
        {
            return IssuesCsvUrl(start, false);
        }

        public Uri IssuesCsvUrl(int start, bool includeClosed)
        {
            return FormatUrl("issues/csv?start={0}&colspec={1}{2}",
                       start.ToString(CultureInfo.InvariantCulture),
                       string.Join("%20", Enum.GetNames(typeof(IssueField))),
                       includeClosed ? "&can=1" : string.Empty);
        }

        private Uri FormatUrl(string relativeUrl)
        {
            var baseUrl = new Uri("http://code.google.com/p/" + Name + "/");
            return string.IsNullOrEmpty(relativeUrl) ? baseUrl : new Uri(baseUrl, relativeUrl);
        }

        private Uri FormatUrl(string relativeUrl, params object[] args)
        {
            return FormatUrl(string.Format(CultureInfo.InvariantCulture, relativeUrl, args));
        }

        public static bool IsValidProjectName(string name)
        {
            if (name == null) 
                throw new ArgumentNullException("name");
            
            //
            // From http://code.google.com/hosting/createProject:
            //
            //   "...project's projectName must consist of a lowercase letter, 
            //    followed by lowercase letters, digits, and dashes, 
            //    with no spaces."
            //

            return name.Length > 0 && Regex.IsMatch(name, @"^[a-z][a-z0-9-]*$");
        }
    }
}