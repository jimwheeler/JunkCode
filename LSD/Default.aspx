<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LaserSuppliesDashBoard.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
    
    <script type="text/javascript">
        (function ($) {
            $(function () { //on DOM ready 
                $("#scroller").simplyScroll();
            });
        })(jQuery);
    </script>


    <table style="width:100%; margin-bottom:10px; border:none;">
        <tr style="border:none;"><td class="TitleHeader1">Laser Supplies - Responding To Customer Escalations</td>
            <td class="TitleHeader2">
            <img src="Images/Question.png" />
            </td></tr>
    </table>


    <div style="margin-left:10%; margin-right:10%">
    <table class="TableRow">
        <tr><td class="TableHeader">Laser Supplies - Customer Escalation Dashboard</td><td class="TargetBox">Answer To Customer Target = 30 Days</td></tr>
    </table>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick"></asp:Timer>
                <asp:GridView ID="GridView1" runat="server" CssClass="GridView" CellPadding="5" OnDataBound="GridView1_DataBound">
                    <HeaderStyle BackColor="Black" CssClass="GridViewHeader" />
                </asp:GridView>
        </ContentTemplate> 
    </asp:UpdatePanel>
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
        <ContentTemplate>
            <ul id="scroller">
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>



    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Timer ID="Timer2" runat="server" OnTick="Timer2_Tick"></asp:Timer>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
