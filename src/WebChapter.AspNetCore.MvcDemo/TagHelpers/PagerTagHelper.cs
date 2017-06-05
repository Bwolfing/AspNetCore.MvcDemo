using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using WebChapter.AspNetCore.MvcDemo.Models;

namespace WebChapter.AspNetCore.MvcDemo.TagHelpers
{
    [HtmlTargetElement("demo-pager", Attributes = PagerModelAttribute, TagStructure = TagStructure.WithoutEndTag)]
    public class PagerTagHelper : TagHelper
    {
        private const string OutputTag = "div";
        private const TagMode OutputTagMode = TagMode.StartTagAndEndTag;
        private const string CssClass = "ui right floated pagination menu";
        private const string PagerModelAttribute = "model";

        [HtmlAttributeName(PagerModelAttribute)]
        public IPaginatedViewModel Model { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Model == null)
            {
                throw new ArgumentException("Cannot have a null model");
            }

            output.TagName = OutputTag;
            output.TagMode = OutputTagMode;

            if (Model.LastPage == 0 || Model.LastPage == 1)
            {
                output.Content.SetHtmlContent("");
                return base.ProcessAsync(context, output);
            }

            output.Attributes.SetAttribute("class", CssClass);

            output.Content.AppendHtml(CreateFirstPageArrow());
            output.Content.AppendHtml(CreatePreviousPageArrow());
            output.Content.AppendHtml(CreateNumberedButtons());
            output.Content.AppendHtml(CreateNextPageArrow());
            output.Content.AppendHtml(CreateLastPageArrow());

            return base.ProcessAsync(context, output);
        }

        private IHtmlContent CreateFirstPageArrow()
        {
            var builder = new HtmlContentBuilder();

            if (Model.CurrentPage != 1 && Model.CurrentPage - 1 != 1)
            {
                builder.SetHtmlContent($@"<a class='item' href='{Model.PagerUrl}?page=1'>
    <i class='double left angle icon'></i>
</a>");
            }

            return builder;
        }

        private IHtmlContent CreatePreviousPageArrow()
        {
            var builder = new HtmlContentBuilder();

            if (Model.CurrentPage - 1 > 0)
            {
                builder.SetHtmlContent($@"<a class='item' href='{Model.PagerUrl}?page={Model.CurrentPage - 1}'>
    <i class='left angle icon'></i>
</a>");
            }

            return builder;
        }

        private IHtmlContent CreateNumberedButtons()
        {
            var builder = new HtmlContentBuilder();

            if (Model.CurrentPage - 2 >= 1)
            {
                builder.AppendHtml("<div class='disabled item'>...</div>");
            }

            if (Model.CurrentPage != 1)
            {
                builder.AppendHtml(NeighborButton(Model.CurrentPage - 1));
            }

            builder.AppendHtml($"<div class='active item'>{Model.CurrentPage}</div>");

            if (Model.CurrentPage != Model.LastPage)
            {
                builder.AppendHtml(NeighborButton(Model.CurrentPage + 1));
            }

            if (Model.CurrentPage + 2 <= Model.LastPage)
            {
                builder.AppendHtml("<div class='disabled item'>...</div>");
            }

            return builder;
        }

        private IHtmlContent NeighborButton(int page)
        {
            var builder = new HtmlContentBuilder();

            builder.SetHtmlContent($"<a class='item' href='{Model.PagerUrl}?page={page}'>{page}</a>");

            return builder;
        }

        private IHtmlContent CreateNextPageArrow()
        {
            var builder = new HtmlContentBuilder();

            if (Model.CurrentPage + 1 <= Model.LastPage)
            {
                builder.SetHtmlContent($@"<a class='item' href='{Model.PagerUrl}?page={Model.CurrentPage + 1}'>
    <i class='right angle icon'></i>
</a>");
            }

            return builder;
        }

        private IHtmlContent CreateLastPageArrow()
        {
            var builder = new HtmlContentBuilder();

            if (Model.CurrentPage != Model.LastPage && Model.CurrentPage + 1 != Model.LastPage)
            {
                builder.SetHtmlContent($@"<a class='item' href='{Model.PagerUrl}?page={Model.LastPage}'>
    <i class='double right angle icon'></i>
</a>");
            }

            return builder;
        }
    }
}
