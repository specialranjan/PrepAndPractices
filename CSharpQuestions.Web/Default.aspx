<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CSharpQuestions.Web._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="row">
        <div class="col-md-4">
            <h2>Task Configure Await Example with false flag</h2>
            <p>
                <asp:Label ID="lblAsyncAwaitCallMessage" runat="server"></asp:Label>
            </p>
            <p>Please go through the detail here <a>https://devblogs.microsoft.com/dotnet/configureawait-faq/</a></p>
        </div>
    </div>

</asp:Content>
