using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Text;
using Sitecore.Web.UI.HtmlControls.Data;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections;
using System.Web;
using System.Web.UI;

namespace UserAuditReport.CustomFields
{
    public class RolesMultilistSitecore : Sitecore.Shell.Applications.ContentEditor.MultilistEx
    {
        /// <summary>Gets the selected items.</summary>
        /// <param name="sources">The sources. Never used</param>
        /// <param name="selected">The selected.</param>
        /// <param name="unselected">The unselected.</param>
        /// <contract>
        ///   <requires name="sources" condition="not null" />
        /// </contract>
        protected override void GetSelectedItems(Item[] sources, out ArrayList selected, out IDictionary unselected)
        {
            //Assert.ArgumentNotNull((object)sources, "sources");

            var roles = Roles.GetAllRoles();

            ListString listString = new ListString(this.Value);
            unselected = (IDictionary)new SortedList((IComparer)StringComparer.Ordinal);
            selected = new ArrayList(listString.Count);

            for (int index = 0; index < listString.Count; ++index)
                selected.Add((object)listString[index]);

            foreach (var role in roles)
            {
                int index = listString.IndexOf(role);
                if (index >= 0)
                    selected[index] = (object)role;
                else
                    unselected.Add((object)MainUtil.GetSortKey(role), (object)role);
            }

            //----------------------------------------------------------------------
            //ListString listString = new ListString(this.Value);
            //unselected = (IDictionary)new SortedList((IComparer)StringComparer.Ordinal);
            //selected = new ArrayList(listString.Count);
            //for (int index = 0; index < listString.Count; ++index)
            //    selected.Add((object)listString[index]);
            //foreach (Item source in sources)
            //{
            //    string @string = source.ID.ToString();
            //    int index = listString.IndexOf(@string);
            //    if (index >= 0)
            //        selected[index] = (object)source;
            //    else
            //        unselected.Add((object)MainUtil.GetSortKey(source.Name), (object)source);
            //}
        }

        protected override void DoRender(HtmlTextWriter output)
        {
            Assert.ArgumentNotNull((object)output, "output");
            Item current = Sitecore.Configuration.Factory.GetDatabase("master").GetItem(this.ItemID, Language.Parse(this.ItemLanguage));
            Item[] sources = (Item[])null;
            using (new LanguageSwitcher(this.ItemLanguage))
                sources = LookupSources.GetItems(current, this.Source);
            ArrayList selected;
            IDictionary unselected;
            this.GetSelectedItems(sources, out selected, out unselected);
            this.ServerProperties["ID"] = (object)this.ID;
            string str1 = string.Empty;
            if (this.ReadOnly)
                str1 = " disabled=\"disabled\"";
            output.Write("<input id=\"" + this.ID + "_Value\" type=\"hidden\" value=\"" + StringUtil.EscapeQuote(this.Value) + "\" />");
            output.Write("<div class='scContentControlMultilistContainer'>");
            output.Write("<table" + this.GetControlAttributes() + ">");
            output.Write("<tr>");
            output.Write("<td class=\"scContentControlMultilistCaption\" width=\"50%\">" + Translate.Text("All") + "</td>");
            output.Write("<td width=\"20\">" + Images.GetSpacer(20, 1) + "</td>");
            output.Write("<td class=\"scContentControlMultilistCaption\" width=\"50%\">" + Translate.Text("Selected") + "</td>");
            output.Write("<td width=\"20\">" + Images.GetSpacer(20, 1) + "</td>");
            output.Write("</tr>");
            output.Write("<tr>");
            output.Write("<td valign=\"top\" height=\"100%\">");
            output.Write("<select id=\"" + this.ID + "_unselected\" class=\"scContentControlMultilistBox\" multiple=\"multiple\" size=\"10\"" + str1 + " ondblclick=\"javascript:scContent.multilistMoveRight('" + this.ID + "')\" onchange=\"javascript:document.getElementById('" + this.ID + "_all_help').innerHTML=this.selectedIndex>=0?this.options[this.selectedIndex].innerHTML:''\" >");
            foreach (DictionaryEntry dictionaryEntry in unselected)
            {
                var obj = dictionaryEntry.Value;
                output.Write("<option value=\"" + obj + "\">" + obj + "</option>");
            }
            output.Write("</select>");
            output.Write("</td>");
            output.Write("<td valign=\"top\">");
            this.RenderButton(output, "Office/16x16/navigate_right.png", "javascript:scContent.multilistMoveRight('" + this.ID + "')");
            output.Write("<br />");
            this.RenderButton(output, "Office/16x16/navigate_left.png", "javascript:scContent.multilistMoveLeft('" + this.ID + "')");
            output.Write("</td>");
            output.Write("<td valign=\"top\" height=\"100%\">");
            output.Write("<select id=\"" + this.ID + "_selected\" class=\"scContentControlMultilistBox\" multiple=\"multiple\" size=\"10\"" + str1 + " ondblclick=\"javascript:scContent.multilistMoveLeft('" + this.ID + "')\" onchange=\"javascript:document.getElementById('" + this.ID + "_selected_help').innerHTML=this.selectedIndex>=0?this.options[this.selectedIndex].innerHTML:''\">");
            for (int index = 0; index < selected.Count; ++index)
            {
                var obj1 = selected[index];
                if (obj1 != null)
                {
                    output.Write("<option value=\"" + obj1 + "\">" + obj1 + "</option>");
                }
                else
                {
                    string str3 = Translate.Text("[Item not found]");
                    output.Write("<option value=\"\">" + str3 + "</option>");
                }
            }
            output.Write("</select>");
            output.Write("</td>");
            output.Write("<td valign=\"top\">");
            this.RenderButton(output, "Office/16x16/navigate_up.png", "javascript:scContent.multilistMoveUp('" + this.ID + "')");
            output.Write("<br />");
            this.RenderButton(output, "Office/16x16/navigate_down.png", "javascript:scContent.multilistMoveDown('" + this.ID + "')");
            output.Write("</td>");
            output.Write("</tr>");
            output.Write("<tr>");
            output.Write("<td valign=\"top\">");
            output.Write("<div class=\"scContentControlMultilistHelp\" id=\"" + this.ID + "_all_help\"></div>");
            output.Write("</td>");
            output.Write("<td></td>");
            output.Write("<td valign=\"top\">");
            output.Write("<div class=\"scContentControlMultilistHelp\" id=\"" + this.ID + "_selected_help\"></div>");
            output.Write("</td>");
            output.Write("<td></td>");
            output.Write("</tr>");
            output.Write("</table>");
            output.Write("</div>");
        }

        /// <summary>Renders the button.</summary>
        /// <param name="output">The output.</param>
        /// <param name="icon">The icon.</param>
        /// <param name="click">The click.</param>
        private void RenderButton(HtmlTextWriter output, string icon, string click)
        {
            Assert.ArgumentNotNull((object)output, "output");
            Assert.ArgumentNotNull((object)icon, "icon");
            Assert.ArgumentNotNull((object)click, "click");
            ImageBuilder imageBuilder = new ImageBuilder();
            imageBuilder.Src = icon;
            imageBuilder.Class = "scNavButton";
            imageBuilder.Width = 16;
            imageBuilder.Height = 16;
            imageBuilder.Margin = "2px";
            if (!this.ReadOnly)
                imageBuilder.OnClick = click;
            output.Write(imageBuilder.ToString());
        }
    }
}