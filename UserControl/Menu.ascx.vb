Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Data
Imports CMS_db

Partial Class UserControl_Menu
    Inherits System.Web.UI.UserControl
    Public Connsql As New CMS_db

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                Menu_BindDB()
            End If
        Catch ex As Exception
            Response.Redirect("~/Error.aspx")
        End Try
    End Sub

    Sub Menu_BindDB()
        Try
            Dim sql As String
            Dim menu As String = ""
            Dim TREE_ID As String = ""
            Dim txt As String = ""
            Dim mail As String = ""
            Dim Conn As New SqlConnection(Connsql.ConnStr())

            sql = " select c.* " & _
            " from DispatchSystem a " & _
            " inner join ROLEPROG b " & _
            " on a.Agent_ID = @Agent_Num and a.Role_id = b.Role_ID " & _
            " inner join PROGLIST c " & _
            " on b.TREE_ID = c.TREE_ID " & _
            " where c.IS_OPEN = 'Y' and a.Agent_Status !='離職' " & _
            " union all " & _
            " select * from PROGLIST " & _
            " where tree_ID in ( " & _
            "     Select  c.PARENT_ID  " & _
            "     from DispatchSystem a " & _
            "     inner join ROLEPROG b " & _
            "     on a.Agent_ID = @Agent_Num and a.Role_id = b.Role_ID " & _
            "     inner join PROGLIST c " & _
            "     on b.TREE_ID = c.TREE_ID " & _
            "     where c.IS_OPEN = 'Y' and a.Agent_Status !='離職' ) " & _
            " Order by c.TREE_ID, LEVEL_ID, SORT_ID "

            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter(sql, Conn)
            Dim ds As New DataSet
            myAdapter.SelectCommand.Parameters.Clear()
            myAdapter.SelectCommand.Parameters.Add("@Agent_Num", SqlDbType.NVarChar).Value = Session("UserID")

            'Conn.Open()   '---- 不用寫，DataAdapter會自動開啟
            myAdapter.Fill(ds, "test")

            If ds.Tables(0).Rows.Count = 0 Then
                'MsgBox( "該使用者沒有設定任何功能權限")
                '轉頁面
            Else
                Dim i As Integer
                menu += "<ul class='nav navbar-nav'>"
                For i = 0 To ds.Tables(0).Rows.Count - 1
                    Select Case ds.Tables(0).Rows(i).Item("LEVEL_ID").ToString
                        Case "2"
                            If i = 0 Then
                            Else
                                menu += "</ul>"
                                menu += "</li>"
                            End If
                            TREE_ID = ds.Tables(0).Rows(i).Item("TREE_ID").ToString
                            menu += "<li class='nav-item dropdown'><a id='" & TREE_ID & "' class='nav-link dropdown-toggle' data-toggle='dropdown' aria-haspopup='true' role='button' aria-expanded='false'>" &
                                "<span class='glyphicon glyphicon-" & ds.Tables(0).Rows(i).Item("IMAGE_FILE").ToString & "'></span>&nbsp;" & ds.Tables(0).Rows(i).Item("TREE_NAME").ToString & "&nbsp;<span class='caret'></span></a>" &
                                "<ul class='dropdown-menu' role='menu' aria-labelledby='" & TREE_ID & "'>"
                        Case "3"
                            menu += "<li role='presentation'><a role='menuitem' class='dropdown-item' href='../" & ds.Tables(0).Rows(i).Item("NAVIGATE_URL").ToString &
                                "'>" & ds.Tables(0).Rows(i).Item("TREE_NAME").ToString & "</a></li>"
                        Case Else
                    End Select
                Next
                menu += "</ul>"
                the_Menu.InnerHtml = menu

            End If

            Select Case Session("Agent_LV")
                Case "10"
                    txt = "一般員工"
                Case "20"
                    txt = "部門主管"
                Case "30"
                    txt = "管理人員"
                Case Else
                    txt = "一般員工"
            End Select

            If Session("Agent_Mail").ToString().Length = 0 Then
                mail = "沒有電子信箱"
            Else
                mail = Session("Agent_Mail")
            End If

            menu = ""
            menu += "<span class='glyphicon glyphicon-user'></span>&nbsp;登入人員：" & Session("UserIDNAME") & "&nbsp;<span class='caret'></span>"
            agent_info.InnerHtml = menu

            menu = ""
            menu += "<li role='presentation'><a class='dropdown-item' role='menuitem'>&nbsp;所屬公司：" & Session("Agent_Company") & "&nbsp;&nbsp;</a></li>"
            menu += "<li role='presentation'><a class='dropdown-item' role='menuitem'>&nbsp;所屬部門：" & Session("Agent_Team") & "&nbsp;&nbsp;</a></li>"
            menu += "<li role='presentation'><a class='dropdown-item' role='menuitem'>&nbsp;人員編號：" & Session("UserID") & "&nbsp;&nbsp;</a></li>"
            menu += "<li role='presentation'><a class='dropdown-item' role='menuitem'>&nbsp;人員姓名：" & Session("UserIDNAME") & "&nbsp;&nbsp;</a></li>"
            menu += "<li role='presentation'><a class='dropdown-item' role='menuitem'>&nbsp;人員權限：" & txt & "&nbsp;&nbsp;</a></li>"
            menu += "<li role='presentation'><a class='dropdown-item' role='menuitem'>&nbsp;電子信箱：" & mail & "&nbsp;&nbsp;</a></li>"
            user_li.InnerHtml = menu
        Catch ex As Exception
            Response.Redirect("~/Error.aspx")
        End Try
    End Sub

    Protected Sub Logout(sender As Object, e As System.EventArgs)
        Session.Clear()
        Session.RemoveAll()
        Session.Abandon()
        Response.Redirect("~/Logout.aspx")
    End Sub
End Class

