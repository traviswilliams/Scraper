using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable 1591
namespace Scraper.Web.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Values = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Values { get; private set; }
    }
}
#pragma warning restore 1591
