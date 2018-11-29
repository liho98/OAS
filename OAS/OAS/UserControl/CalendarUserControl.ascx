<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarUserControl.ascx.cs" Inherits="OAS.UserControl.CalendarUserControl" %>
<asp:Calendar ID="Calendar" runat="server" OnSelectionChanged="Calendar_SelectionChanged"></asp:Calendar>
<div>
    <asp:TextBox ID="txtDate" runat="server" placeholder="Date of Birth">
    </asp:TextBox>

    <asp:LinkButton ID="Literal" runat="server" OnClick="CalendarBtn_Click" Style="color: rgb(100,100,100); text-decoration: none; position: absolute; transform: translate(2100%,-170%);">
    <span class="fa fa-calendar"></span>
    </asp:LinkButton>
</div>

<asp:RequiredFieldValidator ID="txtDate_RequiredFieldValidator"
    ControlToValidate="txtDate"
    Display="Static"
    ForeColor="Red" style="display:none"
    ValidationGroup="ValidateGroup"
    ErrorMessage="Please select a date."
    runat="server" />
<asp:CompareValidator runat="server" ControlToValidate="txtDate" ErrorMessage="Invalid Date Format (mm/dd/yyyy)" style="display:none" ValidateEmptyText="True"
    Type="Date" Operator="DataTypeCheck" Display="Static" ForeColor="Red" ValidationGroup="ValidateGroup">
</asp:CompareValidator>
