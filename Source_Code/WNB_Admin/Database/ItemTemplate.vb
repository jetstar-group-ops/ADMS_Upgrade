Public Class ItemTemplate
    Implements ITemplate
    Private m_ColumnName As String
    Private m_TemplateType As ListItemType
    Private m_ControlPostBack As Boolean
    Private m_ControlWidth As Integer
    'Event CheckBoxCheckedChanged(ByVal sender As CheckBox, ByVal e As EventArgs)
    Public Property TemplateType() As String
        Get
            Return m_TemplateType
        End Get
        Set(ByVal value As String)
            m_TemplateType = value
        End Set
    End Property


    'Event CheckBoxCheckedChanged(ByVal sender As CheckBox, ByVal e As EventArgs)
    Public Property ColumnName() As String
        Get
            Return m_ColumnName
        End Get
        Set(ByVal value As String)
            m_ColumnName = value
        End Set
    End Property

  

    Public Property ControlPostBack() As Boolean
        Get
            Return m_ControlPostBack
        End Get
        Set(ByVal value As Boolean)
            m_ControlPostBack = value
        End Set
    End Property

    Public Property ControlWidth() As Integer
        Get
            Return m_ControlWidth
        End Get
        Set(ByVal value As Integer)
            m_ControlWidth = value
        End Set
    End Property

    Public Sub New()
    End Sub
    Public Sub New(ByVal ColumnName As String, ByVal TemplateType As String, ByVal ControlPostBack As Boolean)
        Me.ColumnName = ColumnName
        Me.TemplateType = TemplateType
        Me.ControlPostBack = ControlPostBack
    End Sub
    Private Sub InstantiateIn(ByVal ThisColumn As System.Web.UI.Control) _
            Implements ITemplate.InstantiateIn

        Select Case TemplateType
            Case ListItemType.Header
                Dim lc As New Literal()
                lc.Text = ColumnName
                ThisColumn.Controls.Add(lc)
            Case ListItemType.Item
                Dim DropDownItem As New DropDownList()
                DropDownItem.ID = "ddl" & ColumnName
                DropDownItem.AutoPostBack = ControlPostBack

                DropDownItem.Items.Insert(0, New ListItem("", ""))
                DropDownItem.Items.Insert(1, New ListItem("Yes", "1"))
                DropDownItem.Items.Insert(2, New ListItem("No", "0"))

                AddHandler DropDownItem.DataBinding, AddressOf DropDownItem_DataBinding
                'AddHandler CheckBoxItem.CheckedChanged, AddressOf CheckBoxItem_CheckedChanged
                ThisColumn.Controls.Add(DropDownItem)

        End Select


        
    End Sub
    'Private Sub CheckBoxItem_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    RaiseEvent CheckBoxCheckedChanged(sender, e)
    'End Sub
    Private Sub DropDownItem_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim DropDownItem As DropDownList = DirectCast(sender, DropDownList)
        Dim CurrentRow As GridViewRow = DirectCast(DropDownItem.NamingContainer, GridViewRow)
        Dim CurrentDataItem As Object = DataBinder.Eval(CurrentRow.DataItem, ColumnName)
        If Not CurrentDataItem Is DBNull.Value Then
            DropDownItem.SelectedValue = Convert.ToString(CurrentDataItem)
        End If
    End Sub


End Class
