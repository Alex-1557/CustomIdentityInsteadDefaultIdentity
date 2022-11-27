Imports System.ComponentModel.DataAnnotations
Namespace FrontEndModels
    Public Class LoginViewModel
        Public Property Username As String
        <DataType(DataType.Password)>
        Public Property Password As String
        <Display(Name:="Remember Me")>
        Public Property RememberMe As Boolean
        Public Property ReturnUrl As String
    End Class

End Namespace