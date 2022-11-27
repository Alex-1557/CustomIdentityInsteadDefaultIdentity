Imports System.Threading
Imports BackendAPI.FrontEndModels
Imports BackendAPI.Model
Imports FrontEndData.Models
Imports Microsoft.AspNetCore.Identity

Namespace Services

    Public Class CustomRoleStore
        Implements ICustomRoleStore


        Private disposedValue As Boolean


        Public Function CreateAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of IdentityResult) Implements ICustomRoleStore.CreateAsync
            Throw New NotImplementedException()
        End Function

        Public Function DeleteAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of IdentityResult) Implements ICustomRoleStore.DeleteAsync
            Throw New NotImplementedException()
        End Function

        Public Function FindByIdAsync(roleId As String, cancellationToken As CancellationToken) As Task(Of ApplcationRole) Implements ICustomRoleStore.FindByIdAsync
            Throw New NotImplementedException()
        End Function

        Public Function FindByNameAsync(normalizedRoleName As String, cancellationToken As CancellationToken) As Task(Of ApplcationRole) Implements ICustomRoleStore.FindByNameAsync
            Throw New NotImplementedException()
        End Function

        Public Function GetNormalizedRoleNameAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of String) Implements ICustomRoleStore.GetNormalizedRoleNameAsync
            Throw New NotImplementedException()
        End Function

        Public Function GetRoleIdAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of String) Implements ICustomRoleStore.GetRoleIdAsync
            Throw New NotImplementedException()
        End Function

        Public Function GetRoleNameAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of String) Implements ICustomRoleStore.GetRoleNameAsync
            Throw New NotImplementedException()
        End Function

        Public Function SetNormalizedRoleNameAsync(role As ApplcationRole, normalizedName As String, cancellationToken As CancellationToken) As Task Implements ICustomRoleStore.SetNormalizedRoleNameAsync
            Throw New NotImplementedException()
        End Function

        Public Function SetRoleNameAsync(role As ApplcationRole, roleName As String, cancellationToken As CancellationToken) As Task Implements ICustomRoleStore.SetRoleNameAsync
            Throw New NotImplementedException()
        End Function

        Public Function UpdateAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of IdentityResult) Implements ICustomRoleStore.UpdateAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_CreateAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of IdentityResult) Implements IRoleStore(Of ApplcationRole).CreateAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_UpdateAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of IdentityResult) Implements IRoleStore(Of ApplcationRole).UpdateAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_DeleteAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of IdentityResult) Implements IRoleStore(Of ApplcationRole).DeleteAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_GetRoleIdAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of String) Implements IRoleStore(Of ApplcationRole).GetRoleIdAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_GetRoleNameAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of String) Implements IRoleStore(Of ApplcationRole).GetRoleNameAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_SetRoleNameAsync(role As ApplcationRole, roleName As String, cancellationToken As CancellationToken) As Task Implements IRoleStore(Of ApplcationRole).SetRoleNameAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_GetNormalizedRoleNameAsync(role As ApplcationRole, cancellationToken As CancellationToken) As Task(Of String) Implements IRoleStore(Of ApplcationRole).GetNormalizedRoleNameAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_SetNormalizedRoleNameAsync(role As ApplcationRole, normalizedName As String, cancellationToken As CancellationToken) As Task Implements IRoleStore(Of ApplcationRole).SetNormalizedRoleNameAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_FindByIdAsync(roleId As String, cancellationToken As CancellationToken) As Task(Of ApplcationRole) Implements IRoleStore(Of ApplcationRole).FindByIdAsync
            Throw New NotImplementedException()
        End Function

        Private Function IRoleStore_FindByNameAsync(normalizedRoleName As String, cancellationToken As CancellationToken) As Task(Of ApplcationRole) Implements IRoleStore(Of ApplcationRole).FindByNameAsync
            Throw New NotImplementedException()
        End Function

#Region "Dispose"
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects)
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ' TODO: set large fields to null
                disposedValue = True
            End If
        End Sub

        ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
        ' Protected Overrides Sub Finalize()
        '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

        Private Sub ICustomRoleStore_Dispose() Implements ICustomRoleStore.Dispose
            Throw New NotImplementedException()
        End Sub
#End Region



    End Class
End Namespace