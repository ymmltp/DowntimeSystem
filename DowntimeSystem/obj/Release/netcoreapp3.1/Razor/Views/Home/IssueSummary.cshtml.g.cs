#pragma checksum "C:\Users\1382919\source\repos\DowntimeSystem\DowntimeSystem\Views\Home\IssueSummary.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7a6f8742a086a683a75bda81e6c7bf9b097731f2"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_IssueSummary), @"mvc.1.0.view", @"/Views/Home/IssueSummary.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\1382919\source\repos\DowntimeSystem\DowntimeSystem\Views\_ViewImports.cshtml"
using DowntimeSystem;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\1382919\source\repos\DowntimeSystem\DowntimeSystem\Views\_ViewImports.cshtml"
using DowntimeSystem.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7a6f8742a086a683a75bda81e6c7bf9b097731f2", @"/Views/Home/IssueSummary.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c1f65d23891b8638a5e8a53fe79ce94a78a82dbb", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_IssueSummary : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/customer.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/cookie.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/bootstrap/dist/js/bootstrap-table.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/bootstrap/dist/js/bootstrap-editable.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/bootstrap/dist/js/bootstrap-table-editable.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\1382919\source\repos\DowntimeSystem\DowntimeSystem\Views\Home\IssueSummary.cshtml"
  
    ViewData["Title"] = "Action Step Maintains";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"container-fluid\">\r\n    <div class=\"panel-default\">\r\n        <div class=\"panel-heading\"><i class=\"fa fa-cogs \"></i> ");
#nullable restore
#line 7 "C:\Users\1382919\source\repos\DowntimeSystem\DowntimeSystem\Views\Home\IssueSummary.cshtml"
                                                          Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
        <div class=""panel-body"">
            <div class=""row"">
                <div class=""col-lg-2 col-xs-3 col-sm-3 col-md-2"" style=""display:none"">
                    <label class=""form-label""> System:</label>
                    <select id=""system-select"" class=""form-control selectpicker"" title=""Select System"" data-size=""6"" multiple data-max-options=""1"">
                    </select>
                </div>
                <div class=""col-lg-2 col-xs-3 col-sm-3 col-md-2"">
                    <label class=""form-label""> Department:</label>
                    <select id=""department-select"" class=""form-control selectpicker"" title=""Select Department"" data-size=""6"" multiple data-max-options=""1"" data-live-search=""true""></select>
                </div>
                <div class=""col-lg-2 col-xs-3 col-sm-3 col-md-2"">
                    <label class=""form-label""> Project:</label>
                    <select id=""project-select"" class=""form-control selectpicker"" title=""Select Project"" data-size");
            WriteLiteral(@"=""6"" multiple data-max-options=""1"" data-live-search=""true"" onchange=""getLine($('#line-select'),$('#project-select').val()); getStation($('#station-select'), $('#department-select').val(), $('#project-select').val(),$('#line-select').val())""> </select>
                </div>
                <div class=""col-lg-2 col-xs-3 col-sm-3 col-md-2"">
                    <label class=""form-label""> Line:</label>
                    <select id=""line-select"" class=""form-control selectpicker"" title=""Select Line"" data-size=""6"" multiple data-max-options=""1"" data-live-search=""true"" onchange="" getStation($('#station-select'), $('#department-select').val(), $('#project-select').val(),$('#line-select').val());""></select>
                </div>
                <div class=""col-lg-2 col-xs-3 col-sm-3 col-md-2"">
                    <label class=""form-label""> Station:</label>
                    <select id=""station-select"" class=""form-control selectpicker"" title=""Select Station"" data-size=""6"" multiple data-max-options=""1"" data-l");
            WriteLiteral(@"ive-search=""true""></select>
                </div>
                <div class=""col-lg-2 col-xs-3 col-sm-3 col-md-2"">
                    <label class=""form-label""> Defect Code:</label>
                    <input type=""text"" class=""form-control "" id=""defect-input"" placeholder=""Defect Input"" />
                </div>
                <div class=""col-lg-1  col-xs-1 col-sm-1 col-md-1"" style=""padding-top:27px""><button type=""button"" class=""btn btn-primary"" id=""analysis"" onclick=""GetIssueSummary()"">Search</button></div>
            </div>
        </div>
    </div>
</div>

<table id=""dtAnalysis""></table>

<div class=""alert""></div>

");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7a6f8742a086a683a75bda81e6c7bf9b097731f28533", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7a6f8742a086a683a75bda81e6c7bf9b097731f29572", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7a6f8742a086a683a75bda81e6c7bf9b097731f210611", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7a6f8742a086a683a75bda81e6c7bf9b097731f211651", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7a6f8742a086a683a75bda81e6c7bf9b097731f212691", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"

<script>
    $(document).ready(function () {
        getDepartment($(""#department-select""));
        getLine($('#line-select'));
        getProject($('#project-select'));
        getStation($('#station-select'));
    });

    function GetIssueSummary() {
        $(""#dtAnalysis"").bootstrapTable('destroy').bootstrapTable({
            cache: false,
            type: 'GET',
            url: '/IssueSummary/GetIssueSummary',
            queryParams: {
                Department: $(""#department-select"").val()[0],
                Project: $(""#project-select"").val()[0],
                Line: $(""#line-select"").val()[0],
                Station: $(""#station-select"").val()[0],
                issue: $(""#defect-input"").val()
            },
            dataType: 'json',
            columns: [
                {
                    field: 'department',
                    title: 'Department',
                    align: 'center',
                    valign: 'middle',
                }, {
     ");
            WriteLiteral(@"               field: 'project',
                    title: 'Project',
                    align: 'center',
                    valign: 'middle',
                }, {
                    field: 'line',
                    title: 'Line',
                    align: 'center',
                    valign: 'middle',
                }, {
                    field: 'station',
                    title: 'Station',
                    align: 'center',
                    valign: 'middle',
                },{
                    field: 'issue',
                    title: 'Defect Code',
                    align: 'center',
                    valign: 'middle',
                }, {
                    field: 'rootcause',
                    title: 'Root Cause',
                    align: 'center',
                    valign: 'middle',
                }, {
                    field: 'qty',
                    title: 'QTY',
                    align: 'center',
                    valign: 'middle'");
            WriteLiteral(@",
                }, {
                    field: 'totaldowntime',
                    title: 'Total Downtime',
                    align: 'center',
                    valign: 'middle',
                    visible:false,
                }, {
                    field: 'action',
                    title: 'Analysis Steps',
                    align: 'center',
                    valign: 'middle',
                    editable: {
                        title: 'Action',
                        type: 'textarea',
                        mode: 'inline',
                    }
                }, {
                    field: 'correctiveaction',
                    title: 'Corrective Action',
                    align: 'center',
                    valign: 'middle',
                    editable: {
                        title: 'Action',
                        type: 'textarea',
                        mode: 'inline',
                    }
                }, {
                    field: 'ed");
            WriteLiteral(@"itor',
                    title: 'Editor',
                    align: 'center',
                    valign: 'middle',
                }, {
                    field: 'lastupdatedate',
                    title: 'Update Time',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return new Date(value).format('yyyy-MM-dd hh:mm:ss');
                    },
                }, {
                    field: 'Link',
                    title: 'View EPM',
                    align: 'center',
                    valign: 'middle',
                    formatter: function (value, row, index) {
                        return ""<a href='https://10.136.16.135:4433/'>访问EPM</a>"";
                    },
                }],
            onEditableSave: function (field, row, oldValue, $el) {
                console.log(field);
                var tmp = {
                    id: row['id'],
           ");
            WriteLiteral(@"         action: row['action'],
                    correctiveaction: row['correctiveaction'],
                    editor: ""Auto"",//getCookie(""dt-displayname""),
                }
                $.ajax({
                    url: '/IssueSummary/EditIssueSummary_Action',
                    method: 'POST',
                    data: tmp,
                    success: function (data) {
                        console.log(data);
                    },
                    error: function (err) {
                        alert(err.responseText);
                    },
                    complete: function (res) {
                        $(""#dtAnalysis"").bootstrapTable(""refreshOptions"", { url: ""/IssueSummary/GetIssueSummary"" });
                    }
                })
            },
        });
    }

</script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
