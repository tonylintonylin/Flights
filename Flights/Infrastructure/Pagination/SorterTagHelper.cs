using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Flights
{
    // Taghelper for sorting
    [HtmlTargetElement("a", Attributes = SortAttributeName)]
    public class SorterTagHelper : TagHelper
    {
        private const string SortAttributeName = "sort";

        [HtmlAttributeName(SortAttributeName)]
        public string Sort { get; set; }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var model = ((dynamic)(ViewContext.View)).RazorPage.Model as PagedModel;

            if (model == null)
            {
                throw new InvalidCastException("The <a sort=''></a> TagHelper requires that @model derives from PagedModel");
            }

            var tagBuilder = new TagBuilder("a");

            if (model.UnsignedSort.Equals(Sort, StringComparison.CurrentCultureIgnoreCase))
                tagBuilder.AddCssClass("selected-" + (model.Sort.StartsWith("-") ? "desc" : "asc"));
            else
                tagBuilder.AddCssClass("selected-none");

            var sort = Reverse(Sort, model.Sort);
            tagBuilder.MergeAttribute("data-sort", sort);
            tagBuilder.MergeAttribute("href", "javascript:void(0);");

            // Translate content
            var childContent = await output.GetChildContentAsync();

            // output.Attributes.Clear();
            output.Content.Clear();

            using (var writer = new StringWriter())
            {
                childContent.WriteTo(writer, HtmlEncoder.Default);
                var content = writer.ToString();
                
                output.Content.Append(content);
            }

            output.MergeAttributes(tagBuilder);
        }

        private string Reverse(string sort, string originalSort)
        {
            return originalSort.StartsWith("-") ? sort : "-" + sort;
        }
    }
}
