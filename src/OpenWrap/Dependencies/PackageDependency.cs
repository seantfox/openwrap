using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenWrap.Dependencies
{
    public class PackageDependency : IPackageDependency
    {
        public PackageDependency(
            string name, 
            IEnumerable<VersionVertex> versions = null,
            IEnumerable<string> tags = null)
        {
            Name = name;
            Tags = tags != null ? new List<string>(tags).AsReadOnly() : Enumerable.Empty<string>();
            VersionVertices = versions != null ? new List<VersionVertex>(versions).AsReadOnly() : Enumerable.Empty<VersionVertex>();
            ContentOnly = Tags.ContainsNoCase("content");
            Anchored = Tags.ContainsNoCase("anchored");

        }
        public string Name { get; private set; }

        public IEnumerable<VersionVertex> VersionVertices { get; private set; }

        public bool Anchored { get; private set; }

        public bool ContentOnly { get; private set; }

        public IEnumerable<string> Tags { get; set; }

        public bool IsFulfilledBy(Version version)
        {
            return VersionVertices.All(x => x.IsCompatibleWith(version));
        }
        public override string ToString()
        {
            var versions = VersionVertices.Select(x=>x.ToString()).Join(" and ");
            var returnValue = versions.Length == 0
                ? Name
                : Name + " " + versions;
            if (Tags.Count() > 0)
                returnValue += " " + string.Join(" ", Tags.ToArray());
            return returnValue;
        }

        internal bool IsExactlyFulfilledBy(Version version)
        {
            bool exactlyFulfilled = false;
            if (VersionVertices.Count() == 1)
            {
                var thisVersion = VersionVertices.FirstOrDefault();
                if (thisVersion is ExactVersionVertex)
                {
                    // only exactly fulfilled if both are of the same length
                    if (thisVersion.Version.Major
                        == version.Major
                        && thisVersion.Version.Minor
                        == version.Minor
                        && thisVersion.Version.Build
                        == version.Build
                        && thisVersion.Version.Revision == -1
                        && version.Revision == -1)
                    {
                        exactlyFulfilled = true;
                    }

                }


            }
            return exactlyFulfilled;
        }
    }
}