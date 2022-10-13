Imports CMS_db
Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System

Partial Class Login
    Inherits System.Web.UI.Page

    Public Shadows Connsql As New CMS_db

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Session.Timeout = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings("SessionTimeout"))
        txt_id.Focus()

    End Sub

    Private Function CheckCSRUser() As Boolean

        Dim Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(txt_pwd.Text, "MD5").ToUpper()
        Dim sql As String = ""
        sql = " SELECT SYSID, Agent_ID, Agent_Name, Agent_Company, Agent_Team, Agent_Mail, Agent_Phone, Role_ID, Agent_LV, Password FROM DispatchSystem WHERE UserID = @UserID AND Agent_Status != '離職'"
        Try

            Dim Conn As New SqlConnection(Connsql.ConnStr())
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter(sql, Conn)
            Dim ds As New DataSet

            myAdapter.SelectCommand.Parameters.Clear()
            myAdapter.SelectCommand.Parameters.Add("@UserID", SqlDbType.NVarChar).Value = txt_id.Text
            myAdapter.Fill(ds, "test")

            If ds.Tables(0).Rows.Count > 0 Then

                If Password = ds.Tables(0).Rows(0).Item("Password").ToString Then

                    Session("MD5") = Password
                    Session("p") = txt_pwd.Text
                    Session("SYSID") = ds.Tables(0).Rows(0).Item("SYSID").ToString
                    Session("UserIDNAME") = ds.Tables(0).Rows(0).Item("Agent_Name").ToString
                    Session("Agent_Company") = ds.Tables(0).Rows(0).Item("Agent_Company").ToString
                    Session("Agent_Team") = ds.Tables(0).Rows(0).Item("Agent_Team").ToString
                    Session("Agent_Phone") = ds.Tables(0).Rows(0).Item("Agent_Phone").ToString
                    Session("UserIDNO") = ds.Tables(0).Rows(0).Item("Agent_ID").ToString
                    Session("UserID") = ds.Tables(0).Rows(0).Item("Agent_ID").ToString
                    Session("RoleID") = ds.Tables(0).Rows(0).Item("ROLE_ID").ToString
                    Session("Agent_LV") = ds.Tables(0).Rows(0).Item("Agent_LV").ToString
                    If ds.Tables(0).Rows(0).Item("Agent_Mail").ToString = "" Then
                        Session("Agent_Mail") = "0"
                    Else
                        Session("Agent_Mail") = "1"
                    End If

                    Return True
                Else
                    Response.Write("<script>alert('登入密碼錯誤！')</script>")
                    Return False
                End If

            Else
                Response.Write("<script>alert('登入帳號不存在！')</script>")
                Return False
            End If
            Conn.Close()
        Catch ex As Exception
            Connsql.WriteErrorLog(ex)
            Response.Redirect("~/ExHandle.aspx")
        Finally
        End Try
        Return False
    End Function

    Protected Sub UserLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserLogin.Click
        If CheckCSRUser() Then
            Response.Redirect("~/Dashboard.aspx")
        End If
    End Sub

End Class
